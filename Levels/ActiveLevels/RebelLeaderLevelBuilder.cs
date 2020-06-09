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
    class RebelLeaderLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new CaveBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new CaveFloor(new Size(500, 300), new Point(0, 700)),
                new CaveFloor(new Size(700, 250), new Point(500, 750)),
                new CaveFloor(new Size(600, 300), new Point(1200, 700)),
                new UndergroundWall(new Size(200, 700), new Point(1600, 0)),
                new RebelLeader(game, level, new Vector2(1400, 700))
            }.AsReadOnly();
            var waypoints = new Vector2[]
            {
                new Vector2(100, 700), new Vector2(200, 700), new Vector2(300, 700),
                new Vector2(400, 700), new Vector2(500, 700),
                new Vector2(600, 750), new Vector2(700, 750),
                new Vector2(800, 750), new Vector2(900, 750),
                new Vector2(1000, 750), new Vector2(1100, 750),
                new Vector2(1200, 700), new Vector2(1300, 700),
                new Vector2(1400, 700)
            };
            var adjacencyLists = waypoints
                .Skip(1)
                .Take(waypoints.Length - 2)
                .Select((wp, wpInd) => new [] { wp, waypoints[wpInd], waypoints[wpInd + 2] })
                .Prepend(waypoints.Take(2).ToArray())
                .Append(new [] { waypoints.Last(), waypoints[waypoints.Length - 2] })
                .ToArray();
            return new LevelData(
                size,
                new Vector2(150, 700),
                entities,
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
