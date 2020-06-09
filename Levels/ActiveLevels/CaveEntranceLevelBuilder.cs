using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Cutscenes;
using The_Game.Interfaces;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Levels.ActiveLevels
{
    class CaveEntranceLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new CaveBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new CaveFloor(new Size(500, 200), new Point(0, 800)),
                new CaveFloor(new Size(1300, 250), new Point(500, 750)),
                new UndergroundWall(new Size(450, 100), new Point(1350, 250)),
                new UndergroundWall(new Size(60, 150), new Point(1350, 100)),
                new LevelExit(game, new LivingQuartersLevelBuilder(), new Size(100, 400), new Point(1700, 350)),
                new Oldman(game, level, new Vector2(1500, 250)),
                new Rebel(game, level, new Vector2(700, 750)),
                new Rebel(game, level, new Vector2(1100, 750)),
                new Rebel(game, level, new Vector2(1500, 750))
            }.AsReadOnly();
            var waypoints = new Vector2[]
            {
                new Vector2(150, 800), new Vector2(300, 800), new Vector2(450, 800),
                new Vector2(600, 750), new Vector2(650, 750), new Vector2(800, 750),
                new Vector2(950, 750), new Vector2(1100, 750), new Vector2(1250, 750),
                new Vector2(1300, 750), new Vector2(1450, 750), new Vector2(1600, 750),
            };
            var adjacencyLists = new Vector2[][]
            {
                new [] { new Vector2(150, 800), new Vector2(300, 800) },
                new [] { new Vector2(300, 800), new Vector2(450, 800), new Vector2(150, 800) },
                new [] { new Vector2(450, 800), new Vector2(600, 750), new Vector2(300, 800) },
                new [] { new Vector2(600, 750), new Vector2(650, 750), new Vector2(450, 800) },
                new [] { new Vector2(650, 750), new Vector2(800, 750), new Vector2(600, 750) },
                new [] { new Vector2(800, 750), new Vector2(950, 750), new Vector2(650, 750) },
                new [] { new Vector2(950, 750), new Vector2(1100, 750), new Vector2(800, 750) },
                new [] { new Vector2(1100, 750), new Vector2(1250, 750), new Vector2(950, 750) },
                new [] { new Vector2(1250, 750), new Vector2(1300, 750), new Vector2(1100, 750) },
                new [] { new Vector2(1300, 750), new Vector2(1450, 750), new Vector2(1250, 750) },
                new [] { new Vector2(1450, 750), new Vector2(1600, 750), new Vector2(1300, 750) },
                new [] { new Vector2(1600, 750), new Vector2(1450, 750) }
            };
            return new LevelData(
                size,
                new Vector2(150, 800),
                entities,
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
