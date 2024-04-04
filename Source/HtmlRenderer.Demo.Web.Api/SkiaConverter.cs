using HtmlRenderer.Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Demo.Common;
using TheArtOfDev.HtmlRenderer.SkiaSharp;

namespace HtmlRenderer.Demo.Web.Api
{
    public class SkiaConverter : SampleConverterBase, IStreamPdfGenerator
    {
        public async Task<Stream> GenerateSampleAsync(HtmlSample sample)
        {
            var config = new PdfGenerateConfig();

            config.PageSize = PageSize.A4;

            config.MarginLeft = 0;
            config.MarginRight = 0;
            config.MarginTop = 0;
            config.MarginBottom = 0;

            var stream = new MemoryStream();
            await PdfGenerator.GeneratePdfAsync(sample.Html, stream, config, imageLoad: OnImageLoaded);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
