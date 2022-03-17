using Microsoft.AspNetCore.Mvc;
using tribal_credit_line_application.Model;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/customer/credit/application", ([FromBody] CreditLine creditLine) =>
{
    return new ApplicationResponse { status = "OK", message = "Your application was succesfully accepted.", approvedCreditLine = 13213 };   
});

app.Run();

