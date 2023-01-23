using HtmlAgilityPack;
using Klarinator3000.Models;
namespace Klarinator3000.Utilities
{
    public class HTMLExtractor
    {
        public HtmlDocument html;
        public HTMLExtractor(IFormFile input)
        {
            html = LoadHtml(input) ?? new HtmlDocument();
        }
        private HtmlDocument LoadHtml(IFormFile input)
        {
            using (var stream = input.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                var html = reader.ReadToEnd();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                return htmlDoc;
            }
        }
    }
}

