using Xunit;
using FluentAssertions;
using UrbansenseAPI.Domain.Enums;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Tests.Domain;

public class RainRiskTests
{
    [Fact]
    public void Calculate_WhenNoRainAndNoEvents_ShouldReturnLowRisk()
    {
        // Arrange
        var city = "São Paulo";
        var hour = 10;
        double? avgRain = null;
        var events = 0;

        // Act
        var result = RainRisk.Calculate(city, hour, avgRain, events);

        // Assert
        result.Level.Should().Be(RiskLevel.Low);
        result.RiskScore.Should().BeLessThan(0.4);
        result.AvgRainMm.Should().Be(0.0);
        result.City.Should().Be(city);
        result.Hour.Should().Be(hour);
    }

    [Fact]
    public void Calculate_WhenModerateRainAndFewEvents_ShouldReturnMediumRisk()
    {
        // Arrange
        var city = "Guarulhos";
        var hour = 15;
        double? avgRain = 10.0;
        var events = 6;

        // Act
        var result = RainRisk.Calculate(city, hour, avgRain, events);

        // Assert
        result.Level.Should().Be(RiskLevel.Medium);
        result.RiskScore.Should().BeGreaterThanOrEqualTo(0.4).And.BeLessThan(0.7);
    }

    [Fact]
    public void Calculate_WhenHeavyRainAndManyEvents_ShouldReturnHighRisk()
    {
        // Arrange
        var city = "Osasco";
        var hour = 17;
        double? avgRain = 20.0;
        var events = 15;

        // Act
        var result = RainRisk.Calculate(city, hour, avgRain, events);

        // Assert
        result.Level.Should().Be(RiskLevel.High);
        result.RiskScore.Should().BeGreaterThanOrEqualTo(0.7);
        result.HistoricalEvents.Should().Be(15);
    }

    [Fact]
    public void Calculate_WhenRainExceedsMaxThreshold_ShouldCapScoreAt1()
    {
        // Arrange
        double? avgRain = 100.0;
        var events = 20;

        // Act
        var result = RainRisk.Calculate("SP", 18, avgRain, events);

        // Assert
        result.RiskScore.Should().BeApproximately(1.0, 0.01);
        result.Level.Should().Be(RiskLevel.High);
    }

    [Theory]
    [InlineData(0.0,  0,  RiskLevel.Low)]
    [InlineData(10.0, 7, RiskLevel.Medium)]
    [InlineData(20.0, 15, RiskLevel.High)]
    public void Calculate_MultipleScenarios_ShouldReturnCorrectLevel(
        double rain, int events, RiskLevel expectedLevel)
    {
        // Arrange & Act
        var result = RainRisk.Calculate("SP", 12, rain, events);

        // Assert
        result.Level.Should().Be(expectedLevel);
    }

    [Fact]
    public void Alert_ShouldHaveTransitLineIdProperty()
    {
        // Arrange
        var alert = new Alert();

        // Act
        alert.transitLineId = 1;

        // Assert
        alert.transitLineId.Should().Be(1);
    }
}


