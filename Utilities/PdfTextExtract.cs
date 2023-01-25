using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Klarinator3000.Utilities
{
    public class PdfTextExtract
    {
        public List<string> PdfPages = new List<string>();
        public PdfTextExtract(IFormFile pdf, ITextExtractionStrategy? strategy = default)
        {
            PdfPages = new List<string>();
            ExtractTextFromPages(pdf);
        }
        public void ExtractTextFromPages(IFormFile pdf)
        {
            using (var stream = pdf.OpenReadStream())
            {
                using (var reader = new PdfReader(stream))
                {
                    PdfDocument pdfDoc = new PdfDocument(reader);

                    for (int index = 1; index < pdfDoc.GetNumberOfPages(); index++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string PdfPageToText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(index), strategy);
                        PdfPages.Add(PdfPageToText);
                    }
                    pdfDoc.Close();
                    reader.Close();
                }
            }
        }
    }
}

