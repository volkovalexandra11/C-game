using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using The_Game.Interfaces;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Levels.ActiveLevels
{
    class LivingQuartersLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new CaveBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new CaveFloor(new Size(1450, 100), new Point(0, 900)),
                new CaveFloor(new Size(950, 100), new Point(0, 550)),
                new CaveFloor(new Size(950, 100), new Point(500, 200)),
                new UndergroundWall(new Size(150, 550), Point.Empty),
                new UndergroundWall(new Size(350, 800), new Point(1450, 200)),
                new Ladder(new Size(60, 350), new Point(950, 550)),
                new Ladder(new Size(60, 350), new Point(440, 200)),
                new LevelExit(game, new CaveAscentLevelBuilder(), new Size(150, 200), new Point(1650, 0)),
                new Oldman(game, level, new Vector2(500, 900)),
                new Oldman(game, level, new Vector2(1000, 900)),
                new Oldman(game, level, new Vector2(1350, 900)),
                new Rebel(game, level, new Vector2(850, 550)),
                new Oldman(game, level, new Vector2(500, 550)),
                new Oldman(game, level, new Vector2(300, 550)),
                new Rebel(game, level, new Vector2(900, 200))
            }.AsReadOnly();
            var lowMainWPLine = new [] { 150, 300, 450, 600, 750, 900, 1050, 1200, 1350 }
                .Select(x => new Vector2(x, 900))
                .ToArray();
            var midMainWPLine = new [] { 250, 450, 600, 750, 900, 1050, 1200 }
                .Select(x => new Vector2(x, 550))
                .ToArray();
            var highMainWPLine = new [] { 350, 500, 650, 800, 950, 1100, 1250, 1300, 1450, 1600 }
                .Select(x => new Vector2(x, 200))
                .ToArray();
            var lowAdjList = lowMainWPLine
                .Skip(1)
                .Take(lowMainWPLine.Length - 2)
                .Select((wp, wpInd) => new List<Vector2>() { wp, lowMainWPLine[wpInd], lowMainWPLine[wpInd + 2] })
                .Prepend(new List<Vector2> { lowMainWPLine[0], lowMainWPLine[1] })
                .Append(new List<Vector2> { lowMainWPLine.Last(), lowMainWPLine[lowMainWPLine.Length - 2]})
                .ToArray();
            var midAdjList = midMainWPLine
                .Skip(1)
                .Take(midMainWPLine.Length - 2)
                .Select((wp, wpInd) => new List<Vector2>() { wp, midMainWPLine[wpInd], midMainWPLine[wpInd + 2] })
                .Prepend(new List<Vector2> { midMainWPLine[0], midMainWPLine[1] })
                .Append(new List<Vector2> { midMainWPLine.Last(), midMainWPLine[midMainWPLine.Length - 2]})
                .ToArray();
            var highAdjList = highMainWPLine
                .Skip(1)
                .Take(highMainWPLine.Length - 2)
                .Select((wp, wpInd) => new List<Vector2>() { wp, highMainWPLine[wpInd], highMainWPLine[wpInd + 2] })
                .Prepend(new List<Vector2> { highMainWPLine[0], highMainWPLine[1] })
                .Append(new List<Vector2> { highMainWPLine.Last(), highMainWPLine[highMainWPLine.Length - 2]})
                .ToArray();

            lowAdjList[lowAdjList.Length - 2].Add(midMainWPLine.Last());
            midAdjList.Last().Add(lowMainWPLine[lowMainWPLine.Length - 2]);

            midAdjList[0].Add(highMainWPLine[0]);
            highAdjList[0].Add(midMainWPLine[0]);

            var waypoints = lowMainWPLine.Concat(midMainWPLine).Concat(highMainWPLine).ToArray();
            var adjacencyLists = lowAdjList
                .Concat(midAdjList)
                .Concat(highAdjList)
                .Select(adjList => adjList.ToArray())
                .ToArray();
            return new LevelData(
                size,
                new Vector2(150, 900),
                entities,
                waypoints,
                LevelData.GetReverseGraph(adjacencyLists)
            );
        }
    }
}
