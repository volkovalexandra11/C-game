using The_Game.Levels;
using The_Game.Model;

namespace The_Game.Interfaces
{
    public interface ILevelBuilder
    {
        LevelData BuildData(GameState game);
    }
}
