using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using tribal.credit.line.application.tests;
using tribal_credit_line_application.Middleware;
using tribal_credit_line_application.Model;
using tribal_credit_line_application.Model.Response;
using tribal_credit_line_application.Repository;

namespace credit.line.application.tests;

public class Tests
{
    [Test]
    public async Task AcceptedCreditForSME()
    {
        await using var app = new CreditLineApplication();
        var creditLine = new CreditLine { foundingType = "SME", cashBalance = 435.30, monthlyRevenue = 4235.40, requestedCreditLine = 100, requestedDate = DateTime.Now };
        var client = app.CreateClient();
        var response =  await client.PostAsJsonAsync("/customer/1/credit/application", creditLine);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var applicationResponse = await response.Content.ReadFromJsonAsync<ApplicationResponse>();
        var expected = new AcceptedApplicationResponse();

        Assert.AreEqual(expected.status, applicationResponse?.status);
        Assert.AreEqual(expected.message, applicationResponse?.message);
    }

    [Test]
    public async Task AcceptedCreditForStartup()
    {
        await using var app = new CreditLineApplication();
        var creditLine = new CreditLine { foundingType = "Startup", cashBalance = 435.30, monthlyRevenue = 4235.40, requestedCreditLine = 500, requestedDate = DateTime.Now };
        var client = app.CreateClient();
        var response = await client.PostAsJsonAsync("/customer/2/credit/application", creditLine);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var applicationResponse = await response.Content.ReadFromJsonAsync<ApplicationResponse>();
        var expected = new AcceptedApplicationResponse();

        Assert.AreEqual(expected.status, applicationResponse?.status);
        Assert.AreEqual(expected.message, applicationResponse?.message);
    }

    [Test]
    public async Task RejectedCreditForSME()
    {
        await using var app = new CreditLineApplication();
        var creditLine = new CreditLine { foundingType = "SME", cashBalance = 435.30, monthlyRevenue = 4235.40, requestedCreditLine = 1000, requestedDate = DateTime.Now };
        var client = app.CreateClient();
        var response = await client.PostAsJsonAsync("/customer/3/credit/application", creditLine);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var applicationResponse = await response.Content.ReadFromJsonAsync<ApplicationResponse>();
        var expected = new RejectedApplicationResponse();

        Assert.AreEqual(expected.status, applicationResponse?.status);
        Assert.AreEqual(expected.message, applicationResponse?.message);
    }

    [Test]
    public async Task RejectedCreditForStartup()
    {
        await using var app = new CreditLineApplication();
        var creditLine = new CreditLine { foundingType = "Startup", cashBalance = 435.30, monthlyRevenue = 4235.40, requestedCreditLine = 1000, requestedDate = DateTime.Now };
        var client = app.CreateClient();
        var response = await client.PostAsJsonAsync("/customer/4/credit/application", creditLine);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var applicationResponse = await response.Content.ReadFromJsonAsync<ApplicationResponse>();
        var expected = new RejectedApplicationResponse();

        Assert.AreEqual(expected.status, applicationResponse?.status);
        Assert.AreEqual(expected.message, applicationResponse?.message);
    }

    [Test]
    public async Task SameAcceptedRegardlessInputs()
    {
        await using var app = new CreditLineApplication();
        var client = app.CreateClient();

        var creditLine1 = new CreditLine { foundingType = "SME", cashBalance = 435.30, monthlyRevenue = 4235.40, requestedCreditLine = 100, requestedDate = DateTime.Now };
        var applicationResponse1 = await await client.PostAsJsonAsync("/customer/5/credit/application", creditLine1)
            .ContinueWith(x => x.Result.Content.ReadFromJsonAsync<ApplicationResponse>());

        var creditLine2 = new CreditLine { foundingType = "SME", cashBalance = 1, monthlyRevenue = 1, requestedCreditLine = 1000, requestedDate = DateTime.Now };
        var applicationResponse2 = await await client.PostAsJsonAsync("/customer/5/credit/application", creditLine1)
            .ContinueWith(x => x.Result.Content.ReadFromJsonAsync<ApplicationResponse>());

        Assert.AreEqual(applicationResponse1?.status, applicationResponse2?.status);
        Assert.AreEqual(applicationResponse1?.message, applicationResponse2?.message);
    }

    [Test]
    public async Task ReturnTooManyRequestIfReceives3OrMoreRequestWithInTwoMinutes()
    {
        await using var app = new CreditLineApplication();
        var httpContext = new DefaultHttpContext();

        httpContext.Request.RouteValues.Add("id", 1);

        var config = (IConfiguration)app.Services.GetService(typeof(IConfiguration));
        var memcache = (IMemoryCache)app.Services.GetService(typeof(IMemoryCache));
        var repository = new ApplicationRepository();
        var applicationResult = new ApplicationResult { approved = true, resultCreditLine = 100, resultDate = DateTime.Now };

        repository.Add(1, applicationResult);

        memcache.Set("lastTime", DateTime.Now);
        memcache.Set("reqCount", 3);

        var middleware = new RateLimitMiddleware(config, memcache, repository, next: (innerHttpContext) => Task.FromResult(0));

        await middleware.InvokeAsync(httpContext);

        Assert.AreEqual((int)HttpStatusCode.TooManyRequests, httpContext.Response.StatusCode);
    }

    [Test]
    public async Task ReturnTooManyRequestIfReceivesNewRequestsWithin30SecondsAfterReject()
    {
        await using var app = new CreditLineApplication();
        var httpContext = new DefaultHttpContext();

        httpContext.Request.RouteValues.Add("id", 1);

        var config = (IConfiguration)app.Services.GetService(typeof(IConfiguration));
        var memcache = (IMemoryCache)app.Services.GetService(typeof(IMemoryCache));
        var repository = new ApplicationRepository();
        var applicationResult = new ApplicationResult { approved = false, resultCreditLine = 0, resultDate = DateTime.Now };

        repository.Add(1, applicationResult);

        memcache.Set("lastTime", DateTime.Now);
        memcache.Set("reqCount", 1);

        var middleware = new RateLimitMiddleware(config, memcache, repository, next: (innerHttpContext) => Task.FromResult(0));

        await middleware.InvokeAsync(httpContext);

        Assert.AreEqual((int)HttpStatusCode.TooManyRequests, httpContext.Response.StatusCode);
    }

    [Test]
    public async Task ReturnContactSalesMessageAfter3Fails()
    {
        await using var app = new CreditLineApplication();
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        httpContext.Request.RouteValues.Add("id", 1);

        var config = (IConfiguration)app.Services.GetService(typeof(IConfiguration));
        var memcache = (IMemoryCache)app.Services.GetService(typeof(IMemoryCache));
        var repository = new ApplicationRepository();
        var applicationResult = new ApplicationResult { approved = false, resultCreditLine = 0, resultDate = DateTime.Now };

        repository.Add(1, applicationResult);

        memcache.Set("lastTime", DateTime.Now.Subtract(new TimeSpan(0,0,40)));
        memcache.Set("reqCount", 3);

        var middleware = new RateLimitMiddleware(config, memcache, repository, next: (innerHttpContext) => Task.FromResult(0));

        await middleware.InvokeAsync(httpContext);
        
        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(httpContext.Response.Body);
        var streamText = reader.ReadToEnd();

        var expected = JsonSerializer.Serialize(new ContactSalesApplicationResponse());
        Assert.AreEqual(expected, streamText);
    }

}