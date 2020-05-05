﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Extensions;
using The_Game.Mobs;

namespace The_Game.MobAI
{
    public static class DijkstraPathFinder
    {
        public static IEnumerable<MobPath> FindPaths(
            Vector2 startPoint,
            Dictionary<Vector2, Dictionary<Vector2, float>> graph,
            IEnumerable<Tuple<Vector2, Mob>> targetsAndPositions)
        {
            var targets = targetsAndPositions.ToDictionary(target => target.Item1, target => target.Item2);
            var foundPaths = new Dictionary<Vector2, PathNode<Vector2>>();
            foundPaths[startPoint] = new PathNode<Vector2>(startPoint, 0, null);
            var pointsToOpen = new HashSet<Vector2>() { startPoint };
            var openedPoints = new HashSet<Vector2>();
            while (targets.Count > 0 && pointsToOpen.Count > 0)
            {
                var pointToOpen = pointsToOpen.MinBy(point => foundPaths[point].Cost);
                pointsToOpen.Remove(pointToOpen);
                var pointToOpenPath = foundPaths[pointToOpen];
                var neighboursDists = graph[pointToOpen];
                if (targets.TryGetValue(pointToOpen, out var mob))
                {
                    targets.Remove(pointToOpen);
                    yield return new MobPath(mob, pointToOpenPath);
                }
                foreach (var neighbour in GetNotOpenedNeighbours(pointToOpen, graph, openedPoints))
                {
                    var newCost = pointToOpenPath.Cost + neighboursDists[neighbour];
                    if (foundPaths.TryGetValue(neighbour, out var foundPath))
                    {
                        if (foundPath.Cost > newCost)
                        {
                            foundPath.Cost = newCost;
                            foundPath.Previous = pointToOpenPath;
                        }
                    }
                    else
                    {
                        foundPaths[neighbour] = new PathNode<Vector2>(neighbour, newCost, pointToOpenPath);
                        pointsToOpen.Add(neighbour);
                    }
                }
                openedPoints.Add(pointToOpen);
            }
        }

        private static IEnumerable<Vector2> GetNotOpenedNeighbours(
            Vector2 point, Dictionary<Vector2, Dictionary<Vector2, float>> graph,
            HashSet<Vector2> openedPoints)
        {
            if (!graph.ContainsKey(point)) return Enumerable.Empty<Vector2>();
            return graph[point].Keys
                .Where(neighbour => !openedPoints.Contains(neighbour));
        }
    }
}
