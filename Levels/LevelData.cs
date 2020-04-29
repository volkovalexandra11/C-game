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

        public LevelData(Size size, Vector2 startPos, ReadOnlyCollection<IEntity> entities)
        {
            LevelSize = size;
            StartPos = startPos;
            Entities = entities;
        }
    }
}
