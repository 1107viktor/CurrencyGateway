using CurrencyGateway.Core.Options;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyGateway.Core.Validators
{
    public class ApiOptionsValidator : AbstractValidator<CbrApiOptions>
    {
        public ApiOptionsValidator()
        {
            RuleFor(x => x.BaseAddress)
                .NotEmpty()
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Некорректный базовый адрес");

            RuleFor(x => x.Endpoint)
                .NotEmpty()
                .Must(uri => Uri.TryCreate(uri, UriKind.Relative, out _))
                .WithMessage("Некорректный эндпоинт");
        }
    }
}
