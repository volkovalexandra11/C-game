using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Tests
{
    class MobTests
    {
        private static readonly GameState gs = new GameState();
        private readonly Player player = gs.GamePlayer;
        
        [Test]
        public void MobDoesNotRepeatPlayerAction()
        {
            var mobs = gs.Level.Mobs;
            var initialPos = new Vector2(0,0);
            if (mobs.Count > 0) initialPos = mobs[0].Pos;
            gs.PlayerActions.Add(MobAction.Jump);
            player.Update();
            Assert.AreEqual(initialPos.Y, mobs[0].Pos.Y, 1e-7);
        }

        [SetUp]
        public void SetUp()
        {
            var level = new LevelForTestsWithGraph();
            gs.ChangeLevel(level);
            var plLevel = new Level(gs, level, player);
            player.ChangeLevel(plLevel);
            gs.EndCutscene();
        }

        [Test]
        //чет непонятно, почему не работает(((
        public void PlayerDiesWhenDoingNoting()
        {
            var mobs = gs.Level.Mobs;
            var mPos = mobs[0].Pos;
            var a = player.HP;
            gs.GamePlayer.Pos = new Vector2(500, 700);
            for (var count = 0; count < 10000; count++)
            {
                gs.UpdateModel();
            }
            gs.UpdateModel();
            var b = player.HP;
            var mobPos = gs.Level.Mobs[0].Pos;
            var playerPos = player.Pos;
            Assert.True(player.IsDead);
        }

        [Test]
        public void MobMoves()
        {
            var mob = gs.Level.Mobs[0];
            while (mob.PlannedPath == null)
            {
                gs.UpdateModel();
            }

            var inPos = mob.Pos;
            mob.Update();
            var curPos = mob.Pos;
            Assert.Less(curPos.X, inPos.X);
        }
    }
}
