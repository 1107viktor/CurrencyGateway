using CurrencyGateway.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGateway.Services.Services
{
    public interface ICurrencyService
    {
        Task<IReadOnlyList<CurrencyRate>> GetRatesAsync(DateTime? date);
        Task<CurrencyRate?> GetRateByCodeAsync(DateTime? date, string currencyCode);
    }
}
