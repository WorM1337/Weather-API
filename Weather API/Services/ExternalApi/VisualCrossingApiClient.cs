using System.Net;
using Weather_API.Model.Errors;
using Weather_API.Model.ResponseModel;

namespace Weather_API.Services.ExternalApi;

public class VisualCrossingApiClient : IWeatherApiClient
{
    private readonly HttpClient _client; 
    private readonly IConfiguration  _configuration;
    private readonly ILogger<VisualCrossingApiClient> _logger;
    private readonly string _apiKey;
    
    public VisualCrossingApiClient(
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration, 
        ILogger<VisualCrossingApiClient> logger)
    {
        _client = httpClientFactory.CreateClient();
        _configuration = configuration;
        _logger = logger;
        
        var baseUrl = configuration["ExternalApi:WeatherApi:BaseUrl"];
        var apiKey = configuration["ExternalApi:WeatherApi:Key"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentNullException("ExternalApi:WeatherApi:BaseUrl", "ExternalApi:WeatherApi:BaseUrl is missing");
        }
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentNullException("ExternalApi:WeatherApi:Key", "ExternalApi:WeatherApi:Key is missing");
        }

        _apiKey = apiKey;
        _client.BaseAddress = new Uri(baseUrl);
    }
    
    public async Task<WeatherResponse?> GetWeatherNoDate(string location, string? unitGroup = null)
    {
        var response = await _client.GetAsync($"{location}?key={_apiKey}" + (unitGroup != null ? "&unitGroup=" + unitGroup : ""));
            
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("NO DATE: request returned with 200");
            
            var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            _logger.LogInformation("NO DATE: parse response with 200");
            return data;
        }
        _logger.LogInformation($"NO DATE: request returned with {response.StatusCode}");
        throw new ExternalApiRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
    }

    public async Task<WeatherResponse?> GetWeatherWithOneDate(string location, string date, string? unitGroup = null)
    {
        var response = await _client.GetAsync($"{location}/{date}?key={_apiKey}" + (unitGroup != null ? "&unitGroup=" + unitGroup : ""));
            
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("ONE DATE: request returned with 200");
            
            var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            _logger.LogInformation("ONE DATE: parse response with 200");
            return data;
        }
        _logger.LogInformation($"ONE DATE: request returned with {response.StatusCode}");
        throw new ExternalApiRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
    }

    public async Task<WeatherResponse?> GetWeatherWithTwoDates(string location, string date1, string date2, string? unitGroup = null)
    {
        var response = await _client.GetAsync($"{location}/{date1}/{date2}?key={_apiKey}" + (unitGroup != null ? "&unitGroup=" + unitGroup : ""));
            
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("TWO DATES: request returned with 200");
            
            var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            _logger.LogInformation("TWO DATES: parse response with 200");
            return data;
        }
        _logger.LogInformation($"TWO DATES: request returned with {response.StatusCode}");
        throw new ExternalApiRequestException(await response.Content.ReadAsStringAsync(), response.StatusCode);
    }
}