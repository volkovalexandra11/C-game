using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
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
            levelName = HandleName(levelName);
            LevelName = levelName;
            Game = game;
            LevelData = data;
            StartPos = LevelData.StartPos;
            Entities = LevelData.Entities.Append(player).OrderBy(ent => ent.Priority).ToList();
            Mobs = Entities.OfType<IMob>().ToList();
        }

        private static string HandleName(string levelName)
        {
            var name = levelName.Split('.')[2];
            name += 'A';
            var sb = new StringBuilder();
            var words = new List<string>();

            for (var letterInd = 0; letterInd < name.Length -1; letterInd++)
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

            words.Add(sb.ToString());

            var resultingName = string.Join(" ", words.ToArray());
            return resultingName;
        }
    }
}
