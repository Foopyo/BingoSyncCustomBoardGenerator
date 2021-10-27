using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace VanillaPlusGenerator
{
    class Program
    {
        static void WriteConsoleColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static List<Goal> ReadGoals()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Reading goals.json");
            string goalsString = "";
            try
            {
                goalsString = File.ReadAllText("goals.json");
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error when reading goals.json: " + e.ToString(), ConsoleColor.Red);
                return null;
            }
            try
            {
                List<Goal> goals = JsonConvert.DeserializeObject<List<Goal>>(goalsString);
                if (goals.Count < 25)
                {
                    WriteConsoleColor("goals.json needs at least 25 different goals. There is currently only " + goals.Count + " specified in this file.", ConsoleColor.Red);
                    return null;
                }
                return goals;
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error while deserializing goals.json: " + e.ToString(), ConsoleColor.Red);
                return null;
            }
        }

        static List<GoalBoard> GenerateBoard(List<Goal> goals, Random r)
        {
            try
            {
                Console.WriteLine("Selecting goals");
                List<GoalBoard> board = new List<GoalBoard>();
                for (int i2 = 0; i2 < 25; i2++)
                {
                    int iGoal = r.Next(0, goals.Count);
                    board.Add(new GoalBoard(goals[iGoal], r));
                    goals.RemoveAt(iGoal);
                }
                return board;
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error while selecting goals for the board: " + e.ToString(), ConsoleColor.Red);
                return null;
            }
        }

        static void WriteBoard(List<GoalBoard> board, string fileName)
        {
            try
            {
                int i = 0;

                while (File.Exists(fileName + i.ToString() + ".json"))
                {
                    i++;
                }
                if (i != 0)
                {
                    fileName += "_" + i.ToString() + ".json";
                }
                else
                {
                    fileName += ".json";
                }
                File.WriteAllText(fileName, JsonConvert.SerializeObject(board));
                WriteConsoleColor("Bingo board " + fileName + " successfully created!", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error while creating the bingo board file: " + e.ToString(), ConsoleColor.Red);
            }
        }

        static VanillaPlus ChoseStartingOptions(Random r)
        {
            try
            {
                Console.WriteLine("Selecting your Vanilla+ starting items");
                VanillaPlusOptions options = JsonConvert.DeserializeObject<VanillaPlusOptions>(File.ReadAllText("startupItems.json"));
                return new VanillaPlus(options, r);
            }
            catch(Exception e)
            {
                WriteConsoleColor("Error while choosing vanilla+ starting items: " + e.ToString(), ConsoleColor.Red);
                return null;
            }
        }

        static void WriteSeed(VanillaPlus items, string fileName, bool multiplayer)
        {
            int i = 0;

            while (File.Exists(fileName + i.ToString() + ".wotwr"))
                i++;
            if (i != 0)
                fileName += "_" + i.ToString() + ".wotwr";
            else
                fileName += ".wotwr";

            try
            {
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine("// Vanilla+");
                    sw.WriteLine("3|0|8|1|105|bool|true");  // Opher already sold you TPA
                    sw.WriteLine("3|0|" + items.Weapon);
                    sw.WriteLine("3|0|" + items.Movement);
                    sw.WriteLine("3|0|" + items.Utility+"\n");

                    using (StreamReader sr = new StreamReader("vanillaPlusSeedTemplate.wotwr"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("|" + items.Weapon + " "))
                                sw.WriteLine(line.Replace("|" + items.Weapon + " ", "|5|17"));
                            else if (line.Contains("|" + items.Movement + " "))
                                sw.WriteLine(line.Replace("|" + items.Movement + " ", "|5|7"));
                            else if (line.Contains("3|0|8|42178|16825|int|1") || line.Contains("3|0|8|48248|16489|int|0") || line.Contains("1|105|6|Fast Travel"))  // Skip reverting spirit well in Glades/Marsh + skip losing TPA from the start + Message when getting TPA
                                continue;
                            else if (line.Contains("// Config") && multiplayer)
                                sw.Write(line.Replace("\"webConn\":false", "\"webConn\":true"));
                            else
                                sw.WriteLine(line);
                        }
                    }
                }
                WriteConsoleColor("Seed file " + fileName + " successfully created!", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error while creating the vanilla+ seed file: " + e.ToString(), ConsoleColor.Red);
            }
        }

        static void Main(string[] args)
        {
            string seed;
            bool multiplayer;
            Console.WriteLine("Enter a seed (empty for random seed)");
            seed = Console.ReadLine();
            seed = seed != "" ? seed : DateTime.Now.GetHashCode().ToString();
            Console.WriteLine("Do you want to play in coop or enable bingo autotracking? (y/n)");
            multiplayer = Console.ReadKey().Key == ConsoleKey.Y;
            Console.WriteLine("");
            Console.WriteLine("Do you want to generate a bingosync board? (y/n)");
            bool bingo = Console.ReadKey().Key == ConsoleKey.Y;
            Console.WriteLine("");
            Random r = new Random(seed.GetHashCode());

            VanillaPlus items = ChoseStartingOptions(r);
            WriteSeed(items, seed, multiplayer);
            if (bingo)
            {
                List<Goal> goals = ReadGoals();
                List<GoalBoard> board = GenerateBoard(goals, r);
                if (board != null)
                {
                    WriteBoard(board, seed);
                }
            }

            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }
    }
}
