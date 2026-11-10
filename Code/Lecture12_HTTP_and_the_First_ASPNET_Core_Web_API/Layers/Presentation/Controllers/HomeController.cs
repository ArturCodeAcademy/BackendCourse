using Microsoft.AspNetCore.Mvc;

namespace Lecture12.Presentation.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Redirect("/swagger");
    }
}