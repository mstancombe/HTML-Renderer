// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using HtmlRenderer.Core.Dom;
using SkiaSharp;
using Svg.Skia;
using System;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.Core.Utils;
using TheArtOfDev.HtmlRenderer.SkiaSharp.Adapters;
using TheArtOfDev.HtmlRenderer.SkiaSharp.Utilities;

namespace TheArtOfDev.HtmlRenderer.SkiaSharp
{
    /// <summary>
    /// TODO:a add doc
    /// </summary>
    public static class ImageGenerator
    {
        /// <summary>
        /// Adds a font mapping from <paramref name="fromFamily"/> to <paramref name="toFamily"/> iff the <paramref name="fromFamily"/> is not found.<br/>
        /// When the <paramref name="fromFamily"/> font is used in rendered html and is not found in existing 
        /// fonts (installed or added) it will be replaced by <paramref name="toFamily"/>.<br/>
        /// </summary>
        /// <remarks>
        /// This fonts mapping can be used as a fallback in case the requested font is not installed in the client system.
        /// </remarks>
        /// <param name="fromFamily">the font family to replace</param>
        /// <param name="toFamily">the font family to replace with</param>
        public static void AddFontFamilyMapping(string fromFamily, string toFamily)
        {
            ArgChecker.AssertArgNotNullOrEmpty(fromFamily, "fromFamily");
            ArgChecker.AssertArgNotNullOrEmpty(toFamily, "toFamily");

            SkiaSharpAdapter.Instance.AddFontFamilyMapping(fromFamily, toFamily);
        }

        /// <summary>
        /// Parse the given stylesheet to <see cref="CssData"/> object.<br/>
        /// If <paramref name="combineWithDefault"/> is true the parsed css blocks are added to the 
        /// default css data (as defined by W3), merged if class name already exists. If false only the data in the given stylesheet is returned.
        /// </summary>
        /// <seealso cref="http://www.w3.org/TR/CSS21/sample.html"/>
        /// <param name="stylesheet">the stylesheet source to parse</param>
        /// <param name="combineWithDefault">true - combine the parsed css data with default css data, false - return only the parsed css data</param>
        /// <returns>the parsed css data</returns>
        public static CssData ParseStyleSheet(string stylesheet, bool combineWithDefault = true)
        {
            return CssData.Parse(SkiaSharpAdapter.Instance, stylesheet, combineWithDefault);
        }

        /// <summary>
        /// Create Svg document from given HTML.<br/>
        /// </summary>
        /// <param name="html">HTML source to create image from</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns>the generated image of the html</returns>
        public static async Task<SKCanvas> GenerateSvgAsync(
            string html,
            Stream outputStream,
            SKSize size,
            CssData cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs> stylesheetLoad = null,
            EventHandler<HtmlImageLoadEventArgs> imageLoad = null)
        {
            // create svg document to render the HTML into
            var canvas = SKSvgCanvas.Create(new SKRect(0, 0, size.Width, size.Height), outputStream);

            // add rendered image
            await DrawSvgAsync(canvas, html, size, cssData, stylesheetLoad, imageLoad);
            canvas.Dispose();

            return canvas;
        }

        /// <summary>
        /// Writes html to a bitmap image
        /// </summary>
        /// <param name="html">HTML source to create image from</param>
        /// <param name="size">The size of the image</param>
        /// <param name="imageFormat">The file format used to encode the image.</param>
        /// <param name="quality">The quality level to use for the image. Quality range from 0-100. Higher values correspond to improved visual quality, but less compression.</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns></returns>
        public static async Task<SKCanvas> GenerateBitmapAsync(
            string html,
            Stream outputStream,
            SKSize size,
            SKEncodedImageFormat imageFormat,
            int quality,
            CssData cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs> stylesheetLoad = null,
            EventHandler<HtmlImageLoadEventArgs> imageLoad = null)
        {

            var bitmap = new SKBitmap((int)size.Width, (int)size.Height);
            var canvas = new SKCanvas(bitmap);

            // add rendered image
            await DrawSvgAsync(canvas, html, size, cssData, stylesheetLoad, imageLoad);
            bitmap.Encode(outputStream, imageFormat, quality);

            return canvas;
        }

        /// <summary>
        /// Create image pages from given HTML and appends them to the provided image document.<br/>
        /// </summary>
        /// <param name="canvas">canvas to draw to</param>
        /// <param name="html">HTML source to create image from</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns>the generated image of the html</returns>
        public static async Task DrawSvgAsync(
            SKCanvas canvas,
            string html,
            SKSize size,
            CssData cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs> stylesheetLoad = null,
            EventHandler<HtmlImageLoadEventArgs> imageLoad = null)
        {
            using var container = new HtmlContainer();
            if (stylesheetLoad != null)
                container.StylesheetLoad += stylesheetLoad;
            if (imageLoad != null)
                container.ImageLoad += imageLoad;

            container.Location = new SKPoint(0, 0);
            //container.MaxSize = size;
            container.MaxSize = new SKSize(size.Width, 0);
            container.PageSize = size;
            container.MarginBottom = 0;
            container.MarginLeft = 0;
            container.MarginRight = 0;
            container.MarginTop = 0;
            container.ScrollOffset = new SKPoint(0, 0);

            await container.SetHtml(html, cssData);

            // layout the HTML with the page width restriction to know how many pages are required
            await container.PerformLayout(canvas);
            await container.PerformPaint(canvas);
        }
    }
}