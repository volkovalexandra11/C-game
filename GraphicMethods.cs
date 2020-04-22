using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game
{
    static class GraphicMethods
    {
        public static TextureBrush GetTextureBrush(string imgPath, Size size)
        {
            var img = Image.FromFile(imgPath);
            var bitmap = new Bitmap(img, size);
            var brush = new TextureBrush(bitmap)
            {
                WrapMode = WrapMode.Clamp
            };
            return brush;
        }
    }
}
