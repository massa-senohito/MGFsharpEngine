module MGDevice
open Autofac
open Autofac.Extras.DynamicProxy
open System.Diagnostics
open Castle.DynamicProxy
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System

type CallLogger() =
  interface IInterceptor with
    member t.Intercept invocation =
      let args = System.String.Join( "," , [ for i in invocation.Arguments -> if i = null then "" else string i] )
      // 対象class
      let target = invocation.InvocationTarget
      Debug.WriteLine invocation.Method.Name
      Debug.WriteLine args
      invocation.Proceed()
      // invocation.ReturnValue
      ()
type IMGDevice =
  abstract member SetVertexBufferOffset :  VertexBuffer ->  Int32 -> unit
  abstract member SetVertexBuffers :  VertexBufferBinding[] -> unit
  abstract member set_Indices :  IndexBuffer -> unit
  abstract member get_Indices : unit -> IndexBuffer
  abstract member get_ResourcesLost : unit ->Boolean
  abstract member set_ResourcesLost :  Boolean -> unit
  abstract member DrawIndexedPrimitivesSi :  PrimitiveType ->  Int32 ->  Int32 ->  Int32 ->  Int32 ->  Int32 -> unit
  abstract member DrawIndexedPrimitives :  PrimitiveType ->  Int32 ->  Int32 ->  Int32 -> unit
  //abstract member DrawUserPrimitivesGen :  PrimitiveType ->  T[] ->  Int32 ->  Int32 -> unit
  //abstract member DrawUserPrimitives :  PrimitiveType ->  T[] ->  Int32 ->  Int32 ->  VertexDeclaration -> unit
  abstract member DrawPrimitives :  PrimitiveType ->  Int32 ->  Int32 -> unit
  //abstract member DrawUserIndexedPrimitives :  PrimitiveType ->  T[] ->  Int32 ->  Int32 ->  Int16[] ->  Int32 ->  Int32 -> unit
  //abstract member DrawUserIndexedPrimitives :  PrimitiveType ->  T[] ->  Int32 ->  Int32 ->  Int16[] ->  Int32 ->  Int32 ->  VertexDeclaration -> unit
  //abstract member DrawUserIndexedPrimitives :  PrimitiveType ->  T[] ->  Int32 ->  Int32 ->  Int32[] ->  Int32 ->  Int32 -> unit
  //abstract member DrawUserIndexedPrimitives :  PrimitiveType ->  T[] ->  Int32 ->  Int32 ->  Int32[] ->  Int32 ->  Int32 ->  VertexDeclaration -> unit
  abstract member DrawInstancedPrimitivesVert :  PrimitiveType ->  Int32 ->  Int32 ->  Int32 ->  Int32 ->  Int32 ->  Int32 -> unit
  abstract member DrawInstancedPrimitives :  PrimitiveType ->  Int32 ->  Int32 ->  Int32 ->  Int32 -> unit
  abstract member DrawInstancedPrimitivesBase :  PrimitiveType ->  Int32 ->  Int32 ->  Int32 ->  Int32 ->  Int32 -> unit
  //abstract member GetBackBufferData<'T when 'T : struct> :  'T[] -> unit
  //abstract member GetBackBufferDataIndex<'T> :  'T[] ->  Int32 ->  Int32 -> unit
  //abstract member GetBackBufferDataRect<'T> : Nullable<Microsoft.Xna.Framework.Rectangle> -> 'T[] ->  Int32 ->  Int32 -> unit
  abstract member get_Handle : Object
  //abstract member SetRenderTarget :  RenderTarget2D ->  Int32 -> unit
  //abstract member SetRenderTarget :  RenderTarget3D ->  Int32 -> unit
  abstract member Flush : unit -> unit
  abstract member get_UseHalfPixelOffset : Boolean
  abstract member get_VertexTextures : TextureCollection
  abstract member get_VertexSamplerStates : SamplerStateCollection
  abstract member get_Textures : TextureCollection
  abstract member get_SamplerStates : SamplerStateCollection
  //abstract member get_DiscardColor : Color
  //abstract member set_DiscardColor :  Color -> unit
  abstract member add_DeviceLost : EventHandler<EventArgs> -> unit
  abstract member remove_DeviceLost : EventHandler<EventArgs> -> unit
  abstract member add_DeviceReset : EventHandler<EventArgs> -> unit
  abstract member remove_DeviceReset : EventHandler<EventArgs> -> unit
  abstract member add_DeviceResetting : EventHandler<EventArgs> -> unit
  abstract member remove_DeviceResetting : EventHandler<EventArgs> -> unit
  abstract member add_ResourceCreated : EventHandler<Graphics.ResourceCreatedEventArgs>  -> unit
  abstract member remove_ResourceCreated : EventHandler<Graphics.ResourceCreatedEventArgs>  -> unit
  abstract member add_ResourceDestroyed : EventHandler<Graphics.ResourceDestroyedEventArgs>  -> unit
  abstract member remove_ResourceDestroyed : EventHandler<Graphics.ResourceDestroyedEventArgs>  -> unit
  abstract member add_Disposing : EventHandler<EventArgs> -> unit
  abstract member remove_Disposing : EventHandler<EventArgs> -> unit
  abstract member get_IsDisposed : Boolean
  abstract member get_IsContentLost : Boolean
  abstract member get_Adapter : GraphicsAdapter
  abstract member get_Metrics : GraphicsMetrics
  abstract member set_Metrics :  GraphicsMetrics -> unit
  abstract member get_GraphicsDebug : GraphicsDebug
  abstract member set_GraphicsDebug :  GraphicsDebug -> unit
  abstract member get_RasterizerState : RasterizerState
  abstract member set_RasterizerState :  RasterizerState -> unit
  abstract member get_BlendFactor : Color
  abstract member set_BlendFactor :  Color -> unit
  abstract member get_BlendState : BlendState
  abstract member set_BlendState :  BlendState -> unit
  abstract member get_DepthStencilState : DepthStencilState
  abstract member set_DepthStencilState :  DepthStencilState -> unit
  abstract member Clear :  Color -> unit
  abstract member ClearDepthStencil :  ClearOptions ->  Color ->  Single ->  Int32 -> unit
  abstract member ClearV4DepthStencil :  ClearOptions ->  Vector4 ->  Single ->  Int32 -> unit
  abstract member Dispose : unit -> unit
  abstract member Present : unit -> unit
  abstract member Reset : unit -> unit
  abstract member ResetParam :  PresentationParameters -> unit
  abstract member get_DisplayMode : DisplayMode
  abstract member get_GraphicsDeviceStatus : GraphicsDeviceStatus
  abstract member get_PresentationParameters : PresentationParameters
  abstract member get_Viewport : Viewport
  abstract member set_Viewport :  Viewport -> unit
  abstract member get_GraphicsProfile : GraphicsProfile
  abstract member get_ScissorRectangle : Rectangle
  abstract member set_ScissorRectangle :  Rectangle -> unit
  abstract member get_RenderTargetCount : Int32
  abstract member SetRenderTarget :  RenderTarget2D -> unit
  abstract member SetRenderTargetArray :  RenderTarget2D -> int -> unit
  abstract member SetRenderTarget3D :  RenderTarget3D -> int -> unit
  abstract member SetRenderTargetCube :  RenderTargetCube ->  CubeMapFace -> unit
  abstract member SetRenderTargetsArray :  RenderTargetBinding[] -> unit
  abstract member GetRenderTargets : RenderTargetBinding[]
  abstract member GetRenderTargetsToArray :  RenderTargetBinding[] -> unit
  abstract member SetVertexBuffer :  VertexBuffer -> unit

type MGDevice(device:GraphicsDevice) =
  interface IMGDevice with
      member t.SetVertexBufferOffset ( vertexBuffer : Microsoft.Xna.Framework.Graphics.VertexBuffer )  ( vertexOffset : System.Int32 ) = 
        device.SetVertexBuffer( vertexBuffer ,  vertexOffset )
      member t.SetVertexBuffers ( vertexBuffers : Microsoft.Xna.Framework.Graphics.VertexBufferBinding[] ) = 
        device.SetVertexBuffers( vertexBuffers )
      member t.set_Indices ( value : Microsoft.Xna.Framework.Graphics.IndexBuffer ) = 
        device.set_Indices( value )
      member t.get_Indices() = 
        device.get_Indices()
      member t.get_ResourcesLost() = 
        device.get_ResourcesLost()
      member t.set_ResourcesLost ( value : System.Boolean )  = 
        device.set_ResourcesLost( value )
      member t.DrawIndexedPrimitivesSi ( primitiveType : PrimitiveType )  ( baseVertex : Int32 )  ( minVertexIndex : Int32 )  ( numVertices : Int32 )  ( startIndex : Int32 )  ( primitiveCount : Int32 )  = 
        device.DrawIndexedPrimitives( primitiveType ,  baseVertex ,  minVertexIndex ,  numVertices ,  startIndex ,  primitiveCount )
      member t.DrawIndexedPrimitives ( primitiveType : PrimitiveType )  ( baseVertex : Int32 )  ( startIndex : Int32 )  ( primitiveCount : Int32 )  = 
        device.DrawIndexedPrimitives( primitiveType ,  baseVertex ,  startIndex ,  primitiveCount )
      // IVertex 制約が必要
      //member t.DrawUserPrimitivesGen ( primitiveType : PrimitiveType )  ( vertexData : 'T[] )  ( vertexOffset : Int32 )  ( primitiveCount : Int32 )  = 
      //  device.DrawUserPrimitives( primitiveType ,  vertexData ,  vertexOffset ,  primitiveCount )
      //member t.DrawUserPrimitives ( primitiveType : PrimitiveType )  ( vertexData : 'T[] )  ( vertexOffset : Int32 )  ( primitiveCount : Int32 )  ( vertexDeclaration : Microsoft.Xna.Framework.Graphics.VertexDeclaration )  = 
      //  device.DrawUserPrimitives( primitiveType ,  vertexData ,  vertexOffset ,  primitiveCount ,  vertexDeclaration )
      member t.DrawPrimitives ( primitiveType : PrimitiveType )  ( vertexStart : Int32 )  ( primitiveCount : Int32 )  = 
        device.DrawPrimitives( primitiveType ,  vertexStart ,  primitiveCount )
      //member t.DrawUserIndexedPrimitives ( primitiveType : Graphics.PrimitiveType )  ( vertexData : 'T[] )  ( vertexOffset : Int32 )  ( numVertices : Int32 )  ( indexData : Int16[] )  ( indexOffset : Int32 )  ( primitiveCount : Int32 )  = 
      //  device.DrawUserIndexedPrimitives( primitiveType ,  vertexData ,  vertexOffset ,  numVertices ,  indexData ,  indexOffset ,  primitiveCount )
      //member t.DrawUserIndexedPrimitives ( primitiveType : Graphics.PrimitiveType )  ( vertexData : 'T[] )  ( vertexOffset : System.Int32 )  ( numVertices : System.Int32 )  ( indexData : System.Int16[] )  ( indexOffset : System.Int32 )  ( primitiveCount : System.Int32 )  ( vertexDeclaration : Graphics.VertexDeclaration )  = 
      //  device.DrawUserIndexedPrimitives( primitiveType ,  vertexData ,  vertexOffset ,  numVertices ,  indexData ,  indexOffset ,  primitiveCount ,  vertexDeclaration )
      //member t.DrawUserIndexedPrimitives ( primitiveType : Graphics.PrimitiveType )  ( vertexData : 'T[] )  ( vertexOffset : System.Int32 )  ( numVertices : System.Int32 )  ( indexData : System.Int32[] )  ( indexOffset : System.Int32 )  ( primitiveCount : System.Int32 )  = 
      //  device.DrawUserIndexedPrimitives( primitiveType ,  vertexData ,  vertexOffset ,  numVertices ,  indexData ,  indexOffset ,  primitiveCount )
      //member t.DrawUserIndexedPrimitives ( primitiveType : Graphics.PrimitiveType )  ( vertexData : 'T[] )  ( vertexOffset : System.Int32 )  ( numVertices : System.Int32 )  ( indexData : System.Int32[] )  ( indexOffset : System.Int32 )  ( primitiveCount : System.Int32 )  ( vertexDeclaration : Graphics.VertexDeclaration )  = 
      //  device.DrawUserIndexedPrimitives( primitiveType ,  vertexData ,  vertexOffset ,  numVertices ,  indexData ,  indexOffset ,  primitiveCount ,  vertexDeclaration )
      member t.DrawInstancedPrimitivesVert ( primitiveType : Graphics.PrimitiveType )  ( baseVertex : System.Int32 )  ( minVertexIndex : System.Int32 )  ( numVertices : System.Int32 )  ( startIndex : System.Int32 )  ( primitiveCount : System.Int32 )  ( instanceCount : System.Int32 )  = 
        device.DrawInstancedPrimitives( primitiveType ,  baseVertex ,  minVertexIndex ,  numVertices ,  startIndex ,  primitiveCount ,  instanceCount )
      member t.DrawInstancedPrimitives ( primitiveType : Graphics.PrimitiveType )  ( baseVertex : System.Int32 )  ( startIndex : System.Int32 )  ( primitiveCount : System.Int32 )  ( instanceCount : System.Int32 )  = 
        device.DrawInstancedPrimitives( primitiveType ,  baseVertex ,  startIndex ,  primitiveCount ,  instanceCount )
      member t.DrawInstancedPrimitivesBase ( primitiveType : Graphics.PrimitiveType )  ( baseVertex : System.Int32 )  ( startIndex : System.Int32 )  ( primitiveCount : System.Int32 )  ( baseInstance : System.Int32 )  ( instanceCount : System.Int32 )  = 
        device.DrawInstancedPrimitives( primitiveType ,  baseVertex ,  startIndex ,  primitiveCount ,  baseInstance ,  instanceCount )
      //member t.GetBackBufferData<'T when 'T : struct> ( data : 'T[] )  = 
      //  device.GetBackBufferData( data )
      //member t.GetBackBufferDataIndex <'T when 'T : struct>( data : 'T[] )  ( startIndex : System.Int32 )  ( elementCount : System.Int32 )  = 
      //  device.GetBackBufferData( data ,  startIndex ,  elementCount )
      //member t.GetBackBufferDataRect <'T when 'T : struct>( rect : Nullable<Microsoft.Xna.Framework.Rectangle>)  ( data : 'T[] )  ( startIndex : System.Int32 )  ( elementCount : System.Int32 )  = 
      //  device.GetBackBufferData( rect ,  data ,  startIndex ,  elementCount )
      member t.get_Handle = 
        device.get_Handle()
      member t.SetRenderTargetArray ( renderTarget : Graphics.RenderTarget2D )  ( arraySlice : System.Int32 )  = 
        device.SetRenderTarget( renderTarget ,  arraySlice )
      member t.SetRenderTarget3D ( renderTarget : Graphics.RenderTarget3D )  ( arraySlice : System.Int32 )  = 
        device.SetRenderTarget( renderTarget ,  arraySlice )
      member t.Flush() = 
        device.Flush()
      member t.get_UseHalfPixelOffset = 
        device.get_UseHalfPixelOffset()
      member t.get_VertexTextures = 
        device.get_VertexTextures()
      member t.get_VertexSamplerStates = 
        device.get_VertexSamplerStates()
      member t.get_Textures = 
        device.get_Textures()
      member t.get_SamplerStates = 
        device.get_SamplerStates()
      //member t.get_DiscardColor() = 
      //  device.DiscardColor()
      //member t.set_DiscardColor ( value : Microsoft.Xna.Framework.Color )  = 
      //  device.set_DiscardColor( value )
      member t.add_DeviceLost ( value : EventHandler<EventArgs> )  = 
        device.add_DeviceLost( value )
      member t.remove_DeviceLost ( value : EventHandler<EventArgs> )  = 
        device.remove_DeviceLost( value )
      member t.add_DeviceReset ( value : EventHandler<EventArgs> )  = 
        device.add_DeviceReset( value )
      member t.remove_DeviceReset ( value : EventHandler<EventArgs> ) = 
        device.remove_DeviceReset( value )
      member t.add_DeviceResetting ( value : EventHandler<EventArgs> ) = 
        device.add_DeviceResetting( value )
      member t.remove_DeviceResetting ( value : EventHandler<EventArgs> ) = 
        device.remove_DeviceResetting( value )
      member t.add_ResourceCreated ( value : EventHandler<Graphics.ResourceCreatedEventArgs> ) = 
        device.add_ResourceCreated( value )
      member t.remove_ResourceCreated ( value : EventHandler<Graphics.ResourceCreatedEventArgs> ) = 
        device.remove_ResourceCreated( value )
      member t.add_ResourceDestroyed ( value : EventHandler<Graphics.ResourceDestroyedEventArgs> ) = 
        device.add_ResourceDestroyed( value )
      member t.remove_ResourceDestroyed ( value : EventHandler<Graphics.ResourceDestroyedEventArgs> ) = 
        device.remove_ResourceDestroyed( value )
      member t.add_Disposing ( value : EventHandler<EventArgs> )  = 
        device.add_Disposing( value )
      member t.remove_Disposing ( value : EventHandler<EventArgs> )  = 
        device.remove_Disposing( value )
      member t.get_IsDisposed = 
        device.get_IsDisposed()
      member t.get_IsContentLost = 
        device.get_IsContentLost()
      member t.get_Adapter = 
        device.get_Adapter()
      member t.get_Metrics = 
        device.get_Metrics()
      member t.set_Metrics ( value : Microsoft.Xna.Framework.Graphics.GraphicsMetrics )  = 
        device.set_Metrics( value )
      member t.get_GraphicsDebug = 
        device.get_GraphicsDebug()
      member t.set_GraphicsDebug ( value : Microsoft.Xna.Framework.Graphics.GraphicsDebug )  = 
        device.set_GraphicsDebug( value )
      member t.get_RasterizerState = 
        device.get_RasterizerState()
      member t.set_RasterizerState ( value : Microsoft.Xna.Framework.Graphics.RasterizerState )  = 
        device.set_RasterizerState( value )
      member t.get_BlendFactor = 
        device.get_BlendFactor()
      member t.set_BlendFactor ( value : Microsoft.Xna.Framework.Color )  = 
        device.set_BlendFactor( value )
      member t.get_BlendState = 
        device.get_BlendState()
      member t.set_BlendState ( value : Microsoft.Xna.Framework.Graphics.BlendState )  = 
        device.set_BlendState( value )
      member t.get_DepthStencilState = 
        device.get_DepthStencilState()
      member t.set_DepthStencilState ( value : Microsoft.Xna.Framework.Graphics.DepthStencilState )  = 
        device.set_DepthStencilState( value )
      member t.Clear ( color : Microsoft.Xna.Framework.Color )  = 
        device.Clear( color )
      member t.ClearDepthStencil ( options : Graphics.ClearOptions )  ( color : Color )  ( depth : System.Single )  ( stencil : System.Int32 )  = 
        device.Clear( options ,  color ,  depth ,  stencil )
      member t.ClearV4DepthStencil ( options : Graphics.ClearOptions )  ( color : Vector4 )  ( depth : System.Single )  ( stencil : System.Int32 )  = 
        device.Clear( options ,  color ,  depth ,  stencil )
      member t.Dispose() = 
        device.Dispose()
      member t.Present() = 
        device.Present()
      member t.Reset() = 
        device.Reset()
      member t.ResetParam ( presentationParameters : Microsoft.Xna.Framework.Graphics.PresentationParameters )  = 
        device.Reset( presentationParameters )
      member t.get_DisplayMode = 
        device.get_DisplayMode()
      member t.get_GraphicsDeviceStatus = 
        device.get_GraphicsDeviceStatus()
      member t.get_PresentationParameters = 
        device.get_PresentationParameters()
      member t.get_Viewport = 
        device.get_Viewport()
      member t.set_Viewport ( value : Microsoft.Xna.Framework.Graphics.Viewport )  = 
        device.set_Viewport( value )
      member t.get_GraphicsProfile = 
        device.get_GraphicsProfile()
      member t.get_ScissorRectangle = 
        device.get_ScissorRectangle()
      member t.set_ScissorRectangle ( value : Microsoft.Xna.Framework.Rectangle )  = 
        device.set_ScissorRectangle( value )
      member t.get_RenderTargetCount = 
        device.get_RenderTargetCount()
      member t.SetRenderTarget ( renderTarget : Graphics.RenderTarget2D )  = 
        device.SetRenderTarget( renderTarget )
      member t.SetRenderTargetCube ( renderTarget : Graphics.RenderTargetCube )  ( cubeMapFace : Graphics.CubeMapFace )  = 
        device.SetRenderTarget( renderTarget ,  cubeMapFace )
      member t.SetRenderTargetsArray ( renderTargets : Microsoft.Xna.Framework.Graphics.RenderTargetBinding[] )  = 
        device.SetRenderTargets( renderTargets )
      member t.GetRenderTargets = 
        device.GetRenderTargets()
      member t.GetRenderTargetsToArray ( outTargets : Microsoft.Xna.Framework.Graphics.RenderTargetBinding[] )  = 
        device.GetRenderTargets( outTargets )
      member t.SetVertexBuffer ( vertexBuffer : Microsoft.Xna.Framework.Graphics.VertexBuffer )  = 
        device.SetVertexBuffer( vertexBuffer )
  //#line hidden
  //#line default
let createDevice device =
  let isDebug = true
  if isDebug then
    let builder = new ContainerBuilder()
    builder.RegisterType<MGDevice>().As<IMGDevice>().EnableClassInterceptors()
    let reg = new Func<IComponentContext,CallLogger>(fun c -> new CallLogger() )
    builder.Register( reg )
    let container = builder.Build()
    let will = container.Resolve<IMGDevice>(TypedParameter.From(device) )
    will
  else
    new MGDevice(device) :> IMGDevice

