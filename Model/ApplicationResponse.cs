namespace tribal_credit_line_application.Model
{
    public class ApplicationResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public double? approvedCreditLine { get; set; }
    }
}
