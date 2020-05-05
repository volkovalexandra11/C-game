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

        public List<Mob> Mobs { get; }

        public Vector2 StartPos { get; }

        public Vector2[] Waypoints { get; }

        public string LevelName { get; }

        public Dictionary<Vector2, Dictionary<Vector2, float>> WPReverseGraph { get; }

        public Level(GameState game, ILevelBuilder levelBuilder, Player player, string levelName)
        {
            Game = game;
            var levelData = levelBuilder.BuildData(game, this);
            levelName = HandleName(levelName);
            LevelName = levelName;
            LevelSize = levelData.LevelSize;
            StartPos = levelData.StartPos;
            Entities = levelData.Entities.Append(player).OrderBy(ent => ent.Priority).ToList();
            Waypoints = levelData.Waypoints;
            WPReverseGraph = levelData.WPRevGraph;
            Mobs = Entities.Where(ent => ent is Mob).Select(mob => (Mob)mob).ToList();
        }

        private static string HandleName(string levelName)
        {
            var name = levelName;
            if (name.StartsWith("The_Game"))
                name = levelName.Split('.')[2];
            name += 'A';
            var sb = new StringBuilder();
            var words = new List<string>();

            for (var letterInd = 0; letterInd < name.Length; letterInd++)
            {
                if (char.IsUpper(name[letterInd]))
                {
                    if (sb.Length > 0)
                    {
                        words.Add(sb.ToString());
                    }

                    sb.Clear();
                }

                if (char.IsDigit(name[letterInd]))
                {
                    if (letterInd != 0 && !char.IsDigit(name[letterInd - 1]))
                    {
                        if (sb.Length > 0)
                        {
                            words.Add(sb.ToString());
                        }

                        sb.Clear();
                    }
                }

                sb.Append(name[letterInd]);
            }

            //words.Add(sb.ToString());

            var resultingName = string.Join(" ", words.ToArray());
            return resultingName;
        }
    }
}
