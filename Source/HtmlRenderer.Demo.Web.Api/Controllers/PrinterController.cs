using Microsoft.AspNetCore.Mvc;
using TheArtOfDev.HtmlRenderer.SkiaSharp;
using TheArtOfDev.HtmlRenderer.Demo.Common;

namespace HtmlRenderer.Demo.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrinterController(ILogger<PrinterController> logger) : ControllerBase
    {
        private readonly ILogger<PrinterController> _logger = logger;


        [HttpGet("SampleNames")]
        public IEnumerable<string> GetSampleNames()
        {
            return SamplesLoader
                .TestSamples
                .Select(s => s.Name);
        }

        [HttpGet("Print")]
        public async Task<IActionResult> PrintSkia(string name)
        {
            var sample = GetSample(name);
            if (sample == null)
            {
                return NotFound();
            }

            var converter = new SkiaConverter();
            return await PrintPdf(converter, sample);
        }


        [HttpGet("PrintLegacy")]
        public async Task<IActionResult> PrintLegacy(string name)
        {
            var sample = GetSample(name);
            if (sample == null)
            {
                return NotFound();
            }

            var converter = new PdfSharpCoreConverter();
            return await PrintPdf(converter, sample);
        }

        private HtmlSample? GetSample(string name)
        {
            var sample = SamplesLoader.TestSamples
                .Where(s => s.FullName.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            return sample;
        }

        private async Task<IActionResult> PrintPdf(IStreamPdfGenerator converter, HtmlSample sample)
        {
            var stream = await converter.GenerateSampleAsync(sample);
            var filename = $"{sample.Name}-{DateTime.Now:yyyyMMdd-hhmmss}";

            return File(stream, "application/pdf", filename);
        }
    }
}
