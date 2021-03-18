namespace MMD
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
