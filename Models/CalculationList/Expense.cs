namespace Klarinator3000.Models
{
    public class Expense
    {
        public string Percentage { get; set; }
        public string Ammount { get; set; }
        public Expense(string percentage, string ammount)
        {
            Percentage = percentage;
            Ammount = ammount;
        }
    }
}

