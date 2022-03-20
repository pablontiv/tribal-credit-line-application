using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using tribal_credit_line_application.Model;
using tribal_credit_line_application.Services;
using tribal_credit_line_application.Util;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}

app.MapPost("/customer/{id}/credit/application", ([FromServices] ApplicationService applicationService, int id, [FromBody] CreditLine creditLine) =>
{
    return Results.Ok(applicationService.CalculateCreditLine(id, creditLine));
});

app.Run();

