# Ori WotW Vanilla+ Generator
This project generates Vanilla+ seed for the [Ori and the Will of the Wisps randomizer](https://github.com/sparkle-preference/OriWotwRandomizerClient). A Vanilla+ seed is the vanilla game but you spawn with three items : one weapon, one movement option and one utility item (TP, skills...).
Getting your weapon at its normal loation will give you the Glades TP. Getting your movement option at its normal position will give you West Woods TP.

# How to use
Simply download [the last release](https://github.com/Foopyo/OriWOTWVanillaPlusGenerator/releases), unzip it and launch `setup.exe`. Then, go to the installation folder and launch `VanillaPGenerator.exe`.

# Coop and Bingo autotracking
When generating a Vanilla+ seed, you have the option of enabling the multiplayer feature. Once you activated that feature, launch your seed and, in the launcher, chose to create a new game.

![new game position](https://i.imgur.com/1iyqKiO.png)

# Bingosync boards
This tool can also generates bingo boards for [bingosync](https://bingosync.com/).
If you are using this tool for another game than Ori and the Will of the Wisps you will have to edit the goals, which are set in the file `goals.json`. You need at least 25 different goals in your `goals.json` file in order for this tool to work.
There is three type of goals you can create:
## Fixed goal
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
