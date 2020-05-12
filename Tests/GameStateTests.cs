using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using The_Game.Levels;
using The_Game.Mobs;
=======
using The_Game.Cutscenes;
using The_Game.Levels;
>>>>>>> ade6d78c061086c1f13ddb83db9d26cdc06b0d6d
using The_Game.Model;

namespace The_Game.Tests
{
    class GameStateTests
    {
<<<<<<< HEAD
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
=======
        private static readonly GameState gs = new GameState();

        [Test]
        public void ShowsCutsceneTest()
        {
            for (var i = 0; i < 31; i++)
            {
                gs.PlayerActions.Add(MobAction.GoRight);
                gs.UpdateModel();
            }
            var cutscene = gs.CurrentCutscene;
            Assert.AreNotEqual(null ,cutscene);
            Assert.AreEqual(Cutscene.KickTheStumpCutscene, cutscene);
            Assert.Throws<InvalidOperationException>(gs.UpdateModel);
>>>>>>> ade6d78c061086c1f13ddb83db9d26cdc06b0d6d
        }

        [Test]
        public void GameStateChangesLevel()
        {
<<<<<<< HEAD
            var level = Gs.Level;
            var newLevel = new LevelForTestsWithGraph();
            Gs.ChangeLevel(newLevel);
            Assert.AreNotEqual(level, Gs.Level);
=======
            var level = gs.Level;
            var newLevel = new LevelForTestsWithGraph();
            gs.ChangeLevel(newLevel);
            Assert.AreNotEqual(level, gs.Level);
        }

        [SetUp]
        public void SetUp()
        {
            gs.EndCutscene();
            gs.ChangeLevel(new Level12ForTests());
>>>>>>> ade6d78c061086c1f13ddb83db9d26cdc06b0d6d
        }
    }
}
