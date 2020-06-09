using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Cutscenes;
using The_Game.Levels;
using The_Game.Levels.ActiveLevels;
using The_Game.Model;

namespace The_Game.Mobs
{
    class RebelLeader : Mob
    {
        public static Size RebelLeaderSize = new Size(3 * 90, 310);
        public static Size RebelLeaderAttackSize
            = new Size((int)(RebelLeaderSize.Width), RebelLeaderSize.Height);

        public override string[] Textures
            => new[] { "RebelLeaderLeft", "RebelLeaderRight",
                "RebelLeaderAttackLeft", "RebelLeaderAttackRight" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            if (UpdatesSinceLastAttack < AttackTimeUpdates * 2 / 3)
            {
                return Dir == Direction.Left
                    ? "RebelLeaderAttackLeft"
                    : "RebelLeaderAttackRight";
            }
            return Dir == Direction.Left
                ? "RebelLeaderLeft"
                : "RebelLeaderRight";
        }

        protected override void TakeDamage(Mob attacker, int dmg)
        {
            base.TakeDamage(attacker, dmg);
            if (IsDead)
            {
                Game.ChangeLevel(new KingBattleLevelBuilder());
                Game.ShowCutscene(Cutscene.RebellionOverCutscene);
            }
        }

        public override void Update()
        {
            base.Update();
            var pos = Pos;
            if (pos.X > 1800 || pos.X < 0 || pos.Y < -100 || pos.Y > 1000)
                Pos = new Vector2(1500, 700);
        }


        public RebelLeader(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, RebelLeaderSize,
                  DrawingPriority.Mob, startPos, 300, 20, 70)
        {
            TextureSizes = TextureSizesBuilder.Build(
                "RebelLeaderLeft", "RebelLeaderRight",
                "RebelLeaderAttackLeft", "RebelLeaderAttackRight",
                RebelLeaderSize, RebelLeaderAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "RebelLeaderLeft", "RebelLeaderRight",
                "RebelLeaderAttackLeft", "RebelLeaderAttackRight",
                RebelLeaderSize, RebelLeaderAttackSize
            );
        }
    }
}
