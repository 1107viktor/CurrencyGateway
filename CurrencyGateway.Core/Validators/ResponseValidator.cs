using CurrencyGateway.Core.Model.Responses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CurrencyGateway.Core.Validators
{
    public class ResponseValidator : AbstractValidator<CbrResponse>
    {
        public ResponseValidator()
        {
            RuleFor(x => x.DateString)
                .NotEmpty()
                .Must(BeValidDate)
                .WithMessage("Некорректный формат даты");

            RuleForEach(x => x.Currencies)
                .SetValidator(new CurrencyValidator());
        }

        private bool BeValidDate(string dateString)
        {
            return DateTime.TryParseExact(
                dateString,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _
            );
        }
    }
}
