using FluentValidation;
using ForeignCard.Model.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static ForeignCard.Model.Customers.CreateCustomerModel;

namespace ForeignCard.Api.Controllers;

#region api/customers
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateCustomerModel.CustomerCreateRequest> _createValidator;
    private readonly IValidator<UpdateCustomerModel.CustomerUpdateRequest> _updateValidator;
    private readonly IValidator<DeleteCustomerModel.CustomerDeleteRequest> _deleteValidator;

    public CustomersController(
        IMediator mediator,
        IValidator<CreateCustomerModel.CustomerCreateRequest> createValidator,
        IValidator<UpdateCustomerModel.CustomerUpdateRequest> updateValidator,
        IValidator<DeleteCustomerModel.CustomerDeleteRequest> deleteValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _deleteValidator = deleteValidator;
    }

    #region GET
    [Authorize(Roles = "ADMIN")]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> Get(int customerId)
    {
        var customer = await _mediator.Send(new GetCustomerInfoModel.CustomerGetRequest { CustomerId = customerId });
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }
    #endregion

    #region POST
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerModel.CustomerCreateRequest command)
    {
        var result = await _createValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Created($"api/customers/{response.Id}", response);
    }
    #endregion

    #region PUT
    [Authorize(Roles = "ADMIN")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerModel.CustomerUpdateRequest command)
    {
        var result = await _updateValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region DELETE
    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> Delete(int customerId)
    {
        var command = new DeleteCustomerModel.CustomerDeleteRequest { CustomerId = customerId };

        var result = await _deleteValidator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion
}
#endregion