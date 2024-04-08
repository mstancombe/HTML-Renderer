using HtmlRenderer.Demo.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Demo.Common;
using TheArtOfDev.HtmlRenderer.SkiaSharp;

namespace HtmlRenderer.Demo.Console
{
    public class SkiaSvgConverter : SampleConverterFileBase
    {
        public SkiaSvgConverter(string sampleRunIdentifier, string basePath) : base(sampleRunIdentifier, basePath)
        {
        }

        public async Task GenerateSampleAsync(HtmlSample sample)
        {
            var size = new SKSize(500, 1000);
            int width = 500;
            int height = 50;

            using (var fileStream = File.Open(GetSamplePath(sample, ".svg"), FileMode.CreateNew))
            {
                await ImageGenerator.GenerateSvgAsync(sample.Html, fileStream, width, height, imageLoad: OnImageLoaded);
                fileStream.Flush();
            }

            using (var fileStream = File.Open(GetSamplePath(sample, ".png"), FileMode.CreateNew))
            {
                await ImageGenerator.GenerateBitmapAsync(sample.Html, fileStream, SKEncodedImageFormat.Png, width, imageLoad: OnImageLoaded);
                fileStream.Flush();
            }
        }
    }
}
