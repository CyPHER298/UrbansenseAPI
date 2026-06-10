using NSubstitute.ExceptionExtensions;
using Xunit;
using FluentAssertions;
using NSubstitute;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.DTOs;
using UrbansenseAPI.Infrastructure.Persistence;

namespace UrbansenseAPI.Tests.Services;

public class AlertServiceTests
{
    private readonly IWeatherService _weatherService = Substitute.For<IWeatherService>();
    private readonly AlertStore      _alertStore     = new();
    private readonly AlertService    _sut;

    public AlertServiceTests()
    {
        _sut = new AlertService(_weatherService, _alertStore, Substitute.For<Microsoft.Extensions.Logging.ILogger<UrbansenseAPI.Domain.Services.AlertService>>());
    }

    [Fact]
    public async Task GetActiveByCityAsync_WhenNoAlerts_ShouldReturnEmptyList()
    {
        // Arrange
        var city = "SÃ£o Paulo";

        // Act
        var result = await _sut.GetActiveByCityAsync(city);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenRainAbove20mm_ShouldCreateFloodingAlert()
    {
        // Arrange
        var weather = new WeatherResponse(
            "SÃ£o Paulo", -23.5505, -46.6333,
            28, 27, 90, 4, 25.0, null, "Rain", "chuva forte", DateTime.UtcNow);

        _weatherService
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
            .Returns(weather);

        // Act
        await _sut.EvaluateAlertsAsync();
        var alerts = await _sut.GetActiveByCityAsync("SÃ£o Paulo");

        // Assert
        alerts.Should().Contain(a => a.Type == "Flooding");
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenRainAbove40mm_ShouldCreateCriticalFloodingAlert()
    {
        // Arrange
        var weather = new WeatherResponse(
            "SÃ£o Paulo", -23.5505, -46.6333,
            22, 21, 95, 8, 45.0, null, "Rain", "chuva muito forte", DateTime.UtcNow);

        _weatherService
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
            .Returns(weather);

        // Act
        await _sut.EvaluateAlertsAsync();
        var alerts = await _sut.GetActiveByCityAsync("SÃ£o Paulo");

        // Assert
        alerts.Should().Contain(a => a.Type == "Flooding" && a.Severity == "Critical");
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenRainBetween10And20mm_ShouldCreateHeavyRainAlert()
    {
        // Arrange
        var weather = new WeatherResponse(
            "SÃ£o Paulo", -23.5505, -46.6333,
            25, 24, 85, 3, 15.0, null, "Rain", "chuva moderada", DateTime.UtcNow);

        _weatherService
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
            .Returns(weather);

        // Act
        await _sut.EvaluateAlertsAsync();
        var alerts = await _sut.GetActiveByCityAsync("SÃ£o Paulo");

        // Assert
        alerts.Should().Contain(a => a.Type == "HeavyRain" && a.Severity == "Medium");
        alerts.Should().NotContain(a => a.Type == "Flooding");
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenUvAbove8_ShouldCreateHighUvAlert()
    {
        // Arrange
        var weather = new WeatherResponse(
            "SÃ£o Paulo", -23.5505, -46.6333,
            32, 34, 40, 2, 0.0, 10, "Clear", "cÃ©u limpo", DateTime.UtcNow);

        _weatherService
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
            .Returns(weather);

        // Act
        await _sut.EvaluateAlertsAsync();
        var alerts = await _sut.GetActiveByCityAsync("SÃ£o Paulo");

        // Assert
        alerts.Should().Contain(a => a.Type == "HighUv");
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenConditionIsStorm_ShouldCreateCriticalAlert()
    {
        // Arrange
        var weather = new WeatherResponse(
            "Guarulhos", -23.4538, -46.5333,
            19, 18, 98, 12, 18.0, null, "Storm", "tempestade", DateTime.UtcNow);

        _weatherService
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
            .Returns(weather);

        // Act
        await _sut.EvaluateAlertsAsync();
        var alerts = await _sut.GetActiveByCityAsync("Guarulhos");

        // Assert
        alerts.Should().Contain(a => a.Severity == "Critical");
    }

    [Fact]
    public async Task GetByTypeAsync_WhenInvalidType_ShouldReturnEmptyList()
    {
        // Arrange
        var invalidType = "TipoInexistente";

        // Act
        var result = await _sut.GetByTypeAsync(invalidType);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenApiThrows_ShouldNotPropagateException()
    {
        // Arrange â€” API falha em todas as cidades
        _weatherService
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
            .Throws(new HttpRequestException("timeout"));

        // Act
        var act = () => _sut.EvaluateAlertsAsync();

        // Assert â€” nÃ£o deve lanÃ§ar exceÃ§Ã£o
        await act.Should().NotThrowAsync();
    }
}




