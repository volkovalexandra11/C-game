using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace The_Game
{
    static class TextureLoader
    {
        public static string TextureFolder => "Textures";

        public static Image LoadImage(string imgName)
        {
            return Image.FromFile(Path.Combine(TextureFolder, imgName));
        }

        public static Bitmap LoadBitmap(string imgName, Size size)
        {
            return new Bitmap(LoadImage(imgName), size);
        }

        public static TextureBrush LoadTextureBrush(string imgName, Size size)
        {
            var bitmap = LoadBitmap(imgName, size);
            var brush = new TextureBrush(bitmap)
            {
                WrapMode = WrapMode.Clamp
            };
            return brush;
        }
    }

    static class TextLoader
    {
        public static string TextFolder => "Texts";

        public static string CutsceneSubfolder => "CutscenesText";

        public static string[] LoadCutsceneText(string path)
        {
            var fileLines = LoadText(Path.Combine(TextFolder, CutsceneSubfolder, path));
            var cutsceneText = new List<string>();
            var currentScreenLines = new List<string>();
            foreach (var line in fileLines)
            {
                if (line.Length > 0)
                {
                    currentScreenLines.Add(line);
                }
                else
                {
                    cutsceneText.Add(string.Join("\n", currentScreenLines));
                    currentScreenLines.Clear();
                }
            }
            if (currentScreenLines.Count > 0)
                cutsceneText.Add(string.Join("\n", currentScreenLines));
            return cutsceneText.ToArray();
        }

        private static string[] LoadText(string path)
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }
    }
}
