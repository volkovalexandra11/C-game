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

        public override void Update()
        {
            FindPathsToPlayer();
            base.Update();
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

        private void FindPathsToPlayer()
        {
            foreach (var pathToPlayer in DijkstraPathFinder.FindPaths(
                GetClosestWaypoint(), MobLevel.WPReverseGraph, GetAIControlledMobData()
            ))
            {
                pathToPlayer.Mob.PlannedPath = pathToPlayer;
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
