﻿using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using The_Game.Interfaces;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Levels
{
    class Level2 : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var waypoints = new[]
            {
                //new Vector2(1800, 300),
                new Vector2(1500, 300),
                new Vector2(1200, 300),
                new Vector2(900, 300),
                new Vector2(600, 300),
                new Vector2(1200, 550),
                new Vector2(200, 800),
                new Vector2(550, 800),
                new Vector2(900, 800),
               // new Vector2(1200, 800),
                //new Vector2(1500, 800),
                //new Vector2(1800, 800)
            };
            var adjacencyLists = new[]
            {
                new [] { new Vector2(1800, 300), new Vector2(1500, 300) },
                new [] {new Vector2(600, 300), new Vector2(600, 550), new Vector2(900,300),  },
                new [] {new Vector2(600, 550), new Vector2(550, 800),  },
                new [] { new Vector2(1500, 300), new Vector2(1200, 300), new Vector2(1200, 550),  },
                new [] { new Vector2(1200,550), new Vector2(1200,800), new Vector2(900,800), },
                new [] { new Vector2(1200, 300), new Vector2(1500, 300), new Vector2(900, 300) },
                new [] { new Vector2(900, 300), new Vector2(1200, 300), new Vector2(600,300), },
                new [] { new Vector2(550, 800), new Vector2(900, 800), },
                new [] {new Vector2(900, 800), new Vector2(550, 800), new Vector2(1200, 800),  },
                new [] { new Vector2(550, 800), new Vector2(200, 800) },
                new [] { new Vector2(1500, 800), new Vector2(1200, 800), },
                new [] { new Vector2(1800, 800), new Vector2(1500, 800) },
                new [] { new Vector2(1800, 800), new Vector2(1500, 800) }
            };

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
                    new Ground(new Size(800, 150), new Point(700, 330)),
                    new Stump(game, new Size(100, 80), new Point(650, 620 + 10)),
                    new Rogue(game, level, new Vector2(1600, 300)),
                    new LevelExit(game, new Level3(), new Size(200, 700), new Point(1600, 0))
                }.AsReadOnly(),
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
