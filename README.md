## CycleFS

I like the elegant way that [CycleJS](cyclejs.org) uses Reactive Programming to build web
interfaces and I wanted to see if it would be a good fit for a game I'm working
on.

Fortunately, Andre Staltz, creator of CycleJS did a great set of 
[videos on egghead.io](https://egghead.io/courses/cycle-js-fundamentals) where 
he builds a simple version of CycleJS to explain how it works.

So I built this toy version of CycleJS in F# using Silk.NET OpenGL library to
handle mouse input.

### usage

    dotnet run

Tested on .NET Core 3.0.

This version starts counting up in 1 second intervals in the shell. If you click
with the left mouse button you'll also see a click count. 

As Silk.NET is all based on events these can be turned into streams (with a bit
of boilerplate) and seem to work quite nicely.

