using TheArtOfDev.HtmlRenderer.Demo.Common;

namespace HtmlRenderer.Demo.Web.Api
{
    public interface IStreamPdfGenerator
    {
        Task<Stream> GenerateSampleAsync(HtmlSample sample);
    }
}
