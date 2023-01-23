using HtmlAgilityPack;
using Klarinator3000.Utilities;

namespace Klarinator3000.Models
{
    public class IncomeCertificateViewModel
    {
        public IncomeCertificateExtractor incomeCertificateExtractor;
        public IncomeCertificateViewModel(IFormFile inputHTML)
        {
            incomeCertificateExtractor = new IncomeCertificateExtractor(inputHTML);
        }
    }
}




