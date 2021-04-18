using System;

namespace BingoBoardGenerator
{
    public class GoalBoard
    {
        public string name;

        public GoalBoard(Goal g, Random r)
        {
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
}
