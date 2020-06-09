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
    class UndeadLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new RuinBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new Ground(new Size(700, 200), new Point(0, 800)),
                new Ground(new Size(250, 450), new Point(1000, 300)),
                new Ground(new Size(800, 250), new Point(1000, 750)),
                new Ladder(new Size(60, 450), new Point(940, 300)),
                new Ground(new Size(200, 50), new Point(400, 750)),
                new LevelCutsceneExit(game, new CaveEntranceLevelBuilder(),
                    new Size(100, 750), new Point(1800, 0), Cutscene.HideoutCutscene
                ),
                new Skeleton(game, level, new Vector2(500, 750)),
                new Zombie(game, level, new Vector2(1450, 750))
            }.AsReadOnly();
            var waypoints = new Vector2[]
            {
                new Vector2(1350, 750), new Vector2(1500, 750), new Vector2(1650, 750),
                new Vector2(350, 800), new Vector2(500, 800), new Vector2(650, 800)
            };
            var adjacencyLists = new Vector2[][]
            {
                new [] { new Vector2(1350, 750), new Vector2(1500, 750) },
                new [] { new Vector2(1500, 750), new Vector2(1650, 750), new Vector2(1350, 750) },
                new [] { new Vector2(1650, 750),  new Vector2(1500, 750) },
                new [] { new Vector2(350, 800), new Vector2(500, 800) },
                new [] { new Vector2(500, 800), new Vector2(350, 800), new Vector2(650, 800) },
                new [] { new Vector2(650, 800), new Vector2(500, 800) },
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
