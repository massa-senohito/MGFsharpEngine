namespace MonoEng
type BVec3 = BulletSharp.Math.Vector3
type BVec4 = BulletSharp.Math.Vector4
type BMtx = BulletSharp.Math.Matrix
module MGUtil =
  open Microsoft.Xna.Framework
  let vec4W1 x y z = new Vector4(x,y,z,1.0f)
  let vec4 x y z w = new Vector4(x,y,z,w)
  let vec3 x y z = new Vector3(x,y,z)
  let toBulletV3 (v:Vector3) = new BVec3 ( v.X , v.Y , v.Z )
  let mulV (v:Vector4) (m:Matrix) = Vector4.Transform(v,m)
  let extractScale (m:Matrix) = vec3 m.M11 m.M22 m.M33
  let mtxToBullet (value:Matrix) =
    //let value = Matrix.Transpose value
    new BulletSharp.Math.Matrix(
        value.M11, value.M12, value.M13, value.M14,
        value.M21, value.M22, value.M23, value.M24,
        value.M31, value.M32, value.M33, value.M34,
        value.M41, value.M42, value.M43, value.M44 )

module MGBullet =
  let v3ToMonoV4 (v:BVec3) = MGUtil.vec4W1 v.X v.Y v.Z
  let vec3 x y z = new BVec3(x,y,z)
  let vec4 x y z w = new BVec4(x,y,z,w)
  let Trans (v:BVec3) (m:BMtx)= BVec3.Transform ( v , m)
  let v4ToMonoV4 (v:BVec4)= MGUtil.vec4 v.X v.Y v.Z v.W
  let mtxToMono (value:BulletSharp.Math.Matrix) =
    new Microsoft.Xna.Framework.Matrix(
        value.M11, value.M12, value.M13, value.M14,
        value.M21, value.M22, value.M23, value.M24,
        value.M31, value.M32, value.M33, value.M34,
        value.M41, value.M42, value.M43, value.M44 )
