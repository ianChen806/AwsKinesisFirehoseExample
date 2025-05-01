using KinesisFirehose.Services;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: 設置 NLog 為日誌提供者
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    // 註冊 WeatherForecastService
    builder.Services.AddScoped<WeatherForecastService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapGet("/weatherforecast", (WeatherForecastService weatherService) =>
        {
            return weatherService.GetForecast();
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

    app.MapGet("/weatherforecast/exception", (WeatherForecastService weatherService) =>
        {
            return weatherService.ExceptionTest();
        })
        .WithName("exception")
        .WithOpenApi();

    app.Run();
}
catch (Exception exception)
{
    // NLog: 捕獲設置錯誤
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // 確保在應用程序關閉時刷新並停止內部計時器/線程
    NLog.LogManager.Shutdown();
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
