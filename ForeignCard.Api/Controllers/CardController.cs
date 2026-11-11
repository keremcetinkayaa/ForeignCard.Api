using FluentValidation;
using ForeignCard.Model.Cards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ForeignCard.Api.Controllers;

#region api/cards
[ApiController]
[Route("api/cards")]
[Authorize(Roles = "ADMIN")]
public class CardsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateCardModel.CreateCardRequest> _createValidator;
    private readonly IValidator<UpdateCardModel.UpdateCardRequest> _updateValidator;

    public CardsController(
        IMediator mediator,
        IValidator<CreateCardModel.CreateCardRequest> createValidator,
        IValidator<UpdateCardModel.UpdateCardRequest> updateValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    #region POST
    [HttpPost]
    public async Task<IActionResult> Create(CreateCardModel.CreateCardRequest command)
    {
        var result = await _createValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Created($"api/cards/{response.Id}", response);
    }
    #endregion

    #region PUT
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCardModel.UpdateCardRequest command)
    {
        var result = await _updateValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion
}
#endregion