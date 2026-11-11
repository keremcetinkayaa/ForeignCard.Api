using ForeignCard.Model.Countries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ForeignCard.Api.Controllers;

#region api/countries
[ApiController]
[Route("api/countries")]
[Authorize]
public class CountriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CountriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region GET
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var response = await _mediator.Send(new GetCountryModel.GetCountryRequest());
        return Ok(response);
    }
    #endregion
}
#endregion