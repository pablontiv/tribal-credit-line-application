using tribal_credit_line_application.Model;
using tribal_credit_line_application.Repository;

namespace tribal_credit_line_application.Services
{
    public class ApplicationService
    {
        private ApplicationRepository _applicationRepository;
        private IConfiguration _configuration;
        public ApplicationService(IConfiguration configuration, ApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
            _configuration = configuration;
        }
        public ApplicationResponse CalculateCreditLine(int id, CreditLine creditLine)
        {
            var application = _applicationRepository.GetById(id);

            if (application == null)
            {
                application = new ApplicationResult();

                var acceptedMonthlyRevenue = creditLine.monthlyRevenue / _configuration.GetValue<int>("monthlyRevenueRatio");
                var acceptedAmount = acceptedMonthlyRevenue;

                if (creditLine.foundingType == FoundingType.Startup)
                {
                    var acceptedCashBalance = creditLine.cashBalance / _configuration.GetValue<int>("cashBalanceRatio");
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
