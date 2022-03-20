using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using tribal.credit.line.application.tests;
using tribal_credit_line_application.Model;
using tribal_credit_line_application.Model.Response;

namespace credit.line.application.tests;

public class Tests
{
    [Test]
    public async Task GetCreditForSME()
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
}