using System.Text.Json;
using Weather_API.Services.ExternalApi;

var builder = WebApplication.CreateBuilder(args);

// var externalApiSection = builder.Configuration.GetSection("ExternalApi");
// var weatherApiSection = externalApiSection.GetSection("WeatherApi");
// var secret = builder.Configuration["Secret"];
// Console.WriteLine("Key: " + weatherApiSection["Key"]);
// Console.WriteLine("BaseUrl: " + weatherApiSection["BaseUrl"]);
// Console.WriteLine("Secret... " + secret);

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IWeatherApiClient, VisualCrossingApiClient>();
builder.Services.AddLogging();
var app = builder.Build();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
