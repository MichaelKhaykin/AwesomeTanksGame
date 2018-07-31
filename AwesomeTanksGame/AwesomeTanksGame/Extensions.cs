using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanksGame
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this float floatToBeVector)
        {
            return new Vector2(floatToBeVector, floatToBeVector);
        }
    }
}
