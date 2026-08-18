using HtmlRenderer.Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Demo.Common;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace HtmlRenderer.Demo.Console
{
    public class PdfSharpCoreConverter : SampleConverterFileBase
    {
        public PdfSharpCoreConverter(string sampleRunIdentifier, string basePath) : base(sampleRunIdentifier, basePath)
        {
        }

        public async Task GenerateSampleAsync(HtmlSample sample, double zoomLevel = 1, int margin = 0)
        {
            var config = new PdfGenerateConfig();

            config.PageSize = PdfSharpCore.PageSize.A4;
            config.MarginLeft = margin;
            config.MarginRight = margin;
            config.MarginTop = margin;
            config.MarginBottom = margin;

            config.ZoomLevel = zoomLevel;
            
            var pdf = await PdfGenerator.GeneratePdfAsync(sample.Html, config, imageLoad: OnImageLoaded);
            pdf.Save(GetSamplePath(sample, zoomLevelPercent: (int)(config.ZoomLevel * 100)));
        }
    }
}
