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
            string goalsString = File.ReadAllText("goals.json");
            List<Goal> goals = JsonConvert.DeserializeObject<List<Goal>>(goalsString);
            List<GoalBoard> board = new List<GoalBoard>();

            for (int i2 = 0; i2 < 25; i2++)
            {
                int iGoal = r.Next(0, goals.Count);
                board.Add(new GoalBoard(goals[iGoal]));
                goals.RemoveAt(iGoal);
            }

            int i = 0;
            string outputFile = "Bingo_";
            while (File.Exists(outputFile + i.ToString() + ".json"))
            {
                i++;
            }
            File.WriteAllText(outputFile + i.ToString() + ".json", JsonConvert.SerializeObject(board));
        }
    }
}
