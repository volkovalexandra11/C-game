using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.Levels
{
    public static class TestLevel
    {
        public static Size LevelSize => new Size(1800, 1000);
        public static Vector2 StartPos => new Vector2(400, 700);
        public static ReadOnlyCollection<IEntity> Entities =
            new List<IEntity>()
            {
                new Background(LevelSize),
                new Ground(new Size(1800, 300), new Point(0, 700)),
                new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10)),
                new CrumbledWall(new Size(200, 250), new Point(1500, 450 + 10)),
                new CrumbledWall(new Size(900, 50), new Point(700, 400 + 10))
            }.AsReadOnly();

    }
}
