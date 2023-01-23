using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Klarinator3000.Utilities;

namespace Klarinator3000.Models
{
    public class CalculationListsViewModel
    {
        public CalculationListsExtractor calculationListsExtractor;
        public CalculationListsViewModel(IFormFile inputPdf)
        {
            calculationListsExtractor = new CalculationListsExtractor(inputPdf);
        }
    }

}

