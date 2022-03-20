namespace tribal_credit_line_application.Model
{
    public class RejectedApplicationResponse: ApplicationResponse
    {
        public RejectedApplicationResponse()
        {
            status = "REJECTED";
            message = "Your application has been rejected";
        }
    }
}
