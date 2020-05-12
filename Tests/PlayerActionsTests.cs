using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game
{
    public class PlayerActionsTests
    {
        private static readonly GameState gs = new GameState();
        private static readonly Player player = gs.GamePlayer;
        private Vector2 initialPosition;

        [Test]
        public void TestGoingRightHorizontalPos()
        {
            gs.PlayerActions.Add(MobAction.GoRight);
            player.Update();
            Assert.True(initialPosition.X < player.Pos.X);
        }

        [Test]
        public void TestGoingRightVerticalPos()
        {
            gs.PlayerActions.Add(MobAction.GoRight);
            player.Update();
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }

        [Test]
        public void TestGoingLeftHorizontalPos()
        {
            gs.PlayerActions.Add(MobAction.GoLeft);
            player.Update();
            Assert.True(initialPosition.X > player.Pos.X);
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }

        [Test]
        public void TestGoingLeftVerticalPos()
        {
            gs.PlayerActions.Add(MobAction.GoLeft);
            player.Update();
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }


        [Test]
        public void TestJumping()
        {
            gs.PlayerActions.Add(MobAction.Jump);
            player.Update();
            Assert.AreEqual(initialPosition.X, player.Pos.X, 1e-7);
        }

        [Test]
        public void TestKillingMob()
        {
            var mob = gs.Level.Mobs[0];
            for (var i = 0; i < 100; i++)
                gs.UpdateModel();
            for (var i = 0; i < 100; i++)
            {
                gs.PlayerActions.Add(MobAction.AttackMelee);
                gs.UpdateModel();
            }
            Assert.True(mob.IsDead);
        }

        [SetUp]
        public void SetUp()
        {
            var level = new LevelForTestsWithGraph();
            gs.ChangeLevel(level);
            gs.EndCutscene();
            gs.PlayerActions.Clear();
            initialPosition = player.Pos;
        }
    }
}