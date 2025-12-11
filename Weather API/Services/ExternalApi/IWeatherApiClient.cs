using Weather_API.Model.ResponseModel;

namespace Weather_API.Services.ExternalApi;

public interface IWeatherApiClient
{
    Task<WeatherResponse?> GetWeatherNoDate(string location);
    Task<WeatherResponse?> GetWeatherWithOneDate(string location, string date);
    Task<WeatherResponse?> GetWeatherWithTwoDates(string location, string date1, string date2);
}