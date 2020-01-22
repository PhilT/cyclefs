module Cycle
open FSharp.Control.Reactive

let run mainFunc drivers =
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

