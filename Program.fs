open FSharp.Control.Reactive

let main sources =
  let inputs =
    match Map.find "mouse" sources with
    | Some source -> source |> Observable.map (fun _ -> 0)
    | None -> failwith "mouse source connected to incorrect driver"

  Map.empty
  |> Map.add "mouse" (
    inputs |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )
  |> Map.add "console" (
    Observable.interval (System.TimeSpan.FromSeconds(1.0))
    |> Observable.scanInit 0 (fun prev _ -> prev + 1)
  )

// This essentially allows you to turn on the drivers you want
// as well as potentially adding your own
let makeDrivers window input =
  Map.empty
  //|> Map.add "display" DisplayDriver.make
  |> Map.add "mouse" (MouseDriver.make input)
  //|> Map.add "keyboard" (KeyboardDriver.make input)
  |> Map.add "console" ConsoleDriver.make
  //|> Map.add "logger" LogDriver.make


Cycle.run main makeDrivers



// Inputs trigger intent updates
// Updates trigger simulation progression (collision detection, momentum)
// Renders happen in the engine

// Should we tie inputs and updates together?


