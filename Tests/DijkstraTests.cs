﻿using System;
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
        public void SimpleTest()
        {
            var adjacencyLists = new[]
            {
                new[] { new Vector2(0, 0), new Vector2(3, 4) }
            };

            var start = new Vector2(0, 0);
            var graph = LevelData.GetGraph(adjacencyLists);
            var mobStartPos = new Vector2(3, 4);
            var target = Tuple.Create(new Vector2(3, 4),
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(start, graph, new []{target}).ToList();
            Assert.AreEqual(1,dijkstraPath.Count);
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            Assert.AreEqual(5, pathCost);
        }

        [Test]
        public void SimpleTestWithNoPath()
        {
            var adjacencyLists = new[]
            {
                new[] { new Vector2(0, 0), new Vector2(3, 4) }
            };

            var start = new Vector2(0, 0);
            var graph = LevelData.GetGraph(adjacencyLists);
            var mobStartPos = new Vector2(5, 8);
            var target = Tuple.Create(new Vector2(5, 8),
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(start, graph, new[] { target }).ToList();
            Assert.AreEqual(0, dijkstraPath.Count);
        }

        [Test]
        public void SimpleTestWithLoop()
        {
            var adjacencyLists = new[]
            {
                new[] { new Vector2(0, 0), new Vector2(3, 4), new Vector2(0,0),  }
            };

            var start = new Vector2(0, 0);
            var graph = LevelData.GetGraph(adjacencyLists);
            var mobStartPos = new Vector2(0, 0);
            var target = Tuple.Create(new Vector2(0, 0),
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(start, graph, new[] { target }).ToList();
            Assert.AreEqual(1, dijkstraPath.Count);
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            Assert.AreEqual(0, pathCost);
        }
        
        [Test]
        public void SimpleTestWithTwoTargets()
        {
            var adjacencyLists = new[]
            {
                new[] { new Vector2(0, 0), new Vector2(3, 4), new Vector2(0,0),  }
            };

            var start = new Vector2(0, 0);
            var graph = LevelData.GetGraph(adjacencyLists);
            var mobStartPos = new Vector2(0, 0);
            var target = new[] 
            { 
                Tuple.Create(new Vector2(0, 0),
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos)),

                Tuple.Create(new Vector2(3, 4),
                    (Mob)new Rogue(Gs, Gs.Level, mobStartPos))
            };
            var dijkstraPath = MobAI.DijkstraPathFinder.FindPaths(start, graph, target).ToList();
            Assert.AreEqual(2, dijkstraPath.Count);
            var pathCost = dijkstraPath[0].FirstWP.Cost;
            Assert.AreEqual(0, pathCost);
            pathCost = dijkstraPath[1].FirstWP.Cost;
            Assert.AreEqual(5, pathCost);
        }

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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                    (Mob) new Rogue(Gs, Gs.Level, startPosFirst)),
                Tuple.Create(new Vector2(1, 2),
                    (Mob) new Rogue(Gs, Gs.Level, startPosSecond))
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos));
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
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos)),
                Tuple.Create(new Vector2(15, 48),
                (Mob)new Rogue(Gs, Gs.Level, mobStartPos))

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
