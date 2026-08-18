using HtmlRenderer.Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Demo.Common;
using TheArtOfDev.HtmlRenderer.SkiaSharp;

namespace HtmlRenderer.Demo.Console
{
    public class SkiaPdfConverter : SampleConverterFileBase
    {
        public SkiaPdfConverter(string sampleRunIdentifier, string basePath) : base(sampleRunIdentifier, basePath)
        {
        }

        public async Task GenerateSampleAsync(HtmlSample sample, float zoomLevel = 1, int margin = 0)
        {
            var config = new PdfGenerateConfig();

            config.PageSize = PageSize.A4;
            
            config.MarginLeft = margin;
            config.MarginRight = margin;
            config.MarginTop = margin;
            config.MarginBottom = margin;

            config.ZoomLevel = zoomLevel;

            using (var fileStream = File.Open(GetSamplePath(sample, zoomLevelPercent: (int)(config.ZoomLevel * 100)), FileMode.CreateNew))
            {
                await PdfGenerator.GeneratePdfAsync(sample.Html, fileStream, config, imageLoad: OnImageLoaded);
                fileStream.Flush();
            }
        }
    }
}
