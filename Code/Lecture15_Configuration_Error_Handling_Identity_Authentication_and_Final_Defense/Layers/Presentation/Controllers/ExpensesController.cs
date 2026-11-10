using System.Security.Claims;
using Lecture15.Application;
using Lecture15.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lecture15.Presentation.Controllers;

[ApiController]
[Route("api/me/expenses")]
[Authorize]
[Produces("application/json")]
public sealed class ExpensesController : ControllerBase
{
    private readonly ExpenseService expenseService;

    public ExpensesController(ExpenseService expenseService)
    {
        this.expenseService = expenseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Expense>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<Expense>>> GetMine(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var expenses = await expenseService.GetMineAsync(userId ?? string.Empty, cancellationToken);

        return Ok(expenses);
    }
}