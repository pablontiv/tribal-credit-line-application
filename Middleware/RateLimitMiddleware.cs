using Microsoft.Extensions.Caching.Memory;
using System.Net;
using tribal_credit_line_application.Model;
using tribal_credit_line_application.Repository;

namespace tribal_credit_line_application.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ApplicationRepository _applicationRepository;

        public RateLimitMiddleware(IMemoryCache cache, ApplicationRepository applicationRepository, RequestDelegate next)
        {
            _next = next;
            _cache = cache;
            _applicationRepository = applicationRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var req = context.Request;
            var error = false;
            if (int.TryParse(req.RouteValues["id"]?.ToString(), out int id))
            { 
                _cache.TryGetValue("lastTime", out DateTime? lastTime);
                _cache.TryGetValue("reqCount", out int reqCount);

                reqCount++;

                if (lastTime == null)
                {
                    lastTime = DateTime.Now;
                }
                else
                {
                    var application = _applicationRepository.GetById(id);

                    if (application != null)
                    {
                        if (lastTime.Value.AddMinutes(2) > DateTime.Now)
                        {
                            if (!application.approved)
                            {
                                if (lastTime.Value.AddSeconds(30) > DateTime.Now)
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                                    error = true;
                                }
                                else if (reqCount >= 3)
                                {
                                    reqCount++;
                                    await context.Response.WriteAsJsonAsync(new ApplicationResponse { status = "ERROR", message = "A sales agent will contact you" });
                                    error = true;
                                }
                            }
                            else if (reqCount >= 3)
                            {
                                reqCount++;
                                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                                error = true;
                            }
                        }
                        else
                        {
                            reqCount = 1;
                        }
                    }
                }

                _cache.Set("reqCount", reqCount);
                _cache.Set("lastTime", DateTime.Now);
            }

            if (!error)
            { 
                await _next(context);
            }
        }
    }
}
