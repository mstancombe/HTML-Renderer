using HtmlRenderer.Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Demo.Common;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace HtmlRenderer.Demo.Web.Api
{
    public class PdfSharpCoreConverter : SampleConverterBase, IStreamPdfGenerator
    {
        public async Task<Stream> GenerateSampleAsync(HtmlSample sample)
        {
            var config = new PdfGenerateConfig();

            config.PageSize = PdfSharpCore.PageSize.A4;
            config.MarginLeft = 0;
            config.MarginRight = 0;
            config.MarginTop = 0;
            config.MarginBottom = 0;

            var pdf = await PdfGenerator.GeneratePdfAsync(sample.Html, config, imageLoad: OnImageLoaded);
            var stream = new MemoryStream();
            pdf.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
