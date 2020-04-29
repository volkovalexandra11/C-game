using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Mobs
{
    public partial class Player : Mob
    {
        public static string[] AllTextures
            => new[] { "KnightLeft.png", "KnightRight.png" };
        public string ChooseTexture()
        {
            return Dir == Direction.Left
                ? "KnightLeft.png"
                : "KnightRight.png";
        }

        public Player(GameState game)
            : base
            (
                  game, true, new Size(90, 210), DrawingPriority.Player,
                  AllTextures, Vector2.Zero
            )
        {
            getTexture = ChooseTexture;
            State = MobState.Walking;
            Dir = Direction.Right;
        }

        protected override void ProcessCollision(IEntity otherEnt)
        {
            if (otherEnt is ITrigger triggerEnt && triggerEnt.Active)
                triggerEnt.Trigger();
        }
    }
}
