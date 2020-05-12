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
        public static Size RogueSize = new Size(90, 210);
        public static Size RogueAttackSize
            = new Size((int)(RogueSize.Width * 1.85), RogueSize.Height);

        public override string[] Textures
            => new[] { "RogueLeft", "RogueRight",
                "RogueAttackLeft", "RogueAttackRight" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            if (IsAttacking)
            {
                return Dir == Direction.Left
                    ? "RogueAttackLeft"
                    : "RogueAttackRight";
            }
            return Dir == Direction.Left
                ? "RogueLeft"
                : "RogueRight";
        }

        public Rogue(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, RogueSize,
                  DrawingPriority.Mob, startPos, 100, 20, 100)
        {
            TextureSizes = TextureSizesBuilder.Build(
                "RogueLeft", "RogueRight",
                "RogueAttackLeft", "RogueAttackRight",
                RogueSize, RogueAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "RogueLeft", "RogueRight",
                "RogueAttackLeft", "RogueAttackRight",
                RogueSize, RogueAttackSize
            );
        }
    }
}
