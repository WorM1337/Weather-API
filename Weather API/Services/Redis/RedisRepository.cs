using System.Text;
using System.Text.Json;
using StackExchange.Redis;
using Weather_API.Model.ResponseModel;

namespace Weather_API.Services.Redis;

public class RedisRepository : IRedisRepository
{
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<RedisRepository> _logger;
    private readonly IConfiguration _configuration;
    private TimeSpan _weatherCacheDurationMinutes;

    public RedisRepository(
        IConnectionMultiplexer redis, 
        ILogger<RedisRepository> logger,
        IConfiguration configuration)
    {
        _database = redis.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger =  logger;
        _configuration = configuration;
        try
        {
            _weatherCacheDurationMinutes =
                TimeSpan.FromMinutes(int.Parse(_configuration["CacheSettings:WeatherCacheDurationMinutes"]));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _weatherCacheDurationMinutes = TimeSpan.FromMinutes(10);
        }
    }

    private string GetKey(string location, string? data1 = null, string? data2 = null, string? unitGroup = null)
    {
        var builder = new StringBuilder("weather:" + location);

        if (!string.IsNullOrEmpty(data1))
        {
            builder.Append($":{data1}");
            if (!string.IsNullOrEmpty(data2))
            {
                builder.Append($":{data2}");
            }
        }

        if (!string.IsNullOrEmpty(unitGroup))
        {
            builder.Append($":{unitGroup}");
        }
        return builder.ToString();
    }
    
    public async Task<WeatherResponse?> GetWeatherAsync(string location, string? data1 = null, string? data2 = null, string? unitGroup = null)
    {
        try
        {
            var key =  GetKey(location, data1, data2, unitGroup);

            if (!await _database.KeyExistsAsync(key)) return null;
        
            var result = await _database.StringGetAsync(key);
        
            return JsonSerializer.Deserialize<WeatherResponse>(result, _jsonOptions);
        }
        catch (Exception e)
        {
            _logger.LogError("CACHE: " + e.Message);
            return null;
        }
    }

    public async Task WriteWeatherAsync(WeatherResponse weather, string location, string? data1 = null, string? data2 = null,
        string? unitGroup = null, TimeSpan? ttl = null)
    {
        try
        {
            var key = GetKey(location, data1, data2, unitGroup);

            if (!await _database.KeyExistsAsync(key))
            {
                await _database.StringSetAsync(key, JsonSerializer.Serialize(weather, _jsonOptions), ttl ?? _weatherCacheDurationMinutes);
                _logger.LogInformation($"CACHE: {key} was added");
            }
            else
            {
                await _database.StringSetAsync(key, JsonSerializer.Serialize(weather, _jsonOptions), ttl ?? _weatherCacheDurationMinutes);
                _logger.LogInformation($"CACHE: {key} was replaced");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("CACHE: couldn't write data: " + e.Message);
        }
    }

    public async Task RemoveWeatherAsync(string location, string? data1 = null, string? data2 = null, string? unitGroup = null)
    {
        try
        {
            var key = GetKey(location, data1, data2, unitGroup);
            if (!await _database.KeyExistsAsync(key))
                throw new KeyNotFoundException($"Key: {key} not found");
            await _database.KeyDeleteAsync(key);
        }
        catch (Exception e)
        {
            _logger.LogError("CACHE: couldn't delete data: " + e.Message);
        }
    }

    public async Task<bool> ExistsWeatherAsync(string location, string? data1 = null, string? data2 = null, string? unitGroup = null)
    {
        try
        {
            var key = GetKey(location, data1, data2, unitGroup);
            return await _database.KeyExistsAsync(key);    
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogError("REDIS CONNECTION FAILED: {Message}", ex.Message);
            return false;
        }
    }
}