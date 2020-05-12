using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;
using The_Game.Levels;
using The_Game.MobAI;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Mobs
{
    public partial class Player : Mob
    {
        public static Size PlayerSize => new Size(90, 210);
        public static Size PlayerAttackSize
            => new Size((int)(PlayerSize.Width * 1.85), PlayerSize.Height);

        public override string[] Textures
            => new[] { "KnightLeft", "KnightRight",
                "KnightAttackLeft", "KnightAttackRight" };

        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        private const int MobPathsUpdateTimeUpdates = 80;
        private int TicksSinceLastUpdate { get; set; }

        public override string GetTexture()
        {
            if (IsAttacking)
            {
                return Dir == Direction.Left
                    ? "KnightAttackLeft"
                    : "KnightAttackRight";
            }
            return Dir == Direction.Left
                ? "KnightLeft"
                : "KnightRight";
        }

        public void ChangeLevel(Level newLevel)
        {
            MobLevel = newLevel;
        }

        public override void Update()
        {
            if (TicksSinceLastUpdate > MobPathsUpdateTimeUpdates && MobLevel.WPReverseGraph != null)
            {
                TicksSinceLastUpdate = 0;
                FindPathsToPlayer();
            }
            else
                TicksSinceLastUpdate++;
            base.Update();
        }

        public Player(GameState game)
            : base
            (
                  game, null, true, PlayerSize, DrawingPriority.Player,
                  Vector2.Zero, 1000, 30, 100
            )
        {
            State = MobState.Walking;
            Dir = Direction.Right;
            TicksSinceLastUpdate = MobPathsUpdateTimeUpdates / 2;

            TextureSizes = TextureSizesBuilder.Build(
                "KnightLeft", "KnightRight",
                "KnightAttackLeft", "KnightAttackRight",
                PlayerSize, PlayerAttackSize
            );
            TextureMobPos = TextureMobPosBuilder.Build(
                "KnightLeft", "KnightRight",
                "KnightAttackLeft", "KnightAttackRight",
                PlayerSize, PlayerAttackSize
            );
        }

        private void FindPathsToPlayer()
        {
            foreach (var pathToPlayer in DijkstraPathFinder.FindPaths(
                GetClosestWaypoint(), MobLevel.WPReverseGraph, GetAIControlledMobData()
            ))
            {
                pathToPlayer.Mob.PlannedPath = pathToPlayer.FirstWP;
            }
        }

        private IEnumerable<Tuple<Vector2, Mob>> GetAIControlledMobData()
        {
            return MobLevel.Mobs
                .Where(mob => !(mob is Player))
                .Select(mob => Tuple.Create(mob.GetClosestWaypoint(), mob));
        }

        protected override void ProcessCollision(IEntity otherEnt)
        {
            if (otherEnt is ITrigger triggerEnt && triggerEnt.Active)
                triggerEnt.Trigger();
        }
    }
}
