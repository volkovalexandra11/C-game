using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Tests
{
    class GameStateTests
    {
        private static readonly GameState Gs = new GameState();
        private readonly Player player = new Player(Gs);
        
        [Test]
        public void GameStateLevelTest()
        {
            var levelType = new Level(Gs, new Level12ForTests(), player, nameof(Level12ForTests)).LevelName;
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

        [Test]
        public void GameStateChangesLevel()
        {
            var level = Gs.Level;
            var newLevel = new LevelForTestsWithGraph();
            Gs.ChangeLevel(newLevel);
            Assert.AreNotEqual(level, Gs.Level);
        }
    }
}
