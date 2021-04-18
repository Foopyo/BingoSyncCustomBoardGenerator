using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BingoBoardGenerator
{
    class Program
    {
        static void WriteConsoleColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
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
            List<Goal> goals = null;
            try
            {
                goals = JsonConvert.DeserializeObject<List<Goal>>(goalsString);
                if (goals.Count < 25)
                {
                    WriteConsoleColor("goals.json needs at least 25 different goals. There is currently only " + goals.Count + " specified in this file.", ConsoleColor.Red);
                    return null;
                }
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error while deserializing goals.json: " + e.ToString(), ConsoleColor.Red);
                return null;
            }
            return goals;
        }

        static List<GoalBoard> GenerateBoard(List<Goal> goals, Random r)
        {
            try
            {
                List<GoalBoard> board = new List<GoalBoard>();
                for (int i2 = 0; i2 < 25; i2++)
                {
                    Console.WriteLine("Select goal number " + (i2 + 1));
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

        static void WriteBoard(List<GoalBoard> board)
        {
            string outputFile = "";
            try
            {
                int i = 0;
                outputFile = "Bingo_"; ;
                while (File.Exists(outputFile + i.ToString() + ".json"))
                {
                    i++;
                }
                outputFile += i.ToString() + ".json";
                File.WriteAllText(outputFile, JsonConvert.SerializeObject(board));
                WriteConsoleColor("Bingo board " + outputFile + " successfully created!", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                WriteConsoleColor("Error while creating the bingo board file: " + e.ToString(), ConsoleColor.Red);
            }
        }

        static void Main(string[] args)
        {
            Random r = new Random();
            List<Goal> goals = ReadGoals();
            if (goals != null)
            {
                List<GoalBoard> board = GenerateBoard(goals, r);
                if(board != null)
                {
                    WriteBoard(board);
                }
            }
        }
    }
}
