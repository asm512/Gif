using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace gif2png
{
    class Gif
    {
        public string Path { private set; get; }

        public string ExtractPath { private set; get; }

        public Gif(string path)
        {
            if (!File.Exists(path)) { throw new FileNotFoundException(); }
            this.Path = path;
        }

        public override string ToString() => System.IO.Path.GetFileName(Path);

        public void SetExtractPath(string path)
        {
            if (!Directory.Exists(path)) { throw new DirectoryNotFoundException(); }
            if (path.EndsWith(@"\")) { this.ExtractPath = path; }
            else { this.ExtractPath = path + @"\"; }
        }

        private double ConvertByteToMegabyte(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public double Size()
        {
            Int64 fileSizeInBytes = new FileInfo(Path).Length;
            return Math.Round(ConvertByteToMegabyte(fileSizeInBytes), 3);
        }

        public int FrameInterval()
        {
            Image gif = Image.FromFile(Path);
            PropertyItem item = gif.GetPropertyItem(0x5100);
            int interval = (item.Value[0] + item.Value[1] * 256) * 13;
            return interval;
        }

        public int FrameCount()
        {
            Image gif = Image.FromFile(Path);
            FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
            int frames = gif.GetFrameCount(dimension);
            return frames;
        }

        public enum OutputFormat
        {
            JPG,
            PNG,
            BMP,
            ICO
        }

        public void ExtractFrame(int frame, OutputFormat extractFormat, string prefix = "x")
        {
            Image gifImg = Image.FromFile(Path);
            gifImg.SelectActiveFrame(FrameDimension.Time, frame);

            switch (extractFormat)
            {
                case OutputFormat.JPG:
                    var outputJpg = ExtractPath + prefix + frame + ".jpg";
                    gifImg.Save(outputJpg, ImageFormat.Jpeg);
                    return;

                case OutputFormat.PNG:
                    var outputPng = ExtractPath + prefix + frame + ".png";
                    gifImg.Save(outputPng, ImageFormat.Png);
                    return;

                case OutputFormat.BMP:
                    var outputBMP = ExtractPath + prefix + frame + ".bmp";
                    gifImg.Save(outputBMP, ImageFormat.Bmp);
                    return;

                case OutputFormat.ICO:
                    var outputIco = ExtractPath + prefix + frame + ".ico";
                    gifImg.Save(outputIco, ImageFormat.Icon);
                    return;
            }
        }

        public void Extract(OutputFormat ExtractFormat, string prefix = "x")
        {
            Image gifImg = Image.FromFile(Path);
            int frames = gifImg.GetFrameCount(FrameDimension.Time);

            switch (ExtractFormat)
            {
                case OutputFormat.JPG:
                    for (int i = 0; i < frames; i++)
                    {
                        gifImg.SelectActiveFrame(FrameDimension.Time, i);
                        var outputFile = ExtractPath + prefix + i + ".jpg";
                        gifImg.Save(outputFile, ImageFormat.Jpeg);
                    }
                    return;
                case OutputFormat.PNG:
                    for (int i = 0; i < frames; i++)
                    {
                        gifImg.SelectActiveFrame(FrameDimension.Time, i);
                        var outputFile = ExtractPath + prefix + i + ".png";
                        gifImg.Save(outputFile, ImageFormat.Png);
                    }
                    return;
                case OutputFormat.BMP:
                    for (int i = 0; i < frames; i++)
                    {
                        gifImg.SelectActiveFrame(FrameDimension.Time, i);
                        var outputFile = ExtractPath + prefix + i + ".bmp";
                        gifImg.Save(outputFile, ImageFormat.Bmp);
                    }
                    return;
                case OutputFormat.ICO:
                    var outputIco = ExtractPath + prefix + ".ico";
                    gifImg.Save(outputIco, ImageFormat.Icon);
                    return;
            }
        }
    }
}
