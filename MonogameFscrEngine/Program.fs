open System
open MonoGameEng
open MMDEng.Bullet
open BulletSharp.Math
open MonoEng
type Tester()=
  let world = new World(new Vector3(0.0f,-10.0f,0.0f),None)
  do
    let body = world.AddBody Matrix.Identity 1.0f
    body.UserObject <- "player"
    let body2 = world.AddBody ( Matrix.Scaling(5.0f , 1.0f , 1.0f) * Matrix.Translation(new Vector3(0.0f,-3.0f , 0.0f) )) 0.0f
    body2.UserObject <- "player2"
    for i in [0..100] do
      world.Step(1.0f / 30.0f) (MGBullet.mtxToMono Matrix.Identity) (MGBullet.mtxToMono Matrix.Identity)
      System.Diagnostics.Debug.WriteLine(body.MotionState.WorldTransform.Origin)
[<EntryPoint;STAThread>]
let main argv =
    //new Tester()
    use game = new GameEng()
    game.Run();
    0 // return an integer exit code


