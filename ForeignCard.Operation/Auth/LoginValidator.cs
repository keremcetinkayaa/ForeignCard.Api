using FluentValidation;
using ForeignCard.Model.Auth;

namespace ForeignCard.Operation.Auth;

public class LoginValidator : AbstractValidator<LoginModel.LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.NationalId).NotEmpty().Length(11).Matches("^[0-9]+$");
        RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
    }
}