open FSharp.Control.Reactive

open Silk.NET.Windowing
open Silk.NET.Windowing.Common
open Silk.NET.Input
open Silk.NET.Input.Common


let main sources =
  let inputs =
    match Map.find "mouse" sources with
    | Some source -> source |> Observable.map (fun _ -> 0)
    | None -> failwith "mouse source connected to incorrect driver"

  Map.empty
  |> Map.add "mouse" (
    inputs |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )
  |> Map.add "log" (
    Observable.interval (System.TimeSpan.FromSeconds(1.0))
    |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )


let drivers input =
  Map.empty
  |> Map.add "mouse" (MouseDriver.make input)
  |> Map.add "log" LogDriver.make


// TODO: This should be hidden in Cycle.run
let window = Window.Create(WindowOptions.Default)
window.add_Load (fun () -> window.GetInput() |> drivers |> Cycle.run main)
window.Run()

