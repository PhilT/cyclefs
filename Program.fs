open FSharp.Control.Reactive
open System

open Silk.NET.Windowing
open Silk.NET.Windowing.Common
open Silk.NET.Input
open Silk.NET.Input.Common

// Helpers to handle Action<arg> and Action<arg, arg> event delegates
// in Silk.NET.

// Action with 1 parameter
let fromAction<'a> addHandler =
  let subject = Subject<'a>.broadcast
  let next a = Subject.onNext a subject |> ignore
  addHandler next
  subject


// Action with 2 parameters
let fromAction2<'a, 'b> addHandler =
  let subject = Subject<('a * 'b)>.broadcast
  let next a b = Subject.onNext (a, b) subject |> ignore
  addHandler next
  subject


// Input handling
let clickTimeout () = DateTimeOffset.Now.AddMilliseconds 600.0
let isButton button = fun (_, (b: MouseButton)) -> b = button

let clicks (mouse: IMouse) =
  let downs = fromAction2 mouse.add_MouseDown
  let ups = fromAction2 mouse.add_MouseUp

  downs
  |> Observable.filter (isButton MouseButton.Left)
  |> Observable.map (fun _ ->
      ups
      |> Observable.firstIf (isButton MouseButton.Left)
      |> Observable.takeUntilTime (clickTimeout ())
    )
  |> Observable.concatInner


let main sources =
  let inputs =
    match Map.find "window" sources with
    | Some windowSource -> windowSource |> Observable.map (fun _ -> 0)
    | None -> failwith "windowSource connected to incorrect driver"

  Map.empty
  |> Map.add "window" (
    inputs
    |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )
  |> Map.add "log" (
    Observable.interval (TimeSpan.FromSeconds(1.0))
    |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )


let logDriver messages =
  messages
  |> Observable.subscribe (fun message -> printfn "%A" message)
  |> ignore

  None

let makeWindowDriver _window (input: IInputContext) =
  let inputs = clicks input.Mice.[0]

  let windowDriver clicks =
    clicks
    |> Observable.subscribe (fun clickCount -> printfn "Clicks: %A" clickCount)
    |> ignore

    // source of the click events
    Some inputs

  windowDriver


let run mainFunc (drivers: Map<string, (IObservable<'T> -> 'b option)>) =
  let fakeSinks = drivers |> Map.map (fun _ _ -> Subject.broadcast)

  let sources =
    drivers
    |> Map.map (fun key driver -> driver (Map.find key fakeSinks))

  let sinks  = mainFunc sources

  sinks
  |> Map.map (fun key sink ->
    Observable.subscribeObserver (Map.find key fakeSinks) sink
  )
  |> ignore


let drivers window input =
  Map.empty
  |> Map.add "window" (makeWindowDriver window input)
  |> Map.add "log" logDriver

let onLoad window input =
  run main (drivers window input)

let window = Window.Create(WindowOptions.Default)
window.add_Load (fun () -> onLoad window (window.GetInput()))
window.Run()

