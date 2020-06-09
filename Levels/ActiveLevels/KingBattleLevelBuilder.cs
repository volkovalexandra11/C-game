using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Levels.ActiveLevels
{
    class KingBattleLevelBuilder : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            var entities = new List<IEntity>()
            {
                new CastleBackground(size),
                new InvisibleWall(new Size(200, 1000), new Point(-200, 0)),
                new CrumbledWall(new Size(200, 300), new Point(1600, 700)),
                new MagicWallOdd(game, new Size(150, 300), new Point(1300, 700)),
                new MagicWallEven(game, new Size(150, 300), new Point(1000, 700)),
                new MagicWallOdd(game, new Size(150, 300), new Point(700, 700)),
                new MagicWallEven(game, new Size(150, 300), new Point(400, 700)),
                new CrumbledWall(new Size(250, 300), new Point(0, 700)),
                new King(game, level, new Vector2(200, 700))
            }.AsReadOnly();
            return new LevelData(
                size,
                new Vector2(1700, 600),
                entities
            );
        }
    }
}
