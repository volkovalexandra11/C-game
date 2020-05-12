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

namespace The_Game.Levels
{
    class LevelForTestsWithGraph : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var waypoints = new[]
            {
                new Vector2(1800, 300), new Vector2(1500, 300), new Vector2(1200, 300), new Vector2(900, 300),
                new Vector2(600, 300), 
                new Vector2(600, 550),  
                new Vector2(200, 800), new Vector2(550, 800), new Vector2(900, 800), new Vector2(1200, 800),
                new Vector2(1500, 800), new Vector2(1800, 800)
            };
            var adjacencyLists = new[]
            {
                new [] { new Vector2(1800, 300), new Vector2(1500, 300) },
                new [] {new Vector2(600, 300), new Vector2(600, 550), new Vector2(900,300),  },
                new [] {new Vector2(600, 550), new Vector2(550, 800),  },
                new [] { new Vector2(1500, 300), new Vector2(1800, 300), new Vector2(1200, 300) },
                new [] { new Vector2(1200, 300), new Vector2(1500, 300), new Vector2(900, 300) },
                new [] { new Vector2(900, 300), new Vector2(1200, 300), new Vector2(600,300), },
                new [] { new Vector2(550, 800), new Vector2(900, 800), },
                new [] {new Vector2(900, 800), new Vector2(550, 800), new Vector2(1200, 800),  },
                new [] { new Vector2(550, 800), new Vector2(200, 800) },
                new [] { new Vector2(1500, 800), new Vector2(1200, 800), new Vector2(1800, 800) },
                new [] { new Vector2(1800, 800), new Vector2(1500, 800) }
            };
            return new LevelData
            (
                size,
                new Vector2(400, 700),
                new List<IEntity>()
                {
                    new Background(size),
                    new Ground(new Size(1800, 300), new Point(0, 700)),
                    new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10)),
                    //new Ladder(new Size(100, 80), new Point(650, 620 + 10)),
                    new Rogue(game, level, new Vector2(600, 300)),
                    new Rogue(game, level, new Vector2(1200, 300))
                }.AsReadOnly(),
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
