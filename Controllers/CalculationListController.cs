using Microsoft.AspNetCore.Mvc;
using Klarinator3000.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;

namespace Klarinator3000.Controllers;

public class CalculationListController : Controller
{
    DebtsViewModel? debts;
    IncomeCertificateViewModel? incomeCertificate;
    CalculationListsViewModel? calculationLists;
    private readonly ILogger<HomeController> _logger;

    public CalculationListController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Upload(IFormFile filePoreska, IFormFile filePotvrda, IFormFile fileObracunska)
    {
        if (filePoreska != null && filePoreska.Length > 0)
        {
            debts = new DebtsViewModel(filePoreska);
        }
        if (filePotvrda != null && filePotvrda.Length > 0)
        {
            incomeCertificate = new IncomeCertificateViewModel(filePotvrda);
        }
        if (fileObracunska != null && fileObracunska.Length > 0)
        {
            calculationLists = new CalculationListsViewModel(fileObracunska);
        }

        if (filePoreska != null && filePotvrda != null)
        {
            List<string> listForPrint = new();

            foreach (var worker in incomeCertificate.incomeCertificateExtractor.workersList)
            {
                listForPrint.AddRange(worker.PayMentsList.Split(";"));
            }
            saveBothToDesktop(debts.debtsExtractor.Debts, listForPrint, incomeCertificate.incomeCertificateExtractor.CompanyName);
        }
        if (fileObracunska != null)
        {
            saveCalculationsListToDesktop(calculationLists.calculationListsExtractor.expenses);
        }


        return RedirectToAction("UploadSuccess");
    }
    public IActionResult UploadSuccess()
    {
        return View("~/Views/Magic/Magic.cshtml");
    }
    private void saveBothToDesktop(List<Debt> debtsList, List<string> listForPrint, string CompanyName)
    {
        string fileName = CompanyName.Replace(" ", "");
        string fileExtension = ".csv";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string filePath = Path.Combine(desktopPath, fileName + timestamp + fileExtension);

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteField("ANALITICKA KARTICA");
            csv.NextRecord();
            foreach (var item in debtsList)
            {
                csv.WriteField(item.Ammount);
                csv.WriteField(item.Month);

                csv.NextRecord();
            }
            csv.WriteField("PPP PO POTVRDE");
            csv.NextRecord();
            foreach (var item in listForPrint)
            {
                csv.WriteField(item);
                csv.NextRecord();
            }
        }
    }
    private void saveCalculationsListToDesktop(Dictionary<string, List<Expense>> expenses)
    {
        string fileName = "ObracunskaLista" + DateTime.Now.ToString("MMMM");
        string fileExtension = ".csv";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string filePath = Path.Combine(desktopPath, fileName + timestamp + fileExtension);

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            foreach (var item in expenses)
            {
                csv.WriteField(item.Key);
                csv.NextRecord();
                foreach (var expense in item.Value)
                {
                    csv.WriteField(expense.Percentage);
                }
                csv.NextRecord();
                foreach (var expense in item.Value)
                {
                    csv.WriteField(expense.Ammount);
                }
                csv.NextRecord();

            }
        }
    }

}
