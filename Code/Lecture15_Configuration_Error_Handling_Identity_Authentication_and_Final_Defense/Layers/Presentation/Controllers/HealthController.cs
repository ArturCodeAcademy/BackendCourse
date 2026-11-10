using Lecture15.Presentation.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Lecture15.Presentation.Controllers;

[ApiController]
[Route("health")]
[Produces("application/json")]
public sealed class HealthController : ControllerBase
{
    private readonly IConfiguration configuration;

    public HealthController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get()
    {
        var applicationName = configuration["Demo:ApplicationName"] ?? "Expense API";
        var response = new HealthResponse("ok", applicationName);

        return Ok(response);
    }
}