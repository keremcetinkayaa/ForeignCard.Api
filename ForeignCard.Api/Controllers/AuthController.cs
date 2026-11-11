using FluentValidation;
using ForeignCard.Model.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForeignCard.Api.Controllers;

#region api/auth
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<LoginModel.LoginRequest> _validator;

    public AuthController(IMediator mediator, IValidator<LoginModel.LoginRequest> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    #region POST
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel.LoginRequest request)
    {
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(request);
        if (response == null)
            return Unauthorized(new { error = "TC kimlik no veya sifre hatali" });

        return Ok(response);
    }
    #endregion
}
#endregion