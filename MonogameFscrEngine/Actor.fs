namespace MonoEng
open Microsoft.Xna.Framework;
open Microsoft.Xna.Framework.Graphics;
open System.Collections.Generic;
open System.Linq;
open MonoGame.Extended
module Entity =

  let debugWrite s = System.Diagnostics.Debug.WriteLine s
  type f32 = float32
  [<AbstractClass>]
  type Component() =
    abstract member Update: time:GameTime -> unit
    abstract member Draw :GameTime-> Matrix-> Matrix -> unit
    abstract member SetWorld: Matrix-> unit

  type Actor(name:string) =
    let mutable world = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f));
    let mutable name = name
    let mutable componentList = []
    member t.Name = name
    member t.SetName n = name <-n
    member t.UpdateWorld w =
      for (i:Component) in componentList do
        i.SetWorld w

    member t.Move (x:f32) (y:f32) (z:f32) =
      world.Translation <- world.Translation + new Vector3(x,y,z)
      t.UpdateWorld world
    member t.ScaleX ratio =
      world.M11 <- world.M11 * ratio
      t.UpdateWorld world

    member t.ScaleY ratio =
      world.M22 <- world.M22 * ratio
      t.UpdateWorld world

    member t.ScaleZ ratio =
      world.M33 <- world.M33 * ratio
      t.UpdateWorld world

    member t.Scale ratio =
      world.M11 <- world.M11 * ratio
      world.M22 <- world.M22 * ratio
      world.M33 <- world.M33 * ratio
      t.UpdateWorld world

    member t.World = world
    member t.SetWorld v= world <- v
    member t.Trans = world.Translation

    member t.AddComponent c = componentList <- c::componentList
    member t.Update(time:GameTime) =
      for (i:Component) in componentList do
        i.Update(time)
    member t.Draw time (view) (proj) =
      for (i:Component) in componentList do
        i.Draw time view proj

  type RigidBodyComponent(actor:Actor , world:MMDEng.Bullet.World , mass) =
    inherit Component()
    let mutable actor = actor
    let setUnitscale (m:Matrix) =
      let mutable mc = m
      mc.M11 <- 1.0f
      mc.M22 <- 1.0f
      mc.M33 <- 1.0f
      mc
    let mutable unitScaleWorld = actor.World |> setUnitscale
    let body = world.AddBody (MGUtil.mtxToBullet unitScaleWorld) mass
    let scaleBody scale =
      // bullet など多くの物理エンジンでは剛体のスケーリングを実装しない そのため衝突形状をスケーリングさせる
      body.CollisionShape.LocalScaling <- MGUtil.toBulletV3 scale
      world.UpdateAABB body
    let mutable scale = MGUtil.extractScale actor.World
    do
      scaleBody scale
  
    member t.Damping lin ang = body.SetDamping(lin , ang)
    member t.SetMovable move = 
      if(move) then 
        body.CollisionFlags <- ~~~BulletSharp.CollisionFlags.KinematicObject &&& body.CollisionFlags
      else
        body.CollisionFlags <- BulletSharp.CollisionFlags.KinematicObject ||| body.CollisionFlags
    override t.Update (time:GameTime) =
      let mutable mtx = body.MotionState.WorldTransform
      mtx.M11 <- scale.X
      mtx.M22 <- scale.Y
      mtx.M33 <- scale.Z
      actor.SetWorld <| MGBullet.mtxToMono mtx

    override t.Draw (time:GameTime) (view:Matrix) (proj:Matrix) = ()
    override t.SetWorld worldMtx =
      scale <- MGUtil.extractScale worldMtx
      scaleBody scale
      unitScaleWorld <- actor.World |> setUnitscale
      let newTra = ref <|MGUtil.mtxToBullet unitScaleWorld
      body.MotionState.SetWorldTransform newTra

  type MeshComponent( actor:Actor , model:Model ) =
    inherit Component()
    override t.Update (time:GameTime) = ()
    override t.Draw (time:GameTime) (view:Matrix) (proj:Matrix)=
      for mesh in model.Meshes do
        for effect in mesh.Effects do
          let effect = effect :?> BasicEffect
          MXFUtil.useDefaultDepth effect.GraphicsDevice
          effect.World <- actor.World
          effect.View <- view;
          effect.Projection <- proj;
          effect.DiffuseColor <- new Vector3( 1.0f , 1.0f , 0.0f );
          effect.LightingEnabled <- true
          effect.EnableDefaultLighting()
          //effect.DiffuseColor <- new Vector3(0.5f,0.5f,0.5f)
          //effect.DirectionalLight0.Direction <- new Vector3(0.1f,0.5f,0.8f)
          //effect.DirectionalLight0.DiffuseColor <- new Vector3(0.1f,0.5f,0.8f)
          //effect.DirectionalLight0.Enabled <- true
          //effect.SpecularColor <- new Vector3(0.0f,1.0f,0.0f)

        mesh.Draw()
    override t.SetWorld world = ()
 
