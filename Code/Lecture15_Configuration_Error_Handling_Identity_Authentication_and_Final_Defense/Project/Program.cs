using Lecture15.Application;
using Lecture15.Infrastructure;
using Lecture15.Presentation.Controllers;
using Lecture15.Project;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(ExpensesController).Assembly);

builder.Services
    .AddAuthentication(DemoAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>(
        DemoAuthenticationHandler.SchemeName,
        _ => { });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();

var app = builder.Build();

app.UseExceptionHandler(errorApplication =>
{
    errorApplication.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var status = exception is BusinessRuleException
            ? StatusCodes.Status400BadRequest
            : StatusCodes.Status500InternalServerError;

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new
        {
            title = status == StatusCodes.Status400BadRequest
                ? "Business rule failed"
                : "Unexpected server error",
            status
        });
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();