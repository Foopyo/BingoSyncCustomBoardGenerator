# BingoSyncCustomBoardGenerator
Generate boards for custom games for [Bingosync](https://bingosync.com/)
There isn't any weight algorithm so this generator is better suited for lockout bingos.

# How to use
Simply download [the last release](https://github.com/Foopyo/BingoSyncCustomBoardGenerator/releases), unzip it and launch `setup.exe`. Then, go to hte installation folder and launch `BingoBoardGenerator.exe`.

# Editing the goals
If you are using this tool for another game than Ori and the Will of the Wisps you will have to edit the goals, which are set in the file `goals.json`. You need at least 25 different goals in your `goals.json` file in order for this tool to work.
There is three type of goals you can create:
## Fix goal
This is the simplest goal which the Bingo Generator won't be able to change. In order to set it, simply add `{ "name":"Goal description" }` in your JSON array.
## Random number goal
In this one, the Bingo Generator will randomly chose a number in a predefined interval. The caracter $ will be replace by a number in the interval [min;max]
```
{
  "name":"Get $ health",
  "min":5,
  "max":100
}

// Get 61 health
```
## Random objective goal
A random objective will chose one objective among a specific list. The caracter # will be replaced by one of the entries in the list array.
```
{
  "name":"Complete the # level",
  "list":
  [
    "plain",
    "water",
    "star"
  ]
}

// Complete the water level
```
