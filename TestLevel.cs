using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game
{
    class TestLevel : ILevel
    {
        public Size LevelSize { get; set; }

        public TestLevel(int lvlWidth, int lvlHeight)
        {
            LevelSize = new Size(lvlWidth, lvlHeight);
        }
    }
}
