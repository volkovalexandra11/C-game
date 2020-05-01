using System.Collections.Generic;
using System.Drawing;

namespace The_Game.Interfaces
{
    public interface ICutscene
    {
        string ImageDirectory { get; }
        string BackgroundImageName { get; }
        TextureBrush BackgroundBrush { get; }

        List<string> Text { get; }
        int LineInd { get; }
        string CurrentLine { get; }

        bool MoveNextLine();
    }
}
