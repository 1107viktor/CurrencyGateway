using CurrencyGateway.Core.Model;
using CurrencyGateway.Core.Model.Responses;
using CurrencyGateway.Core.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyGateway.Services.Services
{
    public sealed class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly CbrApiOptions _apiOptions;

        // в более новой версии C# вынес бы всё это в первичный конструктор
        public CurrencyService(HttpClient httpClient, CbrApiOptions apiOptions)
        {
            _httpClient = httpClient;
            _apiOptions = apiOptions;
        }

        public async Task<IReadOnlyList<CurrencyRate>> GetRatesAsync(DateTime? date)
        {
            var response = await GetCbrResponseAsync(date);
            var result = response.Currencies
                .Select(currency => new CurrencyRate
                {
                    CurrencyCode = currency.CurrencyCode,
                    CurrencyName = currency.CurrencyName,
                    Nominal = currency.Nominal,
                    Value = currency.Value,
                    Date = response.Date
                })
                .ToList();

            return result;
        }

        public async Task<CurrencyRate?> GetRateByCodeAsync(DateTime? date, string currencyCode)
        {
            var response = await GetCbrResponseAsync(date);
            var currency = response.Currencies
                .FirstOrDefault(c => c.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));

            var result = currency != null ? new CurrencyRate
            {
                CurrencyCode = currency.CurrencyCode,
                CurrencyName = currency.CurrencyName,
                Nominal = currency.Nominal,
                Value = currency.Value,
                Date = response.Date
            } : null;

            return result;
        }

        private async Task<CbrResponse> GetCbrResponseAsync(DateTime? date)
        {
            try
            {
                var formattedDate = (date ?? DateTime.Today).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var endpoint = $"{_apiOptions.Endpoint}?date_req={formattedDate}";

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var byteArray = await response.Content.ReadAsByteArrayAsync();
                var encoding = Encoding.GetEncoding("windows-1251");
                var content = encoding.GetString(byteArray);

                var serializer = new XmlSerializer(typeof(CbrResponse));
                using var reader = new StringReader(content);

                return (CbrResponse)serializer.Deserialize(reader);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Ошибка при запросе к ЦБ РФ: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ошибка обработки данных от ЦБ РФ", ex);
            }
        }
    }
}
