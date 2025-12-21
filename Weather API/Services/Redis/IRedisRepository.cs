using Weather_API.Model.ResponseModel;

namespace Weather_API.Services.Redis;

public interface IRedisRepository
{
    Task<WeatherResponse?> GetWeatherAsync(string location, string? data1 = null, string? data2 = null, string? unitGroup = null);
    Task WriteWeatherAsync(WeatherResponse weather, string location, string? data1 = null, string? data2 = null, string? unitGroup = null, TimeSpan? ttl = null);
    Task RemoveWeatherAsync(string location, string? data1 = null, string? data2 = null, string? unitGroup = null);
    Task<bool> ExistsWeatherAsync(string location, string? data1 = null, string? data2 = null, string? unitGroup = null);
}