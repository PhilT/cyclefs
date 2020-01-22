## CycleFS

I like the elegant way that [CycleJS](cyclejs.org) uses Reactive Programming to
build web interfaces and I wanted to see if it would be a good fit for a game
I'm working on in F#.

Fortunately, Andre Staltz, creator of CycleJS did a great set of
[videos on egghead.io](https://egghead.io/courses/cycle-js-fundamentals) where
he builds a simple version of CycleJS to explain how it works.

So I built this toy version of CycleJS in F# using Silk.NET OpenGL library to
handle mouse input.

### Windows usage

    ./run

### Mac/Linux usage

    ./run.sh

Tested on Windows 10 .NET Core 3.0.

This version starts counting up in 1 second intervals in the shell. If you click
with the left mouse button you'll also see a click count.

As Silk.NET is all based on events these can be turned into streams (with a bit
of boilerplate) and seem to work quite nicely.

### Thoughts so far

OpenGL is obviously not component based like the DOM and I think drivers will
mostly be either input or output rather than the dual role the DOM,
and therefore CycleJS has. Still, it's a neat way to get your head into streams.

The only difference in CycleFS compared with the toy CycleJS code is that instead
of a drivers object that gets passed into `Cycle.run` we have a function so that
we can pass in the Silk.NET window and input objects in order to create some of
the drivers but manage the creation of these in the library

However, in Silk.NET, inputs and outputs are discrete, rendering is not triggered
by inputs but by Silk.NET frame render event. Still, inputs do update the state
as well as physics simulation so once these updates are propagated back into
the driver when the render event is triggered it will render with the updated
state. I already have this part working in a separate project so will look to
re-use that here.

