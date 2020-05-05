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
        private static readonly GameState Gs = new GameState();
        private static Player player = new Player(Gs);
        private Vector2 initialPosition;

        [Test]
        public void TestGoingRightHorizontalPos()
        {
            Gs.PlayerActions.Add(MobAction.GoRight);
            player.Update();
            Assert.True(initialPosition.X < player.Pos.X);
        }

        [Test]
        public void TestGoingRightVerticalPos()
        {
            Gs.PlayerActions.Add(MobAction.GoRight);
            player.Update();
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }

        [Test]
        public void TestGoingLeftHorizontalPos()
        {
            Gs.PlayerActions.Add(MobAction.GoLeft);
            player.Update();
            Assert.True(initialPosition.X > player.Pos.X);
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }

        [Test]
        public void TestGoingLeftVerticalPos()
        {
            Gs.PlayerActions.Add(MobAction.GoLeft);
            player.Update();
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }


        [Test]
        public void TestJumping()
        {
            Gs.PlayerActions.Add(MobAction.Jump);
            player.Update();
            Assert.AreEqual(initialPosition.X, player.Pos.X, 1e-7);
        }

        [SetUp]
        public void SetUp()
        {
            var level = new Level12ForTests();
            Gs.ChangeLevel(level);
            player = new Player(Gs);
            var plLevel = new Level(Gs, new Level12ForTests(), player, nameof(Level12ForTests));
            player.ChangeLevel(plLevel);
            Gs.PlayerActions.Clear();
            initialPosition = player.Pos;
        }
    }
}