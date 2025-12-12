using Weather_API.Model.ResponseModel;

namespace Weather_API.Services.ExternalApi;

public interface IWeatherApiClient
{
    Task<WeatherResponse?> GetWeatherNoDate(string location, string? unitGroup = null);
    Task<WeatherResponse?> GetWeatherWithOneDate(string location, string date, string? unitGroup = null);
    Task<WeatherResponse?> GetWeatherWithTwoDates(string location, string date1, string date2, string? unitGroup = null);
}