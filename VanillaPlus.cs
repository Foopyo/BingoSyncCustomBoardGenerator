using System;
using System.Collections.Generic;
using System.Text;

namespace VanillaPlusGenerator
{
    public class VanillaPlus
    {
        public string Weapon;
        public string Movement;
        public string Utility;

        public VanillaPlus(VanillaPlusOptions options, Random r)
        {
            Weapon = options.Weapons[r.Next(options.Weapons.Count)];
            Movement = options.Movement[r.Next(options.Movement.Count)];
            Utility = options.Utility[r.Next(options.Utility.Count)];
        }
    }

    public class VanillaPlusOptions
    {
        public List<string> Weapons;
        public List<string> Movement;
        public List<string> Utility;
    }
}
