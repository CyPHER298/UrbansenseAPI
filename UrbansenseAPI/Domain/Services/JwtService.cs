using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly string _secret      = configuration["Jwt:Secret"]!;
    private readonly int    _expiryHours = int.Parse(configuration["Jwt:ExpiryHours"] ?? "24");

    public TokenResponse GenerateToken(AppUser user)
    {
        var expiresAt = DateTime.UtcNow.AddHours(_expiryHours);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   user.Username),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier,     user.Id.ToString()),
            new Claim(ClaimTypes.Name,               user.Username),
            new Claim(ClaimTypes.Role,               user.Role)
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             configuration["Jwt:Issuer"],
            audience:           configuration["Jwt:Audience"],
            claims:             claims,
            expires:            expiresAt,
            signingCredentials: creds
        );

        return new TokenResponse(
            Token:     new JwtSecurityTokenHandler().WriteToken(token),
            Username:  user.Username,
            Role:      user.Role,
            ExpiresAt: expiresAt
        );
    }
}
