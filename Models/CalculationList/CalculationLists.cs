namespace Klarinator3000.Models
{
    public class CalculationLists
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Month { get; set; }
        private List<Expense> Expenses;
        public CalculationLists(string name, string lastname, string month)
        {
            Name = name;
            LastName = lastname;
            Month = month;
            Expenses = new List<Expense>();
        }
    }
}

