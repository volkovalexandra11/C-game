using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game.Levels;
using The_Game.Model;

namespace The_Game.Interfaces
{
    public interface ILevelBuilder
    {
        LevelData BuildData(GameState game);
    }
}
