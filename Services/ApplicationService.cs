using tribal_credit_line_application.Model;
using tribal_credit_line_application.Repository;

namespace tribal_credit_line_application.Services
{
    public class ApplicationService
    {
        private ApplicationRepository _applicationRepository;
        public ApplicationService(ApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public ApplicationResponse CalculateCreditLine(int id, CreditLine creditLine)
        {
            var application = _applicationRepository.GetById(id);

            if (application == null)
            {
                application = new ApplicationResult();

                var acceptedMonthlyRevenue = creditLine.monthlyRevenue / 5;
                var acceptedAmount = acceptedMonthlyRevenue;

                if (creditLine.foundingType == FoundingType.Startup)
                {
                    var acceptedCashBalance = creditLine.cashBalance / 3;
                    acceptedAmount = acceptedMonthlyRevenue > acceptedCashBalance ? acceptedMonthlyRevenue : acceptedCashBalance;
                }

                if (acceptedAmount > creditLine.requestedCreditLine)
                {
                    application.approved = true;
                    application.resultCreditLine = creditLine.requestedCreditLine;
                }
                else
                {
                    application.approved = false;
                    application.resultCreditLine = 0;
                }

                application.resultDate = DateTime.Now;

                _applicationRepository.Add(id, application);
            }

            return application.approved 
                ? new AcceptedApplicationResponse(application.resultCreditLine) 
                : new RejectedApplicationResponse();
        }
    }
}
