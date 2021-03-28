open System
open System.Reflection
open Microsoft.Xna.Framework.Graphics

open MonoGameEng
[<EntryPoint;STAThread>]
let main argv =
    use game = new GameEng()
    game.Run();
    0 // return an integer exit code


