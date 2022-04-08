# Risk of Rain 2 Arena Mod

Arena is a Risk of Rain 2 mod for fighting your friends at the end of each stage. See the [mod's readme](src/README.md) for more details.

## Development

1. Install [ScriptEngine.](https://github.com/BepInEx/BepInEx.Debug#scriptengine)
2. Change the second argument of the copy command in the post-build event in the [Arena.csproj](src/Arena/Arena.csproj) file to use the correct path of your `BepInEx\scripts` folder. [More info.](https://github.com/risk-of-thunder/R2Wiki/wiki/Build-Events#copy-output-dll=)
