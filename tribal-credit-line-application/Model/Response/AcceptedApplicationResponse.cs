namespace tribal_credit_line_application.Model.Response
{
    public class AcceptedApplicationResponse: ApplicationResponse
    {
        public AcceptedApplicationResponse()
        {
            status = "ACCEPTED";
            message = "Your application was succesfully accepted.";
        }
    }
}
