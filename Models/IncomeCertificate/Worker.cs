using Klarinator3000.Models;
using CsvHelper.Configuration;

namespace Klarinator3000.Models
{
    public class Worker
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PayMentsList { get; set; }
        public string SumOfPaymentString { get; set; }
        public double SumOfPayments;
        public List<double> Payments;
        public Worker(string name, string lastname, List<double> payments)
        {
            Name = name;
            LastName = lastname;
            Payments = payments ?? throw new NullReferenceException("Payments lists cannot be NULL!!!");
            SumOfPayments = Math.Round(Payments.Sum());
            PayMentsList = string.Join(";", payments);
            SumOfPaymentString = SumOfPayments.ToString();
        }
    }
}

