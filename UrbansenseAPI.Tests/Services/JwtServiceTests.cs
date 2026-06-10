using Xunit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.Domain.Services;

namespace UrbansenseAPI.Tests.Services;

public class JwtServiceTests
{
    private readonly JwtService _sut;

    public JwtServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"]      = "supersecretkey-for-testing-purposes-only-32chars",
                ["Jwt:Issuer"]      = "UrbansenseAPI",
                ["Jwt:Audience"]    = "UrbansenseClients",
                ["Jwt:ExpiryHours"] = "24"
            })
            .Build();

        _sut = new JwtService(config);
    }

    [Fact]
    public void GenerateToken_ShouldReturnNonEmptyToken()
    {
        // Arrange
        var user = new AppUser { Id = 1, Username = "joao", Role = "USER" };

        // Act
        var result = _sut.GenerateToken(user);

        // Assert
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GenerateToken_ShouldReturnCorrectUsernameAndRole()
    {
        // Arrange
        var user = new AppUser { Id = 42, Username = "maria", Role = "ADMIN" };

        // Act
        var result = _sut.GenerateToken(user);

        // Assert
        result.Username.Should().Be("maria");
        result.Role.Should().Be("ADMIN");
    }

    [Fact]
    public void GenerateToken_ShouldReturnExpiresAtInFuture()
    {
        // Arrange
        var user = new AppUser { Id = 1, Username = "test", Role = "USER" };

        // Act
        var result = _sut.GenerateToken(user);

        // Assert
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public void GenerateToken_ShouldEmbedUsernameAndRoleClaims()
    {
        // Arrange
        var user = new AppUser { Id = 7, Username = "pedro", Role = "USER" };

        // Act
        var result = _sut.GenerateToken(user);

        // Assert â€” decodifica o JWT e verifica as claims
        var handler = new JwtSecurityTokenHandler();
        var jwt     = handler.ReadJwtToken(result.Token);

        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Name      && c.Value == "pedro");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role      && c.Value == "USER");
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "pedro");
    }

    [Fact]
    public void GenerateToken_ShouldSetCorrectIssuerAndAudience()
    {
        // Arrange
        var user = new AppUser { Id = 1, Username = "ana", Role = "USER" };

        // Act
        var result = _sut.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwt     = handler.ReadJwtToken(result.Token);

        jwt.Issuer.Should().Be("UrbansenseAPI");
        jwt.Audiences.Should().Contain("UrbansenseClients");
    }

    [Fact]
    public void GenerateToken_TwoCallsForSameUser_ShouldReturnDifferentJtiClaims()
    {
        // Arrange
        var user = new AppUser { Id = 1, Username = "lucas", Role = "USER" };

        // Act
        var token1 = _sut.GenerateToken(user);
        var token2 = _sut.GenerateToken(user);

        // Assert â€” cada token deve ter um Jti Ãºnico
        var handler = new JwtSecurityTokenHandler();
        var jti1    = handler.ReadJwtToken(token1.Token).Id;
        var jti2    = handler.ReadJwtToken(token2.Token).Id;

        jti1.Should().NotBe(jti2);
    }
}

