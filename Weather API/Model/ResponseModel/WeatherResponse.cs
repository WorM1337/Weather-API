using System.Text.Json.Serialization;

namespace Weather_API.Model.ResponseModel;

public class WeatherResponse
{
    public string Address { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("timezone")]
    public string TimeZone { get; set; }
    public double TzOffset { get; set; }
    public List<Day> Days { get; set; }
}

public class Day
{
    [JsonPropertyName("datetime")]
    public DateTime Date { get; set; }
    [JsonPropertyName("temp")]
    public double Temperature { get; set; } 
    [JsonPropertyName("feelslike")]
    public double FeelsLike { get; set; }
    public double Humidity { get; set; }
    public double Pressure { get; set; }
    [JsonPropertyName("windspeed")]
    public double WindSpeed { get; set; }
    [JsonPropertyName("sunrise")]
    public TimeSpan SunRise { get; set; }
    [JsonPropertyName("sunset")]
    public TimeSpan SunSet { get; set; }
    public string Conditions { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Hour> Hours { get; set; } = new List<Hour>();
}

public class Hour
{
    [JsonPropertyName("datetime")]
    public string DateTime { get; set; } = "";
    [JsonPropertyName("feelslike")]
    public double FeelsLike { get; set; }
    [JsonPropertyName("windspeed")]
    public double WindSpeed { get; set; }
    public double Pressure { get; set; }
    public double Humidity { get; set; }
    public string Conditions { get; set; } = "";
}