using FluentValidation;
using ForeignCard.Model.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static ForeignCard.Model.Transactions.CreateTransaction;

namespace ForeignCard.Api.Controllers;

#region api/transactions
[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateTransaction.CreateTransactionRequest> _createValidator;
    private readonly IValidator<SimulateTransactions.SimulateTransactionsRequest> _simulateValidator;

    public TransactionsController(
        IMediator mediator,
        IValidator<CreateTransaction.CreateTransactionRequest> createValidator,
        IValidator<SimulateTransactions.SimulateTransactionsRequest> simulateValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
        _simulateValidator = simulateValidator;
    }

    #region POST
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransaction.CreateTransactionRequest command)
    {
        var result = await _createValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        try
        {
            var response = await _mediator.Send(command);
            return Created($"api/transactions/{response.TransactionId}", response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
    #endregion

    #region POST Simulate
    [HttpPost("simulate")]
    [Authorize(Roles = "ADMIN, CUSTOMER")]
    public async Task<IActionResult> Simulate(SimulateTransactions.SimulateTransactionsRequest command)
    {
        var result = await _simulateValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region GET
    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> List([FromQuery] int top = 50)
    {
        var response = await _mediator.Send(new GetTransactions.GetTransactionsRequest { Top = top });
        return Ok(response);
    }
    #endregion

    #region GET by Customer
    [Authorize(Roles = "ADMIN,CUSTOMER")]
    [HttpGet("Customer")]
    public async Task<IActionResult> My([FromQuery] int top = 50)
    {
        var idValue = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst("nameid")?.Value;

        if (!int.TryParse(idValue, out var tokenCustomerId))
            return Unauthorized();

        var response = await _mediator.Send(new GetCustomerTransactions.GetCustomerTransactionsRequest
        {
            CustomerId = tokenCustomerId,
            Top = top
        });

        return Ok(response);
    }
    #endregion
}
#endregion