namespace tribal_credit_line_application.Model.Response
{
    public class ContactSalesApplicationResponse: ApplicationResponse
    {
        public ContactSalesApplicationResponse()
        {
            status = "ALERT";
            message = "A sales agent will contact you.";
        }
    }
}
