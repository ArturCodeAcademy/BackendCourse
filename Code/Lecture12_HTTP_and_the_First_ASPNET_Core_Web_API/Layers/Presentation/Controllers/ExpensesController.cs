using Lecture12.Application;
using Lecture12.Domain;
using Lecture12.Presentation.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lecture12.Presentation.Controllers;

[ApiController]
[Route("api/expenses")]
[Produces("application/json")]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseService expenseService;

    public ExpensesController(ExpenseService expenseService)
    {
        this.expenseService = expenseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Expense>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Expense>> GetAll()
    {
        return Ok(expenseService.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Expense> GetById(int id)
    {
        var expense = expenseService.Get(id);

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(expense);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Expense), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<Expense> Create([FromBody] CreateExpenseRequest request)
    {
        try
        {
            var expense = expenseService.Add(request.Name, request.Amount, request.Category);

            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError("expense", exception.Message);
            return ValidationProblem(ModelState);
        }
    }
}