using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Klarinator3000.Utilities
{
    public class PdfTextExtract
    {
        public List<string> PdfPages = new List<string>();
        private ITextExtractionStrategy _Strategy = new SimpleTextExtractionStrategy();
        private string PdfFilePath = String.Empty;
        public PdfTextExtract(IFormFile pdf)
        {
            UploadPdf(pdf);
        }
        private void UploadPdf(IFormFile pdf)
        {
            var fileName = Path.GetTempFileName() + ".pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                pdf.CopyTo(stream);
                PdfFilePath = filePath;
            }
        }
        public void ExtractTextFromPages()
        {
            if (File.Exists(PdfFilePath))
            {
                PdfReader pdfReader = new PdfReader(PdfFilePath);
                PdfDocument pdfDoc = new PdfDocument(pdfReader);

                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                    PdfPages.Add(pageContent);
                }
                pdfDoc.Close();
                pdfReader.Close();

                File.Delete(PdfFilePath);
            }
            else
            {
                throw new ArgumentException("File is not uploaded yet!!!");

            }
        }
    }
}

