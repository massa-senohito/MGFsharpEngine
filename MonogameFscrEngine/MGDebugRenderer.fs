namespace MonoEng

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework
open System.Diagnostics

module MGDebugRenderer =
  type DebugRender(device:GraphicsDevice) =
    let mutable (vBuf:VertexBuffer) = null
    let effect = new BasicEffect(device)
    member this.AddLine(lineList:Vector4 array) =
      let velem = new VertexElement(0,VertexElementFormat.Vector4,VertexElementUsage.Position,0)
      //let celem = new VertexElement(16,VertexElementFormat.Color,VertexElementUsage.Color,0)
      let bind = new VertexDeclaration(velem)//,celem)
      if vBuf <> null then
        vBuf.Dispose()
      vBuf <- new VertexBuffer(device,bind,lineList.Length,BufferUsage.WriteOnly)
      vBuf.SetData(lineList)
    member this.Draw() =
      MXFUtil.useDefaultDepth device
      if vBuf <> null then
        //Debug.WriteLine "Debug SetVertexBuffer"
        device.SetVertexBuffer(vBuf)
        for i in effect.CurrentTechnique.Passes do
          i.Apply()
          device.DrawPrimitives(PrimitiveType.LineList, 0, vBuf.VertexCount / 2);

