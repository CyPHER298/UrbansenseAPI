using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using UrbansenseAPI.Domain.Exceptions;
using UrbansenseAPI.Domain.Enums;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.Infrastructure.External;
using UrbansenseAPI.Infrastructure.Persistence;

namespace UrbansenseAPI.Tests.Services;

public class WeatherServiceTests
{
    private readonly IOpenWeatherClient _client = Substitute.For<IOpenWeatherClient>();
    private readonly WeatherStore       _store  = new();
    private readonly WeatherService     _sut;

    public WeatherServiceTests()
    {
        _sut = new WeatherService(_client, _store);
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenClientReturns_ShouldReturnMappedResponse()
    {
        // Arrange
        var weather = new Weather(
            Id: null, City: "São Paulo",
            Latitude: -23.5505, Longitude: -46.6333,
            Temperature: 28, FeelsLike: 30,
            Humidity: 85, WindSpeed: 3,
            RainMm: 5.0, UvIndex: null,
            Condition: WeatherCondition.Rain,
            Description: "chuva leve",
            RecordedAt: DateTime.UtcNow);

        _client.FetchCurrentAsync("São Paulo", -23.5505, -46.6333).Returns(weather);

        // Act
        var result = await _sut.GetCurrentWeatherAsync("São Paulo", -23.5505, -46.6333);

        // Assert
        result.City.Should().Be("São Paulo");
        result.Temperature.Should().Be(28);
        result.RainMm.Should().Be(5.0);
        result.Condition.Should().Be("Rain");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenClientReturns_ShouldAddToStore()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var weather = new Weather(
            null, "Guarulhos", -23.4538, -46.5333,
            22, 21, 90, 5, 12.0, null,
            WeatherCondition.Rain, "chuva moderada", now);

        _client.FetchCurrentAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
               .Returns(weather);

        // Act
        await _sut.GetCurrentWeatherAsync("Guarulhos", -23.4538, -46.5333);

        // Assert — store deve ter o registro
        var stored = _store.GetSince(now.AddSeconds(-1));
        stored.Should().HaveCount(1);
        stored[0].City.Should().Be("Guarulhos");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenClientThrows_ShouldWrapInExternalApiException()
    {
        // Arrange
        _client.FetchCurrentAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
               .Throws(new HttpRequestException("connection refused"));

        // Act
        var act = () => _sut.GetCurrentWeatherAsync("SP", 0, 0);

        // Assert
        await act.Should().ThrowAsync<ExternalApiException>()
            .WithMessage("*OpenWeather*");
    }

    [Fact]
    public async Task AnalyzeRainRiskAsync_WhenStoreIsEmpty_ShouldReturnLowRisk()
    {
        // Arrange — store vazio, sem histórico de chuva

        // Act
        var result = await _sut.AnalyzeRainRiskAsync("São Paulo", 10);

        // Assert
        result.Level.Should().Be("Low");
        result.AvgRainMm.Should().Be(0.0);
    }

    [Fact]
    public async Task GetHeavyRainAreasAsync_WhenNoHeavyRainInLastHour_ShouldReturnEmpty()
    {
        // Arrange — store vazio

        // Act
        var result = await _sut.GetHeavyRainAreasAsync(20.0);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetHeavyRainAreasAsync_WhenHeavyRainExists_ShouldReturnMatchingCities()
    {
        // Arrange — adiciona chuva forte direto na store
        var heavyRain = new Weather(
            null, "Osasco", -23.5322, -46.7919,
            20, 19, 95, 8, 25.0, null,
            WeatherCondition.Rain, "chuva forte", DateTime.UtcNow);
        var lightRain = new Weather(
            null, "Campinas", -22.9068, -47.0626,
            24, 23, 70, 2, 3.0, null,
            WeatherCondition.Rain, "garoa", DateTime.UtcNow);

        _store.Add(heavyRain);
        _store.Add(lightRain);

        // Act
        var result = await _sut.GetHeavyRainAreasAsync(20.0);

        // Assert
        result.Should().HaveCount(1);
        result[0].City.Should().Be("Osasco");
    }
}
