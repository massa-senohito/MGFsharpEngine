namespace Game
open System.Linq
open System.Collections.Generic
open MonoGameEng
open Microsoft.Xna.Framework

module GolfGame =
  let inline (@+) (a:ref<'a>) b = a := a.Value + b
  [<AbstractClass>]
  type StateMachine<'t,'tid>() =
    abstract member Context: 't
    abstract member Update: GameTime-> unit
    abstract member Enter:unit -> unit
    abstract member Exit: unit -> unit
    abstract member IsTransable: StateMachine<'t,'tid> -> bool
    abstract member TID: 'tid
  type PatType =
    |StartPat
    |PatDirection


  type GolfPlayerState = StateMachine<GolfContext,PatType>
  //let createState t = new GolfPlayerState(t)
  and StartPatState(c:GolfContext) =
    inherit GolfPlayerState()
    let barPower = ref 0.0f

    override this.TID = StartPat
    override this.Context = c
    override this.IsTransable state =
      state.TID = PatDirection
    override this.Enter() =
      barPower := 0.0f
    override this.Update( time:GameTime) =
      barPower @+ float32 time.ElapsedGameTime.Milliseconds * 0.1f
      if !barPower > 100.0f then
        barPower := 0.0f
      c.PowerChange !barPower
      ()
    override this.Exit() =
      barPower := 0.0f
      ()
  and DirectionPatState(c:GolfContext) =
    inherit GolfPlayerState()
    let inputDir = ref 0.0f
    member t.AddInputDir i =
      inputDir @+ i

    override this.TID = PatDirection
    override this.Context = c
    override this.IsTransable state =
      state.TID = StartPat
    override t.Enter() =
      inputDir := 0.0f
      ()
    override t.Update( time:GameTime ) =
      ()
    override t.Exit() =
      inputDir := 0.0f
      ()
  and GolfContext() as this=
    let init = ()
    let powerChanged = new Event<float32>()
    let mutable state = new StartPatState(this) :> GolfPlayerState
    member this.PowerChange v = powerChanged.Trigger v
    member this.SubscribePowerChange f = powerChanged.Publish.Subscribe f
    member this.ChangeState nextState =
      if state.IsTransable nextState then
        state.Exit()
        state <- nextState
        state.Enter()
    member t.Update(time:GameTime) =
      state.Update(time)

  //type StateTable<'e , 'state when 'e : comparison>()=
  //  let mutable transTable = new Dictionary<'e,'state>()
  //  let mutable stateList = new List<'state>()
  //  member this.AddTrans<'cs ,'ns when 'e : equality> e  =
  //    let stateType = typeof< 'cs>
  //    let mayFoundState = stateList.FirstOrDefault(fun e -> e = stateType)
  //    if mayFoundState = null then
  //      stateList.Add stateType

  //    transTable.Add( e ,s)
  //  member this.SendEvent<'e,'ne> i =
  //    let stateType = typeof< 'e>

    //let transTable =
    //  let table = [(StartPat,)]//Map<'e,'state>

  type State<'Event> =
      | Next of ('Event -> State<'Event>)
      | Stop

  let feed state event =
      match state with
      | Stop -> failwith "Terminal state reached"
      | Next handler -> event |> handler

  type FStateMachine<'event>(initial: State<'event>) =
      let mutable current = initial
      member this.Fire event = current <- feed current event
      member this.IsStopped 
          with get () = match current with | Stop -> true | _ -> false

  let inline createMachine (initial) = new FStateMachine<'a>(initial)

module GameMain =

  type GolfVal(device:Graphics.GraphicsDevice) =
    //inherit Component()

    let myra = new MyraFsModule.MyraFacade(device)
    let barUI = MyraFsModule.createProgressBar 100 120 700 20
    let barValueUI = MyraFsModule.createLabel 300 120
    let grid = MyraFsModule.grid()
    let setBarValue v =
      barUI.Value <- v
      barValueUI.Text <- string <|int v
    let observable =
      { new System.IObserver<float32> with
                        member x.OnNext(args) = setBarValue(args)
                        member x.OnError(ex) = ()
                        member x.OnCompleted() = ()}
    let context = new GolfGame.GolfContext()
    let handle = context.SubscribePowerChange observable
    do
      MyraFsModule.addRowPart grid
      MyraFsModule.addColumnPart grid
      MyraFsModule.addWidget grid barUI
      MyraFsModule.addWidget grid barValueUI
      myra.SetRoot(grid)

    member t.Init() =
      ()
    member t.Update(time : GameTime) =
      context.Update(time)
      myra.Update()
      ()
    member t.Render() =
      myra.Render()
      ()

    member t.Quit() =
      let d = myra :> System.IDisposable
      d.Dispose()
      handle.Dispose()
  //type GolfBall() =

