namespace MonoGameEng
open Microsoft.Xna.Framework
open System.Diagnostics
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open MonoEng
open MMDEng.Bullet
open MonoEng.Entity

  type Camera () =
   let mutable view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10.0f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.UnitY);
   let mutable proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 800f / 480f, 0.1f, 100f);
   member t.View = view
   member t.Proj = proj
   member t.Forward = view.Forward

   member t.Move z=
     view <- view * Matrix.CreateTranslation z
   member t.Rotate y =
     view <- view * Matrix.CreateRotationY y

//module GameMain =
  type GameEng() as t=
    inherit Game()
    let graphicMan = new GraphicsDeviceManager(t)
    let mutable ship = None
    let mutable shipModel = null
    let mutable actorList = []
    let mutable debugRenderer = None
    let unWrapDebugRenderer() = debugRenderer.Value
    let mutable world = None
    let mutable initedTime = 0
    let unWrapWorld() = world.Value
    let camera = new Camera()
    let mutable myra = None
    let mutable inited = false
    do
      t.Content.RootDirectory <- "Content"
      t.Window.AllowUserResizing <- true
      t.IsMouseVisible <- true
    override t.LoadContent() =
      shipModel <- t.Content.Load<Model>( "Ship" )
      let debugRen = new MGDebugRenderer.DebugRender(t.GraphicsDevice)
      debugRenderer <- Some <| debugRen
      world <- Some <|new World( MGBullet.vec3 0.0f -9.8f  0.0f , Some debugRen )
      let m = new MyraFsModule.MyraFacade(t.GraphicsDevice)
      myra <- Some <| m

    member t.Init(time:GameTime) =

      actorList <- []
      let world = unWrapWorld()
      world.ClearWorld()
      let actor = new Actor("ship")
      let mesh = new MeshComponent (actor , shipModel)
      let body = new RigidBodyComponent(actor , world , 1.0f)
      let ground = new Actor("ground")
      ground.Move 0.0f -7.0f 0.0f
      ground.ScaleX 5.0f
      ground.ScaleY 2.0f
      ground.ScaleZ 5.0f

      let gmesh = new MeshComponent (ground, shipModel)
      let gbody = new RigidBodyComponent(ground, world , 0.0f)
      //gbody.SetMovable false
      ground.AddComponent gmesh
      ground.AddComponent gbody
      actor.AddComponent mesh
      actor.AddComponent body

      ship <- Some <|actor
      actorList <- [
        actor
        ground
        ]
      inited <- true
      initedTime <- time.TotalGameTime.Seconds
      ()

    override t.Update(time) =
      let elapsedFromInit = time.TotalGameTime.Seconds - initedTime
      let input = Input.Keyboard.GetState()
      let inL = input.IsKeyDown(Keys.Left)
      let inRight = input.IsKeyDown(Keys.Right)
      let inUp = input.IsKeyDown(Keys.Up)
      let inDow = input.IsKeyDown(Keys.Down)
      let inU = input.IsKeyDown(Keys.U)
      let inJ = input.IsKeyDown(Keys.J)
      let inA = input.IsKeyDown(Keys.A)
      let inR = input.IsKeyDown(Keys.R)
      if inR && elapsedFromInit > 1 then
        inited <- false

      let delta = time.ElapsedGameTime.Milliseconds |> float32 |> (*) 0.001f
      if inUp then
        camera.Move <| Vector3.Forward * delta * -10f
      if inDow then
        camera.Move <| Vector3.Forward * delta * 10f
      if inL then
        camera.Rotate <| delta * -1f
      if inRight then
        camera.Rotate <| delta * 1f
      if inJ then
        camera.Move <| Vector3.Up * delta * 10f
      if inU then
        camera.Move <| Vector3.Down * delta * 10f
      if(not inited) then
        t.Init(time)
      let ship = ship.Value
      for i in actorList do
        i.Update time
      if(inA) then
        ship.Move -0.1f 0.0f 0.0f
      //for i in [0..3] do
      //  let key = Input.GamePad.GetState(i)
      //  let key1 = Input.Joystick.GetState(i)
      //  Debug.WriteLine <| string(key.IsConnected) + string i
      let stepTime = ((float32 time.ElapsedGameTime.Milliseconds ) / 1000.0f) 
      unWrapWorld().Step stepTime camera.View camera.Proj
      base.Update(time)
    override t.Draw(time) =
      let dev = t.GraphicsDevice.Handle :?> SharpDX.Direct3D11.Device
      t.GraphicsDevice.Clear(Color.GreenYellow)
      attempt myra (fun m->m.Render())
      for i in actorList do
        i.Draw time camera.View camera.Proj
      unWrapWorld().Draw(time)
      base.Draw(time)

