using System.Security.Claims;
using FluentValidation;
using ForeignCard.Model.Accounts;
using ForeignCard.Model.Cards;
using ForeignCard.Model.Customers;
using ForeignCard.Model.Transactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForeignCard.Api.Controllers;

#region api/person
// Giriş yapan kullanıcının kendi verileri. Id istekten değil, token'dan alınır.
[ApiController]
[Route("api/person")]
[Authorize(Roles = "CUSTOMER, ADMIN")]
public class PersonController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<UpdateCustomerModel.CustomerUpdateRequest> _updateValidator;

    public PersonController(IMediator mediator, IValidator<UpdateCustomerModel.CustomerUpdateRequest> updateValidator)
    {
        _mediator = mediator;
        _updateValidator = updateValidator;
    }

    // Token'daki müşteri Id'si
    private int? CurrentCustomerId()
    {
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("nameid")?.Value;

        return int.TryParse(value, out var id) ? id : null;
    }

    #region GET person
    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var id = CurrentCustomerId();
        if (id == null) return Unauthorized();

        var customer = await _mediator.Send(new GetCustomerInfoModel.CustomerGetRequest { CustomerId = id.Value });
        if (customer == null) return NotFound();

        return Ok(customer);
    }
    #endregion

    #region PUT person
    [HttpPut]
    public async Task<IActionResult> UpdateMe(UpdatePersonModel.UpdatePersonRequest request)
    {
        var id = CurrentCustomerId();
        if (id == null) return Unauthorized();

        // Müşteri sadece telefon ve e-postasını değiştirebilir
        var command = new UpdateCustomerModel.CustomerUpdateRequest
        {
            CustomerId = id.Value,
            PhoneNo = request.PhoneNo,
            Email = request.Email
        };

        var result = await _updateValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region GET person/accounts
    [HttpGet("accounts")]
    public async Task<IActionResult> GetMyAccounts()
    {
        var id = CurrentCustomerId();
        if (id == null) return Unauthorized();

        var response = await _mediator.Send(new GetCustomerAccountModel.GetCustomerAccountRequest { CustomerId = id.Value });
        return Ok(response);
    }
    #endregion

    #region GET person/cards
    [HttpGet("cards")]
    public async Task<IActionResult> GetMyCards()
    {
        var id = CurrentCustomerId();
        if (id == null) return Unauthorized();

        var response = await _mediator.Send(new GetCustomerCardModel.GetCustomerCardRequest { CustomerId = id.Value });
        return Ok(response);
    }
    #endregion

    #region GET person/statement
    // Tüm hesapların ekstresi; istenirse ?accountNo= ile tek hesaba daraltılır
    [HttpGet("statement")]
    public async Task<IActionResult> GetMyStatement([FromQuery] string? accountNo = null)
    {
        var id = CurrentCustomerId();
        if (id == null) return Unauthorized();

        var myAccounts = await _mediator.Send(new GetCustomerAccountModel.GetCustomerAccountRequest { CustomerId = id.Value });

        var selected = myAccounts.Accounts.Where(a => string.IsNullOrWhiteSpace(accountNo) || a.AccountNo == accountNo.Trim()).ToList();

        if (!string.IsNullOrWhiteSpace(accountNo) && selected.Count == 0)
            return NotFound();

        var result = new List<object>();
        foreach (var account in selected)
        {
            var statement = await _mediator.Send(new GetAccountStatementModel.GetAccountStatementRequest { AccountId = account.AccountId });
            result.Add(new
            {
                account.AccountNo,
                account.CurrencyCode,
                Statement = statement
            });
        }

        return Ok(result);
    }
    #endregion

    #region GET person/transactions
    [HttpGet("transactions")]
    public async Task<IActionResult> GetMyTransactions([FromQuery] int top = 50)
    {
        var id = CurrentCustomerId();
        if (id == null) return Unauthorized();

        var response = await _mediator.Send(new GetCustomerTransactions.GetCustomerTransactionsRequest
        {
            CustomerId = id.Value,
            Top = top
        });
        return Ok(response);
    }
    #endregion
}
#endregion