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
                new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(4, 3), new Vector2(5, 8)
            };
            var graphBlank = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8)}
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
                    new Ladder(new Size(100, 80), new Point(650, 620 + 10)),
                    new Rogue(game, level, new Vector2(5, 8))
                }.AsReadOnly(),
                waypoints,
                LevelData.GetReverseGraph(graphBlank)
            );
        }
    }
}
