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
    public async Task<WeatherResponse?> GetWeatherNoDate(string location)
    {
        try
        {
            var response = await _client.GetAsync($"{location}?key={_apiKey}");
        
            response.EnsureSuccessStatusCode();
            
            _logger.LogInformation("NO DATE: request returned with 200");
            
            var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            _logger.LogInformation("NO DATE: parse response with 200");
            return data;
        }
        catch (Exception e)
        {
            _logger.LogError("VisualCrossingApiClient.GetWeatherNoDate", "Incorrent location");
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherWithOneDate(string location, string date)
    {
        try
        {
            var response = await _client.GetAsync($"{location}/{date}?key={_apiKey}");
        
            response.EnsureSuccessStatusCode();
            
            _logger.LogInformation("ONE DATE: request returned with 200");
            
            var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            _logger.LogInformation("ONE DATE: parse response with 200");
            return data;
        }
        catch (Exception e)
        {
            _logger.LogError("VisualCrossingApiClient.GetWeatherOneDate", "Incorrent location or date");
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherWithTwoDates(string location, string date1, string date2)
    {
        try
        {
            var response = await _client.GetAsync($"{location}/{date1}/{date2}?key={_apiKey}");
        
            response.EnsureSuccessStatusCode();
            
            _logger.LogInformation("TWO DATES: request returned with 200");
            
            var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            _logger.LogInformation("TWO DATES: parse response with 200");
            return data;
        }
        catch (Exception e)
        {
            _logger.LogError("VisualCrossingApiClient.GetWeatherTwoDate", "Incorrent location or date");
            return null;
        }
    }
}