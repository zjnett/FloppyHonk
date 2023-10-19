# Floppy Honk
![](https://img.shields.io/badge/Game_%23-1-chartreuse)
![](https://img.shields.io/badge/Engine-Godot-blue?logo=godotengine)

<p align="center">
  <img src="demo.gif" alt="animated"/>
</p>

A Flappy Bird clone made in Godot. Part of the [20 Games Challenge](https://20_games_challenge.gitlab.io//).

## Description
This was created to be a fairly authentic clone of Flappy Bird made in Godot. The intention of this challenge is to systematically challenge different game development muscles and skills while creating actual finished games in the process.

## Design
I admittedly am working on wrapping my head around the compositional nature of Godot's node-based system. As such, things aren't exactly as organized as I would have hoped for this game, although that's a skill I expect to work on in the future.

Each subsystem in the game has a dedicated script class, including:
- Bird (Player)
- HUD
- PipePair
- Main

Where the main class describes the general game flow and system behavior. There are also a few fragment shaders used for various effects, including a rainbow effect on a new high score and a shader to scroll the ground texture left-to-right.

I also chose to use C# as practice for later more advanced projects where I anticipate taking advantage of C# data structures and potential performance benefits over GDscript.

## Tools Used 
- Godot (game engine)
- Aseprite (sprites and animations)
- VSCode (code editor)
- Logic Pro and chipsynth SFC (sound effects)

## Credits
- Rainbow Shader adapted from [Exuin on godotshaders.com](https://godotshaders.com/shader/moving-rainbow-gradient/)
- (Currently unused) CRT shader from [pend00 on godotshaders.com](godotshaders.com/shader/VHS-and-CRT-monitor-effect)

