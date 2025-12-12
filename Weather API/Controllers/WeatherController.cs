using Microsoft.AspNetCore.Mvc;
using Weather_API.Model.Errors;
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
    public async Task<ActionResult<WeatherResponse>> GetWeather([FromQuery] string location, [FromQuery] string? unitGroup = null)
    {
        try
        {
            var response = await _weatherApiClient.GetWeatherNoDate(location, unitGroup);
            if (response == null)
            {
                _logger.LogInformation("NO DATE CONTROLLER: response is null");
                return NotFound();
            }
            _logger.LogInformation("NO DATE CONTROLLER: response is with status code 200");
            return Ok(response);
        }
        catch (ExternalApiRequestException e)
        {
            return Problem(title: "Error", statusCode: (int)e.StatusCode, detail: e.Message);            
        }
        
    }

    [HttpGet("{date}")]
    public async Task<ActionResult<WeatherResponse>> GetWeatherOneDate([FromQuery] string location,  string date, [FromQuery] string? unitGroup = null)
    {
        try
        {
            var response = await _weatherApiClient.GetWeatherWithOneDate(location, date, unitGroup);
            if (response == null)
            {
                _logger.LogInformation("ONE DATE CONTROLLER: response is null");
                return NotFound();
            }
            _logger.LogInformation("ONE DATE CONTROLLER: response is with status code 200");
            return Ok(response);
        }
        catch (ExternalApiRequestException e)
        {
            return Problem(title: "Error", statusCode: (int)e.StatusCode, detail: e.Message);            
        }
    }


    [HttpGet("{date1}/{date2}")]
    public async Task<ActionResult<WeatherResponse>> GetWeatherDates([FromQuery] string location,  string date1, string date2, [FromQuery] string? unitGroup = null)
    {
        try
        {
            var response = await _weatherApiClient.GetWeatherWithTwoDates(location, date1, date2, unitGroup);
            if (response == null)
            {
                _logger.LogInformation("TWO DATES CONTROLLER: response is null");
                return NotFound();
            }
            _logger.LogInformation("TWO DATES CONTROLLER: response is with status code 200");
            return Ok(response);
        }
        catch (ExternalApiRequestException e)
        {
            return Problem(title: "Error", statusCode: (int)e.StatusCode, detail: e.Message);            
        }
    }
    
}