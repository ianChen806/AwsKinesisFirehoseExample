using KinesisFirehose.Models;
using Microsoft.Extensions.Logging;

namespace KinesisFirehose.Services;

public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(ILogger<WeatherForecastService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<WeatherForecast> GetForecast()
    {
        _logger.LogInformation("Getting weather forecast");

        var weatherForecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            )).ToList();
        _logger.LogInformation("get count {Count}", weatherForecasts.Count);
        return weatherForecasts;
    }

    public Task ExceptionTest()
    {
        try
        {
            // do something
            throw new Exception("test exception");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "get exception");
            throw;
        }
    }
}
