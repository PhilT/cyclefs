module Observable
// Helpers to handle Action<arg> and Action<arg, arg> event delegates
// in Silk.NET.

open FSharp.Control.Reactive

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



