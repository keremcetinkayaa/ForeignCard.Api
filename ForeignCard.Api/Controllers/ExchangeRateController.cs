using ForeignCard.Model.ExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ForeignCard.Api.Controllers;

#region api/exchange-rates
[ApiController]
[Route("api/exchange-rates")]
[Authorize]
public class ExchangeRatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangeRatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region GET
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var response = await _mediator.Send(new GetExchangeRateModel.GetExchangeRateRequest());
        return Ok(response);
    }
    #endregion
}
#endregion