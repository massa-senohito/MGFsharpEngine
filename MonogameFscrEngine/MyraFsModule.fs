namespace MonoGameEng
open Microsoft.Xna.Framework.Graphics;
open Microsoft.Xna.Framework;
open Myra
open Myra.Platform
open Myra.Graphics2D
open Myra.Graphics2D.UI
open System.Numerics
open Microsoft.Xna.Framework.Input

//open MyraUI.TextureUtil
module MyraFsModule=
  type IDisposable = System.IDisposable
  let toXNAMtx (matrix:Matrix3x2) =
    let mutable result = Matrix.Identity
    result.M11 <- matrix.M11;
    result.M12 <- matrix.M12;
    result.M21 <- matrix.M21;
    result.M22 <- matrix.M22;

    result.M41 <- matrix.M31;
    result.M42 <- matrix.M32;
    result
  let toXNARect (r:System.Drawing.Rectangle) = new Rectangle(r.Left , r.Top , r.Width , r.Height)
  let toSysRect (r:Rectangle) = new System.Drawing.Rectangle(r.Left , r.Top , r.Width , r.Height)
  let toXNAColor(c:System.Drawing.Color) = new Color(c.R , c.G , c.B , c.A)
  let toXNAVec2 (v:Vector2) = new Microsoft.Xna.Framework.Vector2(v.X,v.Y)
  let nullableRect r = new System.Nullable<Rectangle>(r)
  let nullRect = new System.Nullable<Rectangle>()
  let nullMtx = new System.Nullable<Matrix3x2>()
  let inline nullable (v:'a) = new System.Nullable<'a>(v)

  type Rend ( device: GraphicsDevice)=
    let batch = new SpriteBatch(device )
    let mutable began = false
    let mutable transformMat = nullMtx
    let endBatch() =
      batch.End()
      began <- false
    let UIRasterizerState =
      let mutable state = new RasterizerState()
      state.ScissorTestEnable <- true
      state
    let beginBatch(  transform : System.Nullable<System.Numerics.Matrix3x2>)=
      let mtx = if transform.HasValue then new System.Nullable<Matrix>(toXNAMtx transform.Value) else new System.Nullable<Matrix>()
      batch.Begin(SpriteSortMode.Deferred,
         BlendState.AlphaBlend,
         SamplerState.PointClamp,
         null,
         UIRasterizerState,
         null,
         mtx
         );
      began <- true
      //let trans = if transform.HasValue then Some transform.Value else None
      transformMat <- transform
    let flush() =
      if( began ) then
        endBatch()
        beginBatch( transformMat )

    interface IMyraRenderer with
      member t.Begin(  transform : System.Nullable<System.Numerics.Matrix3x2>)=
        beginBatch transform
      override t.Draw(tex,dest,src,color) =
        let xnaTex = tex :?> Texture2D
        let nu = if src.HasValue then (nullableRect <| toXNARect src.Value) else (nullRect)
        batch.Draw(
          xnaTex ,
          dest |> toXNARect,
          nu,
          color|>toXNAColor
        );
      override t.Draw(tex, position, sourceRectangle, color, rotation, origin, scale, depth) =
        let xnaTex = tex :?> Texture2D

        let nu = if sourceRectangle.HasValue then (nullableRect <| toXNARect sourceRectangle.Value) else (nullRect)
        batch.Draw(
          xnaTex ,
          position |>toXNAVec2,
          nu,
          color|>toXNAColor,
          rotation,
          origin|>toXNAVec2,
          scale|>toXNAVec2,
          SpriteEffects.None,
          depth);

      override t.End() =
        endBatch()

      override t.Scissor =
        let mutable rect = device.ScissorRectangle
        rect.X <- rect.X - device.Viewport.X;
        rect.Y <- rect.Y - device.Viewport.Y;
        rect |>toSysRect

      override t.set_Scissor v=
        flush()
        let mutable v = v
        v.X <- v.X + device.Viewport.X
        v.Y <- v.Y + device.Viewport.Y
        device.ScissorRectangle <- v|>toXNARect

  type Plat(  device:GraphicsDevice ) =
    let viewSize = new System.Drawing.Point( device.Viewport.Width, device.Viewport.Height )
    let mutable texList = []
    interface IDisposable with
      override this.Dispose() =
        for i in texList do
          let d = i :> IDisposable
          i.Dispose()
    interface IMyraPlatform with
      member t.CreateRenderer() =
        new Rend (device) :> IMyraRenderer
      member t.GetMouseInfo() = 
        let state = Mouse.GetState();
        let result =
          let mutable info = new MouseInfo()
          info.Position <- new System.Drawing.Point(state.X, state.Y)
          info.IsLeftButtonDown <- state.LeftButton = ButtonState.Pressed
          info.IsMiddleButtonDown <- state.MiddleButton = ButtonState.Pressed
          info.IsRightButtonDown <- state.RightButton = ButtonState.Pressed
          info.Wheel <- state.ScrollWheelValue |> float32
          info
        result
      member t.SetKeysDown keys =
        let state = Keyboard.GetState()
        for i in [0 .. keys.Length - 1] do
          keys.[i] <- state.IsKeyDown( enum< Microsoft.Xna.Framework.Input.Keys> i )

      member t.GetTouchState() =
        TouchCollection.Empty
      member t.ViewSize =
        viewSize
        // 大きさ指定
      member t.CreateTexture(width, height) =
        new Texture2D(device , width , height) :> obj

        // そのテクスチャにデータ
      member t.SetTextureData(texture, bounds, data) =
        let xnaTex = texture :?> Texture2D
        xnaTex.SetData (0, bounds |>toXNARect, data, 0, bounds.Width * bounds.Height * 4)
        texList <- xnaTex :: texList
      // todo Clearを入れる
      //member t.Clear

  let createLabel x y =
    let lab = new Label()
    lab.Margin <- new Thickness(x,y)
    lab
  let createButton x y =
    let lab = new TextButton()
    lab.Margin <- new Thickness(x,y)
    lab

  let createProgressBar x y w h =
    let prog = new HorizontalProgressBar()
    prog.Width <-  nullable w
    prog.Height <- nullable h
    prog.Margin <- new Thickness(x,y)
    prog

  let grid() = 
    let g = new Grid()
    //g.RowSpacing <- 10
    //g.ColumnSpacing <- 10
    //g.ColumnsProportions.Add(new Proportion(ProportionType.Auto))
    //g.ColumnsProportions.Add(new Proportion(ProportionType.Auto))
    // partなら画面を占める割合で設定する
    //g.RowsProportions.Add(new Proportion(ProportionType.Part))
    //g.RowsProportions.Add(new Proportion(ProportionType.Auto))
    //g.Widgets.Add <| hello()
    //g.Widgets.Add <| helloBu()
    //g.Widgets.Add <| progress()
    g

  let addWidget (g:Grid) (w:Widget)=
    g.Widgets.Add w
  let addRowPart (g:Grid) =
    g.RowsProportions.Add ( new Proportion (ProportionType.Part))
  let addColumnPart (g:Grid) =
    g.ColumnsProportions.Add ( new Proportion (ProportionType.Part))

  type MyraFacade(device:GraphicsDevice) =
    let desktop = new Desktop()
    let viewSize = new System.Drawing.Point( device.Viewport.Width, device.Viewport.Height )
    
    let plat = new Plat(device)
    do
      MyraEnvironment.Platform <- plat :> IMyraPlatform
    member t.SetRoot r =
      desktop.Root <- r

    member t.Update() =
      ()
    member t.Render() =
      desktop.Render()
      ()
    interface IDisposable with
      member this.Dispose() =
        let d = plat :> IDisposable
        d.Dispose()
