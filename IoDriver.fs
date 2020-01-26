module IoDriver
open FSharp.Control.Reactive

open System
open Silk.NET.Input
open Silk.NET.Input.Common
open Silk.NET.Windowing.Common


let clickTimeout () = DateTimeOffset.Now.AddMilliseconds 600.0
let isButton button = fun (_, (b: MouseButton)) -> b = button


let clickStream (mouse: IMouse) =
  let downs = Observable.fromAction2 mouse.add_MouseDown
  let ups = Observable.fromAction2 mouse.add_MouseUp

  downs
  |> Observable.filter (isButton MouseButton.Left)
  |> Observable.map (fun _ ->
      ups
      |> Observable.firstIf (isButton MouseButton.Left)
      |> Observable.takeUntilTime (clickTimeout ())
    )
  |> Observable.concatInner


let make (window: IWindow) (input: IInputContext) =
  let driver clicks =
    let inputs = clickStream input.Mice.[0]

    clicks
    // This is where we turn these into actions for the view
    |> Observable.subscribe (printfn "Click: %A")
    |> ignore // Should disposing of object be handled

    // source of the click events
    Some inputs

  driver

