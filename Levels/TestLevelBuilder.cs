using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Model;

namespace The_Game.Levels
{
    public class TestLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            return new LevelData
            (
                size,
                new Vector2(400, 700),
                new List<IEntity>()
                {
                    new Background(size),
                    new Ground(new Size(1800, 300), new Point(0, 700)),
                    new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10)),
                    new CrumbledWall(new Size(200, 250), new Point(1500, 0 + 10)),
                    new Stump(game, new Size(100, 80), new Point(650, 620 + 10)),
                    new LevelExit(game, new TestLevel2Builder(), new Size(200, 700), new Point(1600, 0))
                }.AsReadOnly()
            );
        }
    }
}