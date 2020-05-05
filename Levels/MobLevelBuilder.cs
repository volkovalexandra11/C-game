using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using The_Game.Interfaces;
using The_Game.Model;
using The_Game.Mobs;
using System.Runtime.InteropServices;

namespace The_Game.Levels
{
    class MobLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new Background(size),
                new Ground(new Size(2000, 300), new Point(-100, 800)),
                new CrumbledWall(new Size(200, 700), new Point(0, 300)),
                new Ground(new Size(900, 150), new Point(900, 300)),
                new Rogue(game, level, new Size(90, 210), new Vector2(1600, 300))
            }.AsReadOnly();
            var waypoints = new Vector2[]
            {
                new Vector2(1800, 300), new Vector2(1500, 300), new Vector2(1200, 300), new Vector2(900, 300),
                new Vector2(750, 550), new Vector2(1050, 550),
                new Vector2(200, 800), new Vector2(550, 800), new Vector2(900, 800), new Vector2(1200, 800),
                new Vector2(1500, 800), new Vector2(1800, 800)
            };
            var adjacencyLists = new Vector2[][]
            {
                new [] { new Vector2(1800, 300), new Vector2(1500, 300) },
                new [] { new Vector2(1500, 300), new Vector2(1800, 300), new Vector2(1200, 300) },
                new [] { new Vector2(1200, 300), new Vector2(1500, 300), new Vector2(900, 300) },
                new [] { new Vector2(900, 300), new Vector2(1200, 300), new Vector2(750, 550), new Vector2(1050, 550) },
                new [] { new Vector2(750, 550), new Vector2(550, 800), new Vector2(900, 800) },
                new [] { new Vector2(1050, 550), new Vector2(900, 800), new Vector2(1200, 800)},
                new [] { new Vector2(200, 800), new Vector2(550, 800) },
                new [] { new Vector2(550, 800), new Vector2(200, 800), new Vector2(900, 800) },
                new [] { new Vector2(900, 800), new Vector2(550, 800), new Vector2(1200, 800) },
                new [] { new Vector2(1200, 800), new Vector2(900, 800), new Vector2(1500, 800) },
                new [] { new Vector2(1500, 800), new Vector2(1200, 800), new Vector2(1800, 800) },
                new [] { new Vector2(1800, 800), new Vector2(1500, 800) }
            };
            return new LevelData(
                size,
                new Vector2(50, 300),
                entities,
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
