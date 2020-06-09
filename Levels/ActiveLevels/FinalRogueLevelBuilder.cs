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
    class FinalRogueLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new CountrysideBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new Ground(new Size(400, 600), new Point(0, 400)),
                new Ground(new Size(1800, 200), new Point(0, 800)),
                new Ground(new Size(600, 700), new Point(1200, 300)),
                new Ground(new Size(300, 400), new Point(730, 0)),
                new Ladder(new Size(60, 400), new Point(400, 400)),
                new Ladder(new Size(70, 500), new Point(1130, 300)),
                new Rogue(game, level, new Vector2(730, 800)),
                new Rogue(game, level, new Vector2(1030, 800)),
                new Rogue(game, level, new Vector2(1500, 300)),
                new LevelExit(game, new UndeadLevelBuilder(), new Size(200, 300), new Point(1600, 0))
            }.AsReadOnly();
            var waypoints = new Vector2[]
            {
                                         new Vector2(1165, 300), new Vector2(1332, 300), new Vector2(1500, 300),
                new Vector2(430, 600),   new Vector2(1165, 550),
                new Vector2(430, 800), new Vector2(530, 800), new Vector2(630, 800), new Vector2(730, 800),
                new Vector2(830, 800), new Vector2(930, 800), new Vector2(1030, 800), new Vector2(1165, 800)
            };
            var adjacencyLists = new Vector2[][]
            {
                new [] { new Vector2(430, 600), new Vector2(430, 800)},
                new [] { new Vector2(430, 800), new Vector2(430, 600), new Vector2(530, 800) },
                new [] { new Vector2(530, 800), new Vector2(630, 800), new Vector2(430, 800) },
                new [] { new Vector2(630, 800), new Vector2(730, 800), new Vector2(530, 800) },
                new [] { new Vector2(730, 800), new Vector2(830, 800), new Vector2(630, 800) },
                new [] { new Vector2(830, 800), new Vector2(930, 800), new Vector2(730, 800) },
                new [] { new Vector2(930, 800), new Vector2(1030, 800), new Vector2(830, 800) },
                new [] { new Vector2(1030, 800), new Vector2(1165, 800), new Vector2(930, 800) },
                new [] { new Vector2(1165, 800), new Vector2(1030, 800), new Vector2(1165, 550) },
                new [] { new Vector2(1165, 550), new Vector2(1165, 300), new Vector2(1165, 800) },
                new [] { new Vector2(1165, 300), new Vector2(1332, 300), new Vector2(1165, 550) },
                new [] { new Vector2(1332, 300), new Vector2(1500, 300), new Vector2(1165, 300) },
                new [] { new Vector2(1500, 300), new Vector2(1332, 300) }
            };
            return new LevelData(
                size,
                new Vector2(150, 400),
                entities,
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
