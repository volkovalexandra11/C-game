using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Levels.ActiveLevels
{
    class CaveAscentLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new CaveBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new CaveFloor(new Size(350, 100), new Point(0, 900)),
                new CaveFloor(new Size(200, 150), new Point(350, 850)),
                new CaveFloor(new Size(200, 200), new Point(550, 800)),
                new CaveFloor(new Size(200, 250), new Point(750, 750)),
                new CaveFloor(new Size(200, 300), new Point(950, 700)),
                new CaveFloor(new Size(200, 350), new Point(1050, 650)),
                new CaveFloor(new Size(350, 400), new Point(1250, 600)),
                new CaveFloor(new Size(200, 800), new Point(1600, 200)),
                new Ladder(new Size(60, 400), new Point(1540, 200)),
                new LevelExit(game, new RebelLeaderLevelBuilder(), new Size(50, 200), new Point(1750, 0))
            }.AsReadOnly();
            return new LevelData(
                size,
                new Vector2(150, 900),
                entities
            );
        }
    }
}
