using Microsoft.AspNetCore.Mvc;
using Weather_API.Model.ResponseModel;
using Weather_API.Services.ExternalApi;

namespace Weather_API.Controllers;
[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherApiClient client, ILogger<WeatherController> logger) : ControllerBase
{
    private IWeatherApiClient _weatherApiClient = client;
    private ILogger<WeatherController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<WeatherResponse>> GetWeather([FromQuery] string location)
    {
        var response = await _weatherApiClient.GetWeatherNoDate(location);
        if (response == null)
        {
            _logger.LogInformation("NO DATE CONTROLLER: response is null");
            return BadRequest();
        }
        _logger.LogInformation("NO DATE CONTROLLER: response is with status code 200");
        return Ok(response);
    }

    [HttpGet("{date}")]
    public async Task<ActionResult<WeatherResponse>> GetWeatherOneDate([FromQuery] string location, string date)
    {
        var response = await _weatherApiClient.GetWeatherWithOneDate(location, date);
        if (response == null)
        {
            _logger.LogInformation("ONE DATE CONTROLLER: response is null");
            return BadRequest();
        }
        _logger.LogInformation("ONE DATE CONTROLLER: response is with status code 200");
        return Ok(response);
    }


    [HttpGet("{date1}/{date2}")]
    public async Task<ActionResult<WeatherResponse>> GetWeatherDates([FromQuery] string location, string date1, string date2)
    {
        var response = await _weatherApiClient.GetWeatherWithTwoDates(location, date1, date2);
        if (response == null)
        {
            _logger.LogInformation("TWO DATES CONTROLLER: response is null");
            return BadRequest();
        }
        _logger.LogInformation("TWO DATES CONTROLLER: response is with status code 200");
        return Ok(response);
    }
    
}