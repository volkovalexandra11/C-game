using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Levels.ActiveLevels;
using The_Game.Model;

namespace The_Game.Levels
{
    class IntroLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            return new LevelData
            (
                size,
                new Vector2(100, 800),
                new List<IEntity>()
                {
                    new TutorialBackground(size),
                    new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                    new Ground(new Size(1800, 200), new Point(0, 800)),
                    new Stump(game, new Size(100, 80), new Point(1500, 180+10)),
                    new Ladder(new Size(60, 600), new Point(1240, 250)),
                    new Ground(new Size(100, 40), new Point(500, 760)),
                    new Ground(new Size(500, 550), new Point(1300, 250)),
                    new LevelExit(game, new MobIntroLevelBuilder(), new Size(100, 200), new Point(1700, 0))
                }.AsReadOnly()
            );
        }
    }
}
