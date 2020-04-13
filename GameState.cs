using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace The_Game
{
    public class GameState
    {
        public float Dt { get; }
        public Player Player { get; }
        public ILevel Level { get; set; }

        public GameState(float dt)
        {
            Dt = dt;
            Player = new Player(this) {Pos = new Vector2(300, 600) };
            Level = new TestLevel(1000, 1000);
        }
    }
}
