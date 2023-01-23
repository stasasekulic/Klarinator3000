using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
using Klarinator3000.Utilities;

namespace Klarinator3000.Models
{
    public class IncomeCertificateExtractor
    {
        public List<Worker> workersList = new List<Worker>();
        public string CompanyName;
        private HTMLExtractor htmlExtractor;
        public IncomeCertificateExtractor(IFormFile inputHtml)
        {
            htmlExtractor = new(inputHtml);
            CompanyName = GetCompanyName();
            ExtractWorkers();
        }
        private string GetCompanyName()
        {
            string result = string.Empty;
            if (htmlExtractor.html != null)
            {
                string innerText = Tokens.CompanyNameToken;
                HtmlNode node = htmlExtractor.html.DocumentNode.SelectNodes("//*[text()='" + innerText + "']")[1];

                result = node.NextSibling.InnerText;
            }
            return result;
        }
        private void ExtractWorkers()
        {
            if (htmlExtractor.html != null)
            {
                string innerText = Tokens.WorkerNameToken;
                string payments1InnerText = Tokens.PaymentToken;

                HtmlNodeCollection workers = htmlExtractor.html.DocumentNode.SelectNodes("//*[text()='" + innerText + "']");
                HtmlNodeCollection paymentsNodes = htmlExtractor.html.DocumentNode.SelectNodes("//td[contains(@class, '" + payments1InnerText + "')]");
                int skip = 1;
                for (int i = 0; i < workers.Count(); i++)
                {
                    string workerName = workers[i].NextSibling.InnerText.Split(" ").First();
                    string workerLastName = workers[i].NextSibling.InnerText.Split(" ").Last();

                    List<double> payments = new List<double>();

                    foreach (HtmlAgilityPack.HtmlNode node in paymentsNodes.Skip(skip))
                    {
                        skip++;
                        if (node.InnerText.Equals("3.1"))
                        {
                            break;
                        }
                        if (node.InnerText.Equals("Приходи из радног односа") ||
                           node.InnerText.Equals("Приходи ван радног односа"))
                        {

                        }
                        else
                        {
                            payments.Add(Double.Parse(node.NextSibling.NextSibling.NextSibling.NextSibling.InnerText.Replace(".", "")));
                            payments.Add(Double.Parse(node.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText.Replace(".", "")));
                            payments.Add(Double.Parse(node.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText.Replace(".", "")));
                        }
                    }
                    workersList.Add(new Worker(workerName, workerLastName, payments));
                }
            }
        }
        public void WriteToFile(string filePath)
        {
            if (workersList.Count > 0)
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteField("PPP PO POTVRDE");
                    csv.NextRecord();
                    foreach (var item in workersList)
                    {
                        csv.WriteField(item);
                        csv.NextRecord();
                    }
                }
            }
        }
    }
}




