module Cycle

open FSharp.Control.Reactive

open Silk.NET.Input
open Silk.NET.Input.Common
open Silk.NET.Windowing
open Silk.NET.Windowing.Common

let run mainFunc makeDriversFunc =
  let window = Window.Create(WindowOptions.Default)

  let onLoad () =
    let drivers = makeDriversFunc window (window.GetInput())
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

  window.add_Load (fun () -> onLoad ())
  window.Run()
