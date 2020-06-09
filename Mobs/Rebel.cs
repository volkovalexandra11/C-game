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
    class Rebel : Mob
    {
        public static Size RebelSize = new Size(90, 210);
        public static Size RebelAttackSize
            = new Size((int)(RebelSize.Width * 1.85), RebelSize.Height);

        public override string[] Textures
            => new[] { "RebelLeft", "RebelRight",
                "RebelAttackLeft", "RebelAttackRight" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            if (UpdatesSinceLastAttack < AttackTimeUpdates * 2 / 3)
            {
                return Dir == Direction.Left
                    ? "RebelAttackLeft"
                    : "RebelAttackRight";
            }
            return Dir == Direction.Left
                ? "RebelLeft"
                : "RebelRight";
        }

        public Rebel(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, RebelSize,
                  DrawingPriority.Mob, startPos, 150, 20, 70)
        {
            TextureSizes = TextureSizesBuilder.Build(
                "RebelLeft", "RebelRight",
                "RebelAttackLeft", "RebelAttackRight",
                RebelSize, RebelAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "RebelLeft", "RebelRight",
                "RebelAttackLeft", "RebelAttackRight",
                RebelSize, RebelAttackSize
            );
        }
    }
}
