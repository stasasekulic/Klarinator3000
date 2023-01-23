using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Klarinator3000.Utilities;

namespace Klarinator3000.Models
{
    public class DebtsExtractor
    {
        private PdfTextExtract _PdfTextExtract;
        public List<Debt> Debts;
        public DebtsExtractor(IFormFile inputPdf)
        {
            _PdfTextExtract = new PdfTextExtract(inputPdf);
            Debts = new List<Debt>();
            ExtractDebts();
        }
        public string ExtractCompanyNameFromPdfPage()
        {
            string result = string.Empty;
            string _PdfPage = string.Empty;
            try
            {
                //Get first page of pdf file
                _PdfPage = _PdfTextExtract.PdfPages[Indexes.CompanyNamePageIndex];
                //Split page via token and get the company name on given index
                result = _PdfPage.Split(Tokens.NewLineToken).ToList()[Indexes.CompanyNameIndex];
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            return result;
        }
        private void ExtractDebts()
        {
            _PdfTextExtract.ExtractTextFromPages();
            foreach (var _page in _PdfTextExtract.PdfPages)
            {
                List<String> page = _page.Split(Tokens.NewLineToken).ToList();

                for (int pageIndex = 0; pageIndex < page.Count(); pageIndex++)
                {
                    string Type = string.Empty;
                    string Date = string.Empty;
                    string Month = string.Empty;
                    string Ammount = string.Empty;

                    if (!page[pageIndex].Contains(Tokens.StornoToken))
                    {
                        if (page[pageIndex].Contains(Tokens.ZaduzenjePoPoreskojToken) && AmmountBiggerThatZero(page[pageIndex + 3]))
                        {
                            Type = page[pageIndex].Substring(11);
                            Date = page[pageIndex].Substring(0, 10);

                            if (!page[pageIndex + 2].Equals("PID--"))
                            {
                                //Month = FormatMonth(page[pageIndex + 2].Substring(0, 6).Replace("-", "").Replace(Tokens.PIDToken, ""));
                                Month = (page[pageIndex + 2].Substring(0, 6).Replace("-", "").Replace(Tokens.PIDToken, ""));
                            }
                            else
                            {
                                //Month = "NoMonth";
                                Month = "O";
                            }
                            Ammount = page[pageIndex + 3].Split(' ').First(); ;
                            Debts.Add(new Debt(Type, Date, Month, Ammount));
                        }
                        if (page[pageIndex].Contains(Tokens.ZaduzenjePoIzmenjenojPoreskojToken))
                        {
                            Type = page[pageIndex] + page[pageIndex + 1];
                            Date = page[pageIndex - 1];
                            //Month = FormatMonth(page[pageIndex + 3].Substring(0, 6).Replace(",", "").Replace(Tokens.PIDToken, "").Replace("-", ""));
                            Month = (page[pageIndex + 3].Substring(0, 6).Replace(",", "").Replace(Tokens.PIDToken, "").Replace("-", ""));
                            Ammount = page[pageIndex + 4].Split(' ').First();
                            Debts.Add(new Debt(Type, Date, Month, Ammount));
                        }
                    }
                }
            }
        }
        private bool AmmountBiggerThatZero(string page)
        {
            string ammountString = string.Empty;
            double ammountNumber = -1;

            //Get ammount from string
            ammountString = page.Trim(',').Split(Tokens.SpaceToken).First();

            if (Double.TryParse(ammountString, out ammountNumber))
            {
                if (ammountNumber > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("The number should be in the correct format : " + ammountString);
            }
        }
        public void WriteToFile(string filePath)
        {
            if (Debts.Count > 0)
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteField("ANALITICKA KARTICA");
                    csv.NextRecord();
                    foreach (var item in Debts)
                    {
                        csv.WriteField(item.Ammount);
                        csv.WriteField(item.Month);

                        csv.NextRecord();
                    }
                }
            }
        }
        private string FormatMonth(string monthNumber)
        {
            if (Enum.TryParse<MonthEnum>(monthNumber, out MonthEnum month))
            {
                return month.ToString();
            }
            else
            {
                throw new ArgumentException("Invalid month value :" + monthNumber);
            }
        }
        enum MonthEnum
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December,
            NoMonth
        }
        internal class Indexes
        {
            public const int CompanyNameIndex = 6;
            public const int CompanyNamePageIndex = 1;
        }
    }

}

