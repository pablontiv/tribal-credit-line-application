using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using tribal_credit_line_application.Util;

namespace tribal.credit.line.application.tests
{
    internal class CreditLineApplication: WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDependencies();
            });

            return base.CreateHost(builder);
        }
    }
}
