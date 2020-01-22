module ConsoleDriver

open FSharp.Control.Reactive

let make messages =
  messages
  |> Observable.subscribe (fun message -> printfn "%A" message)
  |> ignore

  None

