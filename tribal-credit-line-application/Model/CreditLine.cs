namespace tribal_credit_line_application.Model
{
    public class CreditLine
    {
        public string foundingType { get; set; }
        public double cashBalance { get; set; }
        public double monthlyRevenue { get; set; }
        public double requestedCreditLine { get; set; }
        public DateTime requestedDate { get; set; }
    }
}
