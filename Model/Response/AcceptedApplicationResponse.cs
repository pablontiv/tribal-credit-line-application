namespace tribal_credit_line_application.Model.Response
{
    public class AcceptedApplicationResponse: ApplicationResponse
    {
        public double approvedCreditLine { get; set; }
        public AcceptedApplicationResponse(double approvedCreditLine)
        {
            status = "ACCEPTED";
            message = "Your application was succesfully accepted.";
            this.approvedCreditLine = approvedCreditLine;
        }
    }
}
