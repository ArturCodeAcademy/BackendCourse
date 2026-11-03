using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Lecture15.Application;
using Lecture15.Infrastructure;
using Lecture15.Presentation;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(DemoAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>(DemoAuthenticationHandler.SchemeName, _ => { });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    Exception? error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
    int status = error is BusinessRuleException
        ? StatusCodes.Status400BadRequest
        : StatusCodes.Status500InternalServerError;

    context.Response.StatusCode = status;
    await context.Response.WriteAsJsonAsync(new
    {
        title = status == StatusCodes.Status400BadRequest ? "Business rule failed" : "Unexpected server error",
        status
    });
}));

app.UseAuthentication();
app.UseAuthorization();

string applicationName = builder.Configuration["Demo:ApplicationName"] ?? "Expense API";
app.MapGet("/health", () => Results.Ok(new { status = "ok", applicationName }));
app.MapApiEndpoints();
app.Run();
