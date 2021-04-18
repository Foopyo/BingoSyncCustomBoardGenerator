using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BingoBoardGenerator
{
    public class Goal
    {
        public string name;
        public int min;
        public int max;
        public List<string> list;
    }

    public class GoalBoard
    {
        public string name;

        public GoalBoard(Goal g)
        {
            Random r = new Random();
            if (g.name.Contains("$"))
            {
                name = g.name.Replace("$", r.Next(g.min, g.max + 1).ToString());
            }
            else if (g.name.Contains("#"))
            {
                name = g.name.Replace("#", g.list[r.Next(g.list.Count)]);
            }
            else
            {
                name = g.name;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Reading goals.json");
            string goalsString = "";
            try
            {
                goalsString = File.ReadAllText("goals.json");
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error when reading goals.json: " + e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                return;
            }
            List<Goal> goals = null;
            try
            {
                goals = JsonConvert.DeserializeObject<List<Goal>>(goalsString);
                if(goals.Count < 25)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("goals.json needs at least 25 different goals. There is currently only " + goals.Count + " specified in this file.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Press a key to exit");
                    Console.ReadKey();
                    return;
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while deserializing goals.json: " + e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                return;
            }
            List<GoalBoard> board = new List<GoalBoard>();

            try
            {
                for (int i2 = 0; i2 < 25; i2++)
                {
                    Console.WriteLine("Select goal number " + (i2 + 1));
                    int iGoal = r.Next(0, goals.Count);
                    board.Add(new GoalBoard(goals[iGoal]));
                    goals.RemoveAt(iGoal);
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while selecting goals for the board: " + e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                return;
            }

            string outputFile;

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
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while creating the bingo board file: " + e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bingo board " + outputFile + " successfully created!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
            return;
        }
    }
}
