using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Levels;
using The_Game.Model;

namespace The_Game.Mobs
{
    class Oldman : Mob
    {
        public static Size OldmanSize = new Size(90, 210);
        public static Size OldmanAttackSize
            = new Size((int)(OldmanSize.Width * 1.85), OldmanSize.Height);

        public override string[] Textures
            => new[] { "OldmanLeft", "OldmanRight",
                "OldmanAttackLeft", "OldmanAttackRight" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            if (UpdatesSinceLastAttack < AttackTimeUpdates * 2 / 3)
            {
                return Dir == Direction.Left
                    ? "OldmanAttackLeft"
                    : "OldmanAttackRight";
            }
            return Dir == Direction.Left
                ? "OldmanLeft"
                : "OldmanRight";
        }

        public Oldman(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, OldmanSize,
                  DrawingPriority.Mob, startPos, 30, 20, 70)
        {
            TextureSizes = TextureSizesBuilder.Build(
                "OldmanLeft", "OldmanRight",
                "OldmanAttackLeft", "OldmanAttackRight",
                OldmanSize, OldmanAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "OldmanLeft", "OldmanRight",
                "OldmanAttackLeft", "OldmanAttackRight",
                OldmanSize, OldmanAttackSize
            );
        }
    }
}
