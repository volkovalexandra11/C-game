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

        public List<Mob> Mobs { get; }

        public Vector2 StartPos { get; }

        public Vector2[] Waypoints { get; }

        public string LevelName { get; }

        public Dictionary<Vector2, Dictionary<Vector2, float>> WPReverseGraph { get; }

        public Level(GameState game, ILevelBuilder levelBuilder, Player player)
        {
            Game = game;
            var levelData = levelBuilder.BuildData(game, this);
            LevelSize = levelData.LevelSize;
            StartPos = levelData.StartPos;
            Entities = levelData.Entities.Append(player).OrderBy(ent => ent.Priority).ToList();
            Waypoints = levelData.Waypoints;
            WPReverseGraph = levelData.WPRevGraph;
            Mobs = Entities.Where(ent => ent is Mob).Select(mob => (Mob)mob).ToList();
        }

        public Level(GameState game, ILevelBuilder levelBuilder, Player player, string name)
            : this(game, levelBuilder, player)
        {
            LevelName = name;
        }

        public void RemoveMob(Mob mob)
        {
            Entities.Remove(mob);
            Mobs.Remove(mob);
        }
    }
}
