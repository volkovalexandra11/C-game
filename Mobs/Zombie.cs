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
    public class Zombie : Mob
    {
        public static Size ZombieSize = new Size(150, 210);
        public static Size ZombieAttackSize
            = new Size((int)(ZombieSize.Width * 1.85), ZombieSize.Height);

        public override string[] Textures
            => new[] { "ZombieLeft", "ZombieRight",
                "ZombieAttackLeft", "ZombieAttackRight" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            if (UpdatesSinceLastAttack < AttackTimeUpdates * 2 / 3)
            {
                return Dir == Direction.Left
                    ? "ZombieAttackLeft"
                    : "ZombieAttackRight";
            }
            return Dir == Direction.Left
                ? "ZombieLeft"
                : "ZombieRight";
        }

        public Zombie(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, ZombieSize,
                  DrawingPriority.Mob, startPos, 150, 20, 70)
        {
            TextureSizes = TextureSizesBuilder.Build(
                "ZombieLeft", "ZombieRight",
                "ZombieAttackLeft", "ZombieAttackRight",
                ZombieSize, ZombieAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "ZombieLeft", "ZombieRight",
                "ZombieAttackLeft", "ZombieAttackRight",
                ZombieSize, ZombieAttackSize
            );
        }
    }
}
