using Lecture14.Application;
using Lecture14.Presentation.Contracts;
using Lecture14.Presentation.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lecture14.Presentation.Controllers;

[ApiController]
[Route("api/users/{userId:int}/expenses")]
[Produces("application/json")]
public sealed class ExpensesController : ControllerBase
{
    private readonly ExpenseService expenseService;

    public ExpensesController(ExpenseService expenseService)
    {
        this.expenseService = expenseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExpenseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ExpenseResponse>>> GetAll(
        int userId,
        CancellationToken cancellationToken)
    {
        var expenses = await expenseService.GetMineAsync(userId, cancellationToken);
        var response = expenses
            .Select(ExpenseResponseMapper.FromEntity)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{expenseId:int}")]
    [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseResponse>> GetById(
        int userId,
        int expenseId,
        CancellationToken cancellationToken)
    {
        var expense = await expenseService.GetMineByIdAsync(userId, expenseId, cancellationToken);

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(ExpenseResponseMapper.FromEntity(expense));
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExpenseResponse>> Create(
        int userId,
        [FromBody] CreateExpenseRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var expense = await expenseService.CreateAsync(
                userId,
                request.Name,
                request.Amount,
                request.Category,
                cancellationToken);

            var response = ExpenseResponseMapper.FromEntity(expense);
            return CreatedAtAction(nameof(GetById), new { userId, expenseId = expense.Id }, response);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError("expense", exception.Message);
            return ValidationProblem(ModelState);
        }
    }
}