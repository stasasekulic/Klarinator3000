namespace Klarinator3000.Models
{
    public class Debt
    {
        public string Type { get; set; }
        public string Date { get; set; }
        public string Month { get; set; }
        public string Ammount { get; set; }
        public Debt(string type, string date, string month, string ammount)
        {
            Type = type;
            Date = date;
            Month = month;
            Ammount = ammount;
        }
    }

}

