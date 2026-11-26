using FluentValidation;
using ForeignCard.Model.Transactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForeignCard.Api.Controllers;

#region api/transactions
// Sadece admin: işlem simülasyonu ve tüm işlemlerin listesi.
// Müşteri kendi işlemlerini /api/me/transactions ile görür.
[ApiController]
[Route("api/transactions")]
[Authorize(Roles = "ADMIN")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<SimulateTransactions.SimulateTransactionsRequest> _simulateValidator;

    public TransactionsController(
        IMediator mediator,
        IValidator<SimulateTransactions.SimulateTransactionsRequest> simulateValidator)
    {
        _mediator = mediator;
        _simulateValidator = simulateValidator;
    }

    #region POST Simulate
    [HttpPost("simulate")]
    [Authorize(Roles = "ADMIN")]
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
}
#endregion