using Microsoft.AspNetCore.Mvc;

namespace Lecture14.Presentation.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public sealed class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Lecture 14 API is running.",
            endpoints = new[]
            {
                "GET /api/users/{userId}/expenses",
                "GET /api/users/{userId}/expenses/{expenseId}",
                "POST /api/users/{userId}/expenses"
            }
        });
    }
}