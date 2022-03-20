using Microsoft.AspNetCore.Mvc;
using tribal_credit_line_application.Middleware;
using tribal_credit_line_application.Model;
using tribal_credit_line_application.Services;
using tribal_credit_line_application.Util;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies();

var app = builder.Build();

app.UseMiddleware<RateLimitMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/customer/{id}/credit/application", (ApplicationService applicationService, int id, [FromBody] CreditLine creditLine) =>
{
    var foundingTypes = new List<string> { FoundingType.SME, FoundingType.Startup };
    
    if (!foundingTypes.Contains(creditLine.foundingType) || creditLine.cashBalance < 1 || creditLine.monthlyRevenue < 1 || creditLine.requestedCreditLine < 1)
        return Results.BadRequest();

    return Results.Ok(applicationService.CalculateCreditLine(id, creditLine));
});

app.Run();

