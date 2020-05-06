using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using The_Game.Interfaces;
using The_Game.Model;

namespace The_Game.Levels
{
    class Level12ForTests : ILevelBuilder
    {
        public LevelData BuildData(GameState game, Level level)
        {
            var size = new Size(1800, 1000);
            return new LevelData
            (
                size,
                new Vector2(400, 700),
                new List<IEntity>()
                {
                    new Background(size),
                    new Ground(new Size(1800, 300), new Point(0, 700)),
                    new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10)),
                    new Ladder(new Size(100, 80), new Point(650, 620 + 10))
                }.AsReadOnly()
            );
        }
    }
}
