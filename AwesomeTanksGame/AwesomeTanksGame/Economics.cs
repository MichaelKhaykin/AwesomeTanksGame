using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanksGame
{
    public static class Economics
    {
        public static int Money { get; set; }
        public static List<int> DamageCosts = new List<int>();
        public static List<int> HealthCosts = new List<int>();
        public static List<int> DeFoggerCosts = new List<int>();
        public static List<int> SpeedCosts = new List<int>();

        public static List<int> GetListByName(string name)
        {
            switch(name)
            {
                case "sheild":
                    return HealthCosts;

                case "damage":
                    return DamageCosts;

                case "fog":
                    return DeFoggerCosts;

                case "speed":
                    return SpeedCosts;
            }
            return null;
        }
    }
}
