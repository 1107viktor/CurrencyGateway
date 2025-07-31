using CurrencyGateway.API.Endpoints;
using CurrencyGateway.Core.Options;
using CurrencyGateway.Services.Services;
using Microsoft.OpenApi.Models;
using System.Text;

// Регистрация провайдера кодировок
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Currency Gateway API",
        Version = "v1",
        Description = "Шлюз для получения курсов валют ЦБ РФ"
    });
});

var cbrOptions = builder.Configuration.GetSection("CbrApi").Get<CbrApiOptions>()!;


// Регистрируем конфигурацию как синглтон
builder.Services.AddSingleton(cbrOptions);

// IoC
// IHttpClientFactory
builder.Services.AddHttpClient<ICurrencyService, CurrencyService>(client =>
{
    client.BaseAddress = new Uri(cbrOptions.BaseAddress);
    client.Timeout = TimeSpan.FromSeconds(30);
});


var app = builder.Build();
var isDevelopment = app.Environment.IsDevelopment();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCurrencyEndpoints();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Произошла внутренняя ошибка сервера"
        });
    });
});

app.Run();
