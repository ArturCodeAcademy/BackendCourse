using Microsoft.AspNetCore.Mvc;

namespace Lecture15.Presentation.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public sealed class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Lecture 15 API is running.",
            endpoints = new[]
            {
                "GET /health",
                "GET /api/me/expenses (requires X-Demo-User header)"
            }
        });
    }
}