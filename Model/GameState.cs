using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;

namespace The_Game
{
    public class GameState
    {
        public int Dt { get; }
        public Player Player { get; }
        public ILevel Level { get; set; }
        public readonly HashSet<Keys> PressedKeys;

        public GameState(int dt)
        {
            Dt = dt;
            PressedKeys = new HashSet<Keys>();
            Player = new Player(this, new Vector2(400, 700));
            Level = new TestLevel(Player, "Textures");
            Player.Level = Level;
        }

        public void UpdateModel()
        {
            foreach (var mob in Level.Mobs)
            {
                mob.Update();
            }
        }

        public void HandleKey(Keys e, bool down)
        {
            if (down)
                PressedKeys.Add(e);
            else if (PressedKeys.Contains(e))
                PressedKeys.Remove(e);
        }
    }
}
