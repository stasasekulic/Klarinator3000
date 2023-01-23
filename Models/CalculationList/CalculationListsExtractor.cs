using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Klarinator3000.Utilities;

namespace Klarinator3000.Models
{
    public class CalculationListsExtractor
    {
        private PdfTextExtract _PdfTextExtractor;
        public Dictionary<string, List<Expense>> expenses = new Dictionary<string, List<Expense>>();
        public CalculationListsExtractor(IFormFile inputPdf)
        {
            _PdfTextExtractor = new PdfTextExtract(inputPdf);
            ExtractExpenses();
        }
        public string ExtractWorkerNameFromPdfPage(string _page)
        {
            string result = string.Empty;
            var Name = _page.Split("\n")[5].Split(" ")[4];
            var LastName = _page.Split("\n")[5].Split(" ")[3];

            result = Name + "_" + LastName;

            return result;
        }
        private void ExtractExpenses()
        {
            _PdfTextExtractor.ExtractTextFromPages();
            foreach (var _page in _PdfTextExtractor.PdfPages)
            {
                var WorkerKey = ExtractWorkerNameFromPdfPage(_page);
                expenses.TryAdd(WorkerKey, ExtractExpense(_page));
            }
        }
        private List<Expense> ExtractExpense(string _page)
        {
            var WorkerExpenses = new List<Expense>();
            List<String> page = _page.Split(Tokens.NewLineToken).ToList();

            var percentage1 = page[49];
            var percentage2 = page[50];
            var percentage3 = page[51];
            var percentage4 = page[45];
            var percentage5 = page[46];
            var percentage6 = page[47];
            var percentage7 = page[48];

            var ammount1 = page[34];
            var ammount2 = page[35];
            var ammount3 = page[36];
            var ammount4 = page[41];
            var ammount5 = page[42];
            var ammount6 = page[43];
            var ammount7 = page[44];

            WorkerExpenses.Add(new Expense(percentage1, ammount1));
            WorkerExpenses.Add(new Expense(percentage2, ammount2));
            WorkerExpenses.Add(new Expense(percentage3, ammount3));
            WorkerExpenses.Add(new Expense(percentage4, ammount4));
            WorkerExpenses.Add(new Expense(percentage5, ammount5));
            WorkerExpenses.Add(new Expense(percentage6, ammount6));
            WorkerExpenses.Add(new Expense(percentage7, ammount7));

            return WorkerExpenses;
        }
    }

}

