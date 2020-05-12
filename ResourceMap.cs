using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game
{
    public static class TextureMap
    {
        public static string GetTextureFile(string textureName) => textureName + ".png";
    }

    public static class TextMap
    {
        public static string GetTextFile(string textName) => textName + ".txt";
    }
}
