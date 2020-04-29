using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Levels
{
    public class Level
    {
        public GameState Game { get; }

        public Size LevelSize { get; }

        public List<IEntity> Entities { get; }

        public List<IMob> Mobs { get; }

        public Vector2 StartPos { get; }

        public Level(GameState game, LevelData data, Player player)
        {
            LevelSize = new Size(1800, 1000);
            Game = game;
            StartPos = data.StartPos;
            Entities = data.Entities.Append(player).OrderBy(ent => ent.Priority).ToList();
            Mobs = Entities.Where(ent => ent is IMob).Select(mob => (IMob)mob).ToList();
        }
    }
}
