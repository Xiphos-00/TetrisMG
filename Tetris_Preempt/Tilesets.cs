using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris_Preempt
{
    class Tilesets
    {
        public Dictionary<string, int[,]> TileSets = new Dictionary<string, int[,]>();
        public static string[] TileNames = new string[]
        {
            "I",
            "O",
            "T",
            "J",
            "L",
            "S",
            "Z"
        };
        public static Color[] TileColors = new Color[]
        {
            Color.Turquoise,
            Color.Orange,
            Color.Purple,
            Color.LightGreen,
            Color.Red,
            Color.Blue,
            Color.DarkOrange
        };
        public static int[,] I = new int[,]
        {
            {1,1,1,1 },
            {0,0,0,0 },
            {0,0,0,0 },
            {0,0,0,0 }
        };
        public static int[,] O = new int[,]
        {
            {0,0,1,1 }, //x = 0
            {0,0,1,1 }, //x = 1
            {0,0,0,0 }, //x = 2
            {0,0,0,0 } //x = 3
        };
        public static int[,] T = new int[,]
        {
            {0,0,0,1 },
            {0,0,1,1 },
            {0,0,0,1 },
            {0,0,0,0 }
        };
        public static int[,] J = new int[,]
        {
            {0,0,0,0 },
            {0,1,0,0 },
            {0,1,0,0 },
            {1,1,0,0 }
        };
        public static int[,] L = new int[,]
        {
            {0,0,0,0 },
            {1,0,0,0 },
            {1,0,0,0 },
            {1,1,0,0 }

        };
        public static int[,] S = new int[,]
        {
            {0,0,0,0 },
            {0,0,0,0 },
            {0,1,1,0 },
            {1,1,0,0 }
        };
        public static int[,] Z = new int[,]
        {
            {0,0,0,0 },
            {0,0,0,0 },
            {1,1,0,0 },
            {0,1,1,0 }
        };
        public Tilesets()
        {
            TileSets.Add("I", I);
            TileSets.Add("O", O);
            TileSets.Add("T", T);
            TileSets.Add("J", J);
            TileSets.Add("L", L);
            TileSets.Add("S", S);
            TileSets.Add("Z", Z);
        }

}
}
