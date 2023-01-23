using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Klarinator3000.Utilities;

namespace Klarinator3000.Models
{
    public class DebtsViewModel
    {
        public DebtsExtractor? debtsExtractor;
        public DebtsViewModel(IFormFile inputPdf)
        {
            debtsExtractor = new DebtsExtractor(inputPdf);
        }
    }

}

