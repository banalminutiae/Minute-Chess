# Description
Minute Chess is a small chess implementation where the core engine code is written to be easily extensible with move generation algorithms. There are no plans to add features not directly relevant to the theory of chess such as time management or move analysis.

# Usage
Rather than writing a bespoke graphics layer, Minute Chess implements the [UCI](https://en.wikipedia.org/wiki/Universal_Chess_Interface#:~:text=The%20Universal%20Chess%20Interface%20(UCI,to%20communicate%20with%20user%20interfaces.) protocol to communicate with premade GUIs such as [CuteChess](https://cutechess.com/) or [Arena](http://www.playwitharena.de/)
