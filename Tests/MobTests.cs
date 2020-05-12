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

        [SetUp]
        public void SetUp()
        {
            var level = new LevelForTestsWithGraph();
            gs.ChangeLevel(level);
            gs.EndCutscene();
        }

        [Test]
        public void MobDoesNotRepeatPlayerAction()
        {
            var mobs = gs.Level.Mobs;
            var initialPos = mobs[0].Pos;
            gs.PlayerActions.Add(MobAction.Jump);
            player.Update();
            Assert.AreEqual(initialPos.Y, mobs[0].Pos.Y, 1e-7);
        }

        [Test]
        public void PlayerDiesWhenDoingNoting()
        {
            for (var count = 0; count < 2000; count++)
            {
                gs.UpdateModel();
            }
            Assert.True(player.IsDead);
        }

        [Test]
        public void MobMoves()
        {
            var mob = gs.Level.Mobs[0];
            var inPos = mob.Pos;
            for (var i  = 0; i < 100; i++)
                gs.UpdateModel();
            var curPos = mob.Pos;
            Assert.Less(curPos.X, inPos.X);
            Assert.Greater(curPos.Y, inPos.Y);
            Assert.AreEqual(550, curPos.X, 20);
        }

        [Test]
        public void MobsDoNotKillEachOther()
        {
            var firstMob = gs.Level.Mobs[0];
            var secondMob = gs.Level.Mobs[1];
            var initialHP = firstMob.HP;
            for (var i = 0; i < 1000; i++)
            {
                gs.UpdateModel();
            }
            Assert.AreEqual(initialHP,firstMob.HP);
            Assert.AreEqual(initialHP, secondMob.HP);
        }
    }
}
