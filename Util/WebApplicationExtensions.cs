using tribal_credit_line_application.Middleware;

namespace tribal_credit_line_application.Util
{
    public static class WebApplicationExtensions
    {
        public static void Config(this WebApplication app)
        {
            app.UseMiddleware<RateLimitMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
