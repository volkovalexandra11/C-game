using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game.Cutscenes;
using The_Game.Levels;
using The_Game.Model;

namespace The_Game.Tests
{
    class GameStateTests
    {
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
        }

        [Test]
        public void GameStateChangesLevel()
        {
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
        }
    }
}
