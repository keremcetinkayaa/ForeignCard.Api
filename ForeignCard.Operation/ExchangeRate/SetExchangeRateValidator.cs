using FluentValidation;
using ForeignCard.Model.ExchangeRates;

namespace ForeignCard.Operation.ExchangeRates;

public class SetExchangeRateValidator : AbstractValidator<SetExchangeRateModel.SetExchangeRateRequest>
{
    public SetExchangeRateValidator()
    {
        RuleFor(x => x.CurrencyCode).NotEmpty().Length(3);
        RuleFor(x => x.Rate).GreaterThan(0);
    }
}