module MouseDriver
open FSharp.Control.Reactive

open System
open Silk.NET.Input
open Silk.NET.Input.Common


let clickTimeout () = DateTimeOffset.Now.AddMilliseconds 600.0
let isButton button = fun (_, (b: MouseButton)) -> b = button


let clicks (mouse: IMouse) =
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


let make (input: IInputContext) =
  let inputs = clicks input.Mice.[0]

  let windowDriver clicks =
    clicks
    |> Observable.subscribe (fun clickCount -> printfn "Clicks: %A" clickCount)
    |> ignore

    // source of the click events
    Some inputs

  windowDriver



