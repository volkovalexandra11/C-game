using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Tests
{
    public class DijkstraTests
    {
        private static readonly GameState Gs = new GameState();

        [Test]
        public void DijkstraFindsWay()
        {

            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8)}
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            Assert.AreEqual(1, dijkstraPath.Count);
        }

        [Test]
        public void DijkstraDoesNotFindWayIfNotVertexInGraph()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8)}
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetReverseGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            Assert.AreEqual(0, dijkstraPath.Count);
        }

        [Test]
        public void DijkstraFindsPathForTwoTargets()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8)}
            };

            var startPoint = new Vector2(0, 0);
            var startPosFirst = new Vector2(5, 8);
            var startPosSecond = new Vector2(1, 2);
            var targets = new[]
            {
                Tuple.Create(new Vector2(5, 8),
                    (Mob) new Rogue(Gs, Gs.Level, new Size(90, 210), startPosFirst)),
                Tuple.Create(new Vector2(1, 2),
                    (Mob) new Rogue(Gs, Gs.Level, new Size(90, 210), startPosSecond))
            };
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, targets).ToList();
            Assert.AreEqual(2, dijkstraPath.Count);
        }

        [Test]
        public void DijkstraTest1()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new [] {new Vector2(4,3), new Vector2(5,8), new Vector2(15,48)},
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(15, 48),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            Assert.AreEqual(1, dijkstraPath.Count);
        }

        [Test]
        public void DijkstraTest2()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8)}
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            var expected = new[] { 13, 34 }.Select(x => Math.Sqrt(x)).Sum();
            Assert.AreEqual(expected, pathCost, 3 * 1e-3);
        }

        [Test]
        public void DijkstraTest3()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8)}
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(1, 2),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            var expected = new[] { 5 }.Select(x => Math.Sqrt(x)).Sum();
            Assert.AreEqual(expected, pathCost, 3 * 1e-3);
        }

        [Test]
        public void DijkstraTest4()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8), new Vector2(15,48) }
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(15, 48),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            var expected = new[] { 5, 2, 4, 11 * 11 + 45 * 45 }.Select(x => Math.Sqrt(x)).Sum();
            Assert.AreEqual(expected, pathCost, 0.1);
        }

        [Test]
        public void DijkstraTest5()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8), new Vector2(15,48) }
            };

            var startPoint = new Vector2(5, 8);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            var expected = new[] { 0 }.Select(x => Math.Sqrt(x)).Sum();
            Assert.AreEqual(expected, pathCost, 0.1);
        }

        [Test]
        public void DijkstraTest6()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new []{new Vector2(4,3), new Vector2(5,8), new Vector2(15,48) }
            };

            var startPoint = new Vector2(5, 8);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos));
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, new[] { target }).ToList();
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            var expected = new[] { 0 }.Select(x => Math.Sqrt(x)).Sum();
            Assert.AreEqual(expected, pathCost, 0.1);
        }

        [Test]
        public void DijkstraTest7()
        {
            var adjacencyLists = new[]
            {
                new [] {new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(5, 8)},
                new [] {new Vector2(1,2), new Vector2(2,3)},
                new [] {new Vector2(2,3), new Vector2(4,3), new Vector2(5,8)},
                new [] {new Vector2(4,3), new Vector2(5,8), new Vector2(15,48)},
                new [] {new Vector2(5,8), new Vector2(15,48)}
            };

            var startPoint = new Vector2(0, 0);
            var mobStartPos = new Vector2(5, 8);
            var target = new[]
            {
                Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos)),
                Tuple.Create(new Vector2(15, 48),
                (Mob)new Rogue(Gs, Gs.Level, new Size(90, 210), mobStartPos))

            };
            var graph = LevelData.GetGraph(adjacencyLists);
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(startPoint, graph, target).ToList();
            var pathCost = dijkstraPath[0].FirstWP.Cost + dijkstraPath[1].FirstWP.Cost;
            var expected = new[] { 13, 34 }.Select(x => Math.Sqrt(x)).Sum()
                + new[] { 13, 34, 10 * 10 + 40 * 40 }.Select(x => Math.Sqrt(x)).Sum();
            Assert.AreEqual(expected, pathCost, 0.1);
        }
    }
}
