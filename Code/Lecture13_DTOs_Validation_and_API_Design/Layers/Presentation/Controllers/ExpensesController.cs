using Lecture13.Application;
using Lecture13.Presentation.Dtos;
using Lecture13.Presentation.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Presentation.Controllers;

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
    [ProducesResponseType(typeof(IReadOnlyList<ExpenseResponseDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ExpenseResponseDto>> GetAll(int userId)
    {
        var response = expenseService
            .GetForUser(userId)
            .Select(ExpenseDtoMapper.FromEntity)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{expenseId:int}")]
    [ProducesResponseType(typeof(ExpenseResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ExpenseResponseDto> GetById(int userId, int expenseId)
    {
        var expense = expenseService.Get(userId, expenseId);

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(ExpenseDtoMapper.FromEntity(expense));
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ExpenseResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ExpenseResponseDto> Create(
        int userId,
        [FromBody] CreateExpenseRequestDto request)
    {
        try
        {
            var expense = expenseService.Create(userId, request.Name, request.Amount, request.Category);
            var response = ExpenseDtoMapper.FromEntity(expense);

            return CreatedAtAction(nameof(GetById), new { userId, expenseId = expense.Id }, response);
        }
        catch (ValidationException exception)
        {
            ModelState.AddModelError(exception.Key, exception.Message);
            return ValidationProblem(ModelState);
        }
    }
}