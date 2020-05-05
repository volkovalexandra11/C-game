using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;

namespace The_Game.Levels
{
    public class LevelData
    {
        public Size LevelSize { get; }
        public Vector2 StartPos { get; }
        public ReadOnlyCollection<IEntity> Entities { get; }
        public Vector2[] Waypoints { get; }
        public Dictionary<Vector2, Dictionary<Vector2, float>> WPGraph { get; }

        public LevelData(
            Size size, Vector2 startPos, ReadOnlyCollection<IEntity> entities
        )
        {
            LevelSize = size;
            StartPos = startPos;
            Entities = entities;
        }

        public LevelData(
            Size size, Vector2 startPos, ReadOnlyCollection<IEntity> entities,
            Vector2[] waypoints, Dictionary<Vector2, Dictionary<Vector2, float>> wpDists
        )
            : this(size, startPos, entities)
        { 
            Waypoints = waypoints;
            WPGraph = wpDists;
        }

        public static Dictionary<Vector2, Dictionary<Vector2, float>> GetGraph(
            Vector2[][] adjLists
        )
        {
            return adjLists
                .ToDictionary(
                    pointNeighbours => pointNeighbours[0],
                    pointNeighbours => pointNeighbours
                        .Skip(1)
                        .ToDictionary(
                            neighbour => neighbour,
                            neighbour => Vector2.Distance(pointNeighbours[0], neighbour)
                         )
                );
        }
    }
}
