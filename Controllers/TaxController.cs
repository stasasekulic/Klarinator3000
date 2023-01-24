using Microsoft.AspNetCore.Mvc;
using Klarinator3000.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Klarinator3000.Controllers;

public class TaxController : Controller
{
    DebtsViewModel? debts;
    IncomeCertificateViewModel? incomeCertificate;
    CalculationListsViewModel? calculationLists;
    private readonly ILogger<HomeController> _logger;
    public TaxController(ILogger<HomeController> logger)
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

        saveToDesktop(incomeCertificate, debts);
        return RedirectToAction("UploadSuccess");
    }
    public IActionResult UploadSuccess()
    {
        return View("~/Views/Magic/Magic.cshtml");
    }

    private void saveToDesktop(IncomeCertificateViewModel incomeCertificate, DebtsViewModel debts)
    {
        string fileName = string.Empty;

        if (!string.IsNullOrEmpty(incomeCertificate.incomeCertificateExtractor.CompanyName))
        {
            fileName = incomeCertificate.incomeCertificateExtractor.CompanyName.Replace(" ", "");
        }

        string fileExtension = ".csv";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string filePath = Path.Combine(desktopPath, fileName + timestamp + fileExtension);

        if (incomeCertificate.incomeCertificateExtractor.workersList.Count > 0)
        {
            incomeCertificate.incomeCertificateExtractor.WriteToFile(filePath);
        }
        if (debts.debtsExtractor.Debts.Count > 0)
        {
            debts.debtsExtractor.WriteToFile(filePath);
        }

    }

}
