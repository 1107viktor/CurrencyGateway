using CurrencyGateway.Core.Model;
using CurrencyGateway.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyGateway.API.Endpoints
{
    public static class CurrencyEndpoints
    {
        public static void MapCurrencyEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/currency", HandleGetCurrencyRates)
                .Produces<List<CurrencyRate>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("GetCurrencyRates")
                .WithTags("Currency")
                .WithOpenApi(operation =>
                {
                    operation.Summary = "Получение курсов валют";
                    operation.Description = "Возвращает курсы валют ЦБ РФ на указанную дату";
                    return operation;
                });
        }

        private static async Task<IResult> HandleGetCurrencyRates(
            [FromQuery] DateTime? date,
            [FromQuery] string? code,
            ICurrencyService service)
        {
            if (date?.Date > DateTime.Today)
                return Results.BadRequest("Дата не может быть в будущем");

            // Валидация кода валюты
            if (!string.IsNullOrEmpty(code) && code.Length != 3)
                return Results.BadRequest("Код валюты должен содержать 3 символа");

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    var rates = await service.GetRatesAsync(date);
                    return rates.Count > 0 ? Results.Ok(rates) : Results.NoContent();
                }

                var rate = await service.GetRateByCodeAsync(date, code);
                return rate != null ? Results.Ok(rate) : Results.NoContent();
            }
            catch (ApplicationException ex)
            {
                return Results.Problem(
                    title: "Ошибка при получении данных",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch
            {
                return Results.Problem(
                    title: "Внутренняя ошибка сервера",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
