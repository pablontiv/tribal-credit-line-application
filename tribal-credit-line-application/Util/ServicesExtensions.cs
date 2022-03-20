using tribal_credit_line_application.Repository;
using tribal_credit_line_application.Services;

namespace tribal_credit_line_application.Util
{
    public static class ServicesExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<ApplicationService>();
            services.AddTransient<ApplicationRepository>();
            services.AddMemoryCache();
        }
    }
}
