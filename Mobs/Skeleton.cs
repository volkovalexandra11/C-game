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
    class Skeleton : Mob
    {
        public static Size SkeletonSize = new Size(140, 210);
        public static Size SkeletonAttackSize
            = new Size((int)(SkeletonSize.Width * 1.1), SkeletonSize.Height);

        public override string[] Textures
            => new[] { "SkeletonLeft", "SkeletonRight",
                "SkeletonAttackLeft", "SkeletonAttackRight" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            if (UpdatesSinceLastAttack < AttackTimeUpdates * 2 / 3)
            {
                return Dir == Direction.Left
                    ? "SkeletonAttackLeft"
                    : "SkeletonAttackRight";
            }
            return Dir == Direction.Left
                ? "SkeletonLeft"
                : "SkeletonRight";
        }

        public Skeleton(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, SkeletonSize,
                  DrawingPriority.Mob, startPos, 100, 20, 70)
        {
            TextureSizes = TextureSizesBuilder.Build(
                "SkeletonLeft", "SkeletonRight",
                "SkeletonAttackLeft", "SkeletonAttackRight",
                SkeletonSize, SkeletonAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "SkeletonLeft", "SkeletonRight",
                "SkeletonAttackLeft", "SkeletonAttackRight",
                SkeletonSize, SkeletonAttackSize
            );
        }
    }
}
