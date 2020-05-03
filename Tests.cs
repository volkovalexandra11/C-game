using System.Drawing;
using System.Numerics;
using NUnit.Framework;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game
{
    public class Tests
    {
        private static readonly GameState Gs = new GameState();
        private static Player player = new Player(Gs);
        private Vector2 initialPosition;
        
        [Test]
        public void TestGoingRight()
        {
            Gs.PlayerActions.Add(MobAction.GoRight);
            player.Update();
            Assert.True(initialPosition.X < player.Pos.X);
            Assert.AreEqual(initialPosition.Y, player.Pos.Y, 1e-7);
        }

        [Test]
        public void TestGoingLeft()
        {
            Gs.PlayerActions.Add(MobAction.GoLeft);
            player.Update();
            Assert.True(initialPosition.X > player.Pos.X);
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
            player = new Player(Gs);
            var level = new Level12ForTests();
            Gs.ChangeLevel(level);
            Gs.PlayerActions.Clear();
            initialPosition = player.Pos;
        }

        [Test]
        public void GameStateLevelTest()
        {
            var levelType = "Level 12 For Tests";
            var thisType = Gs.Level.LevelName;
            Assert.True(thisType == levelType);
        }
        
        [Test]
        public void GameStateEntitiesTest()
        {
            var crumbledWall = new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10));
            var stump = new Stump(Gs, new Size(100, 80), new Point(650, 620 + 10));
            Assert.True(Gs.Level.Entities.Contains(crumbledWall));
            Assert.True(!Gs.Level.Entities.Contains(stump));
        }
    }
}
