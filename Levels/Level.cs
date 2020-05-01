using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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

        public LevelData LevelData { get; }

        public string LevelName { get; }

        public Level(GameState game, LevelData data, Player player, string levelName)
        {
            LevelSize = new Size(1800, 1000);
            LevelName = levelName;
            Game = game;
            LevelData = data;
            StartPos = LevelData.StartPos;
            Entities = LevelData.Entities.Append(player).OrderBy(ent => ent.Priority).ToList();
            Mobs = Entities.OfType<IMob>().ToList();
        }
    }
}
