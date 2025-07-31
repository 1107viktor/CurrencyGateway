using CurrencyGateway.Core.Model.Responses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CurrencyGateway.Core.Validators
{
    public class CurrencyValidator : AbstractValidator<Currency>
    {
        public CurrencyValidator()
        {
            RuleFor(x => x.CurrencyCode)
                .NotEmpty()
                .Length(3)
                .WithMessage("Некорректный код валюты");

            RuleFor(x => x.CurrencyName)
                .NotEmpty()
                .WithMessage("Название валюты обязательно");

            RuleFor(x => x.Nominal)
                .GreaterThan(0)
                .WithMessage("Номинал должен быть больше 0");

            RuleFor(x => x.ValueString)
                .NotEmpty()
                .Must(BeValidDecimal)
                .WithMessage("Некорректный формат суммы");
        }

        private bool BeValidDecimal(string value)
        {
            return decimal.TryParse(
                value.Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out _
            );
        }
    }
}
