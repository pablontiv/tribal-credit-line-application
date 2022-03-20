using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using tribal_credit_line_application.Util;

namespace tribal_credit_line_application_test
{
    //Startup needs to inherit from the dependency injection test framework
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencies();
        }
    }
}
