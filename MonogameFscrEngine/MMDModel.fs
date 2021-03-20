namespace MMD
open Microsoft.Xna.Framework
open System.IO
open Microsoft.Xna.Framework.Graphics

module TestCSVModel =
  type ModelBuffer (device) =
    let mutable (vBuf:VertexBuffer) = null
    let mutable (iBuf:IndexBuffer) = null
    let effect = new BasicEffect(device)
    
    member t.MakeVBuf (vList:float32 array) (indList:int array) = 
      let bind = // new VertexDeclaration(velem,nelem , uvelem)
        //VertexPositionColorTexture.VertexDeclaration
        VertexPositionNormalTexture.VertexDeclaration
      if vBuf <> null then
        vBuf.Dispose()
      vBuf <- new VertexBuffer(device ,bind , vList.Length / 8 , BufferUsage.WriteOnly)
      vBuf.SetData(vList)
      let indCount = Array.length indList
      let bitSize = if indCount > 65535 then IndexElementSize.SixteenBits else IndexElementSize.ThirtyTwoBits
      if iBuf <> null then
        iBuf.Dispose()
      iBuf <- new IndexBuffer(device , bitSize , indCount , BufferUsage.WriteOnly)
      iBuf.SetData(indList)
    member this.Draw view proj =
      if vBuf <> null then
        device.SetVertexBuffer(vBuf)
        device.Indices <-(iBuf)
        effect.World <- Matrix.Identity
        effect.View <- view
        effect.Projection <- proj
        effect.LightingEnabled <- true
        effect.EnableDefaultLighting()
        effect.TextureEnabled <- false
        //effect.DiffuseColor <- new Vector3(0.5f,0.5f,0.5f)
        //effect.DirectionalLight0.Direction <- new Vector3(0.1f,0.5f,0.8f)
        //effect.DirectionalLight0.DiffuseColor <- new Vector3(0.1f,0.5f,0.8f)
        //effect.DirectionalLight0.Enabled <- true
        //effect.SpecularColor <- new Vector3(0.0f,1.0f,0.0f)
        try
          for i in effect.CurrentTechnique.Passes do
            i.Apply()
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0 , iBuf.IndexCount / 3);
        with e -> System.Diagnostics.Debug.WriteLine e
  let loadCSV path = 
    let lines = File.ReadAllLines path
    let csvDatas = [for i in lines -> [for s  in i.Split "," -> s]]
    let mutable verts = []
    let mutable inds = []
    for line in csvDatas do
      if line.[0] = "Vertex" then
        verts <- verts @ [float32 line.[2] ; float32 line.[3] ; float32 line.[4] ;
                          //1.14437e-28f
                          float32 line.[5] ; float32 line.[6] ; float32 line.[7] ;
                          float32 line.[9] ; float32 line.[10]  ;
                          ]

      if line.[0] = "Face" then
        inds <- inds @ [int line.[5] ; int line.[4] ; int line.[3]]
    verts |>List.toArray,inds |> List.toArray
  let loadModel path device=
    let buf = new ModelBuffer(device)
    let vert,ind = loadCSV path
    buf.MakeVBuf vert ind
    buf
  let test device = loadModel @"testcube.csv" device

(*
module MMDModel=
  let loadFromAssem typ path =
    let assembly = Assembly.GetExecutingAssembly()
    use fs = assembly.GetManifestResourceStream( typ , path )
    let buffer = Array.zeroCreate fs.Length
    fs.Read(buffer , 0 , buffer.Length)
    buffer

  type PMXModel (device , path , skinning = null, defaultMaterialShader = null ) =
    let loadFromAssem = loadFromAssem t.GetType()
    let mutable worldMatrix = Matrix.Identity
    let MaximumNumberOfBones = 768
    let mutable boneList = []
    let mutable matList = []
    let mutable morphList = []
    let mutable rootBoneList = []
    let mutable ikBoneList = []
    let mutable vertControl = null
    let mutable indList = []
    let mutable format = null

    let mutable skinningShader = None
    let mutable releaseSkinning = false

    let mutable vertexBuf = null
    let mutable vertexLay = null

    let (+@) x y = x := !x + y
    let compare (x:PMXBoneControl) (y:PMXBoneControl) =
      // 後であればあるほどスコアが大きくなるように計算する
      let xScore = ref 0
      let yScore = ref 0
      let count = boneList.Length
      if x.PMXFBourne.PostPhysicalDeformation then
        xScore += count * count
      if y.PMXFBourne.PostPhysicalDeformation then
        yScore += count * count
      xScore +@ count * x.TransformationHierarchy
      yScore +@ count * y.TransformationHierarchy
      xScore +@ x.BoneIndex
      yScore +@ y.BoneIndex
      xScore - yScore

    let readAndInit device stream skinning defaultMaterialShader onOpen =
      format <- new PMXFormat.Model( stream)
      // todo 骨が多すぎたらpanic
      stream.Dispose()
      boneList <- [for i in format.BoneList -> i ]
      for i in boneList do
        i.PostReadProc boneList
      let isIK bone = bone.PMXFBourne.IKBone
      ikBoneList <- boneList |> List.filter isIK
      let isRoot bone = bone.PMXFBourne.ParentBoneIndex = -1
      rootBoneList <- boneList |> List.filter isRoot
      let rec setting (bone:PMXBoneControl) layer =
        bone.TransformationHierarchy <- layer
        for child in bone.ChildBoneList
          setting child (layer + 1)
      for i in rootBoneList do
        setting i 0
      List.sortBy ikBoneList compare
      List.sortBy rootBoneList compare
      // 親付与によるFKを初期化する。
      let fkTransByParentAdd = FkTransByParentAdd boneList
      let PhysicUpdater = PMXPhysicUpdater boneList
      matList <- [for i in format.MatList -> createMat i defaultMaterialShader
      morphList <- [for i in format.MorphList -> createMorph i]
      // PMXVertexControl
      let vertList = [for i in format.VertexList -> createVertLayout i ] 
      vertControl <- createVertControl vertList

      let skinDec =
	let sizeInbyte = VS_INPUT.SizeInBytes * this.PMXVertexControl.InputVertexArray.Length
	let usage = ResourceUsage.Default
	let bind = BindFlags.VertexBuffer | BindFlags.ShaderResource | BindFlags.UnorderedAccess
	let cpu = CpuAccessFlags.None
	let opt = ResourceOptionFlags.BufferAllowRawViews
        GPUUtil.bufDec sizeInbyte usage bind cpu opt 0 // ?
      let skinBuf = new SharpDX.Direct3D11.Buffer(device , skinDec)
      let vertBufferUAV =
        let vbUAVBuf = uavBufDesc 0 (VS_INPUT.SizeInBytes * this.PMXVertexControl.InputVertexArray.Length / 4) UnorderedAccessViewBufferFlags.Raw
        let vbUAV = GPUUtil.uavDesc r32Typeless UnorderedAccessViewDimension.Buffer vbUAVBuf
        new UnorderedAccessView( device , skinBuf , vbUAV)
      // 頂点レイアウト作成
      loadFromAssem "Resources.Shaders.DefaultVertexShaderForObject.cso"


    let pmxModel device path , skinning defaultMaterialShader =
      use stream = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
      readAndInit device stream skinning defaultMaterialShader onOpen
*)
