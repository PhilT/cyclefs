open FSharp.Control.Reactive

open Silk.NET.Windowing
open Silk.NET.Windowing.Common
open Silk.NET.Input
open Silk.NET.Input.Common


let main sources =
  let inputs =
    match Map.find "mouse" sources with
    | Some windowSource -> windowSource |> Observable.map (fun _ -> 0)
    | None -> failwith "windowSource connected to incorrect driver"

  Map.empty
  |> Map.add "mouse" (
    inputs
    |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )
  |> Map.add "log" (
    Observable.interval (System.TimeSpan.FromSeconds(1.0))
    |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )


let logDriver messages =
  messages
  |> Observable.subscribe (fun message -> printfn "%A" message)
  |> ignore

  None


let drivers input =
  Map.empty
  |> Map.add "mouse" (MouseDriver.make input)
  |> Map.add "log" logDriver


let window = Window.Create(WindowOptions.Default)
window.add_Load (fun () -> window.GetInput() |> drivers |> Cycle.run main)
window.Run()

