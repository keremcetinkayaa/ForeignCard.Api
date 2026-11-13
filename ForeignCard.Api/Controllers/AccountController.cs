using FluentValidation;
using ForeignCard.Model.Accounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static ForeignCard.Model.Accounts.UpdateAccountModel;

namespace ForeignCard.Api.Controllers;

#region api/accounts
[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateAccountModel.AccountCreateRequest> _createValidator;
    private readonly IValidator<UpdateAccountModel.AccountUpdateRequest> _updateValidator;

    public AccountsController(
        IMediator mediator,
        IValidator<CreateAccountModel.AccountCreateRequest> createValidator,
        IValidator<UpdateAccountModel.AccountUpdateRequest> updateValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    #region POST
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountModel.AccountCreateRequest command)
    {
        var result = await _createValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Created($"api/accounts/{response.Id}", response);
    }
    #endregion

    #region PUT
    [Authorize(Roles = "ADMIN")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateAccountModel.AccountUpdateRequest command)
    {
        var result = await _updateValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region GET
    [Authorize(Roles = "ADMIN")]
    [HttpGet("{accountId}")]
    public async Task<IActionResult> Get(int accountId)
    {
        var account = await _mediator.Send(new GetAccountInfoModel.GetAccountInfoRequest { AccountId = accountId });
        if (account == null)
            return NotFound();

        return Ok(account);
    }
    #endregion

    #region GET Statement
    [Authorize(Roles = "ADMIN")]
    [HttpGet("{accountId}/statement")]
    public async Task<IActionResult> Statement(int accountId)
    {
        var statement = await _mediator.Send(new GetAccountStatementModel.GetAccountStatementRequest { AccountId = accountId });
        if (statement == null)
            return NotFound();

        return Ok(statement);
    }
    #endregion

    #region POST Deposit
    [Authorize(Roles = "ADMIN")]
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        DepositAccountModel.DepositAccountRequest request,
        [FromServices] FluentValidation.IValidator<DepositAccountModel.DepositAccountRequest> validator)
    {
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        try
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
    #endregion
}
#endregion