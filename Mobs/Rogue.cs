using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Levels;
using The_Game.Model;

namespace The_Game.Mobs
{
    public class Rogue : Mob
    {
        public override string[] Textures
            => new[] { "RogueLeft.png", "RogueRight.png",
                "RogueAttackLeft.png", "RogueAttackRight.png" };
        public override string GetTexture()
        {
            if (IsAttacking)
            {
                return Dir == Direction.Left
                    ? "RogueAttackLeft.png"
                    : "RogueAttackRight.png";
            }
            return Dir == Direction.Left
                ? "RogueLeft.png"
                : "RogueRight.png";
        }

        public Rogue(GameState game, Level level, Size size, Vector2 startPos)
            : base(game, level, true, size,
                  DrawingPriority.Mob, startPos, 100, 20, 100)
        {

        }
    }
}
