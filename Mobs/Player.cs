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
        public override string[] Textures
            => new[] { "KnightLeft.png", "KnightRight.png" };
        public override string GetTexture()
        {
            return Dir == Direction.Left
                ? "KnightLeft.png"
                : "KnightRight.png";
        }

        public void ChangeLevel(Level newLevel)
        {
            MobLevel = newLevel;
        }

        public Player(GameState game)
            : base
            (
                  game, null, true, new Size(90, 210), DrawingPriority.Player,
                  Vector2.Zero
            )
        {
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
