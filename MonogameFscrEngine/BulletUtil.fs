namespace MMDEng
open System
open BulletSharp
open MonoEng
open BulletSharp.Math

module Bullet =
  let debugWrite s = System.Diagnostics.Debug.WriteLine s
  let attempt s f =
    match s with
    |Some x->f x
    |None -> ()
  type MGDynamicWorld(dispatcher , overlappingPairCache , solver , collisionConfiguration) =
    inherit DiscreteDynamicsWorld(dispatcher , overlappingPairCache , solver , collisionConfiguration)
    member this.RemoveAll() =
      //this.Broadphase.Dispose()
      // メモリ壊れるらしい
      //let bp = this.Broadphase 
      //this.CollisionObjectArray.Clear()
      ()
  type ShapeID =
    |Box
    |Convex of Vector3 []
  type World(gravity,debugRenderer:MGDebugRenderer.DebugRender option) =
    let collisionConfiguration = new DefaultCollisionConfiguration();
    let dispatcher = new CollisionDispatcher( collisionConfiguration );
    let overlappingPairCache = new DbvtBroadphase();
    let solver = new SequentialImpulseConstraintSolver();
    let world = new MGDynamicWorld(dispatcher , overlappingPairCache , solver , collisionConfiguration)
    let drawer = new MMFlexUtil.BulletUtil.SharpDXBulletDrawer()
    do
      world.SetGravity <| ref gravity
      drawer.DebugMode <- DebugDrawModes.All
      world.DebugDrawer <- drawer

    member t.AddBodyWithShape (mtx:Math.Matrix) mass (shape:CollisionShape) =
      let localIne = Math.Vector3.Zero
      if mass > 0.0f then
        shape.CalculateLocalInertia(mass,ref localIne)
      let body = new RigidBody(new RigidBodyConstructionInfo(mass, new DefaultMotionState(mtx), shape, localIne));
      body.MotionState.WorldTransform <- mtx
      world.AddRigidBody( body )
      body
    member t.AddBoxBody (mtx:Math.Matrix) mass =
      let sca = mtx.ScaleVector
      let box = new BoxShape(sca.X,sca.Y,sca.Z)
      t.AddBodyWithShape mtx mass box

    member t.AddConvexHullBody  (mtx:Math.Matrix) mass (vs:Vector3 array) =
      let convex = new ConvexHullShape(vs)
      t.AddBodyWithShape mtx mass convex

    member t.AddBody  (mtx:Math.Matrix) mass shapeID =
      match shapeID with
      |Box -> t.AddBoxBody mtx mass
      |Convex vs -> t.AddConvexHullBody mtx mass vs

    member t.ClearWorld() =
      let objCount = world.NumCollisionObjects
      //for i in [0.. objCount - 1 ] do
      //  world.RemoveCollisionObject world.CollisionObjectArray.[i]
      let colLis = [for i in world.CollisionObjectArray ->i]
      for i in colLis do
        world.RemoveCollisionObject i
    member t.UpdateAABB (body:RigidBody) =
      world.UpdateSingleAabb(body)
    member t.DrawerLines view proj =
      let lineList = drawer.LineList
      let mutable lineArray = Array.zeroCreate (lineList.Length * 2)
      for i in [0 .. lineList.Length - 1] do
        let frp = lineList.[i].From
        let top = lineList.[i].To
        let j = i*2
        lineArray.[j] <- MGUtil.mulV (MGBullet.v3ToMonoV4 frp) (view * proj)
        lineArray.[j+1] <- MGUtil.mulV (MGBullet.v3ToMonoV4 top) (view * proj)
      lineArray
    member t.DrawerBoxes view proj =
      let boxList = drawer.BoxList
      let vNum = 24
      let mutable boxArray = Array.zeroCreate (boxList.Length * vNum)
      for i in [0 .. boxList.Length - 1] do
        let cBox = boxList.[i]
        let frp = cBox.Min
        let top = cBox.Max
        let world = MGBullet.mtxToMono cBox.World
        let j = i * vNum
        boxArray.[j + 0] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 1] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 2] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 3] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 4] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 5] <- MGUtil.mulV (MGUtil.vec4 top.X frp.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 6] <- MGUtil.mulV (MGUtil.vec4 top.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 7] <- MGUtil.mulV (MGUtil.vec4 top.X top.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 8] <- MGUtil.mulV (MGUtil.vec4 top.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 9] <- MGUtil.mulV (MGUtil.vec4 top.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 10] <- MGUtil.mulV (MGUtil.vec4 top.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 11] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 12] <- MGUtil.mulV (MGUtil.vec4 top.X frp.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 13] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 14] <- MGUtil.mulV (MGUtil.vec4 top.X frp.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 15] <- MGUtil.mulV (MGUtil.vec4 top.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 16] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 17] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y frp.Z 1.0f) (world * view * proj)
        boxArray.[j + 18] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 19] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 20] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 20] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 21] <- MGUtil.mulV (MGUtil.vec4 frp.X top.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 22] <- MGUtil.mulV (MGUtil.vec4 frp.X frp.Y top.Z 1.0f) (world * view * proj)
        boxArray.[j + 23] <- MGUtil.mulV (MGUtil.vec4 top.X frp.Y top.Z 1.0f) (world * view * proj)

        //let j = i * 2
        //boxArray.[j] <- MGUtil.mulV (MGBullet.v3ToMonoV4 frp) (world * view * proj)
        //boxArray.[j+1] <- MGUtil.mulV (MGBullet.v3ToMonoV4 top) (world * view * proj)
      boxArray
      
    member t.Step(step) view proj =
      drawer.Present()
      let sim = world.StepSimulation(step)
      world.DebugDrawWorld()
      let lines = t.DrawerBoxes view proj
      if(lines.Length > 0) then
        attempt debugRenderer (fun d->d.AddLine(lines))
      for i in [0..dispatcher.NumManifolds - 1] do
        let mani = dispatcher.GetManifoldByIndexInternal i
        let body0 = mani.Body0
        let body1 = mani.Body1
        let contactCount = mani.NumContacts
        for contactID in [0 .. contactCount - 1] do
          let point = mani.GetContactPoint(contactID)
          if(point.Distance < 0.0f) then
            let normal = point.NormalWorldOnB
            let impulse = point.AppliedImpulse
            // 接触した
            ()
    member t.Draw(step) =
      attempt debugRenderer (fun d->d.Draw())
    member t.Raycast from top =
      let res = new AllHitsRayResultCallback( from, top);
      world.RayTest(from , top , res )
      res

    interface IDisposable with
      member t.Dispose() =
        solver.Dispose()
        overlappingPairCache.Dispose()
        dispatcher.Dispose()
        collisionConfiguration.Dispose()
        world.Dispose()
