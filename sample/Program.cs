using System;
using System.IO;
using LineSegmentDetectorCSharp;
using SkiaSharp;

namespace LineSegmentDetectorCSharpSample
{
    static class Program
    {
        static void Main(string[] args)
        {
            SKImage sKImage = SKImage.FromEncodedData(Directory.GetCurrentDirectory() + "/1");
            SKBitmap sourceBtm = SKBitmap.FromImage(sKImage);
            var cropImageInfo = new SkiaSharp.SKImageInfo(sourceBtm.Width, sourceBtm.Height, SKColorType.Gray8);

            SKBitmap resBtm = new SKBitmap(cropImageInfo);

            SKPaint skp = new SKPaint();
            skp.Color = SKColors.White;
            var lsd = new LSD(sourceBtm);

            for (int i = 1; i <= 10; i++)
            {
                var lines = lsd.FindLines(i * 0.1);

                resBtm = new SKBitmap(cropImageInfo);

                using (SKCanvas canvas = new SKCanvas(resBtm))

                {
                    canvas.DrawColor(SKColors.Black);
                    foreach (var line in lines)
                    {
                        canvas.DrawLine(new SKPoint((float)line.Pnt1.X, (float)line.Pnt1.Y), new SKPoint((float)line.Pnt2.X, (float)line.Pnt2.Y), skp);
                    }
                }
                var resData = resBtm.Encode(SKEncodedImageFormat.Png, 100);
                using (var fs = new FileStream(Directory.GetCurrentDirectory() + "/2 " + (i * 0.1).ToString("0.00"), FileMode.OpenOrCreate))
                {
                    resData.SaveTo(fs);

                }
            }
        }
    }
}
