using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Presentation.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public sealed class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Redirect("/swagger");
    }
}