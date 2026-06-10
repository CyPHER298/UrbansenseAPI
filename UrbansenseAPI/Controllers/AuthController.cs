using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbansenseAPI.Data;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Controllers;

/// <summary>Endpoints de autenticação e registro de usuários</summary>
[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public class AuthController(AppDbContext db, IJwtService jwtService) : ControllerBase
{
    /// <summary>Registra um novo usuário na plataforma</summary>
    /// <param name="request">Dados do novo usuário</param>
    /// <returns>Dados do usuário criado</returns>
    /// <remarks>
    /// O campo `role` aceita apenas "USER" (padrão) ou "ADMIN".
    /// A senha é armazenada com hash BCrypt.
    ///
    ///     POST /api/v1/auth/register
    ///     {
    ///         "username": "henrique",
    ///         "password": "senha123",
    ///         "role": "USER"
    ///     }
    ///
    /// </remarks>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await db.Users.CountAsync(u => u.Username == request.Username) > 0)
            return Conflict(new { message = "Usuário já existe." });

        var user = new AppUser
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role     = request.Role.ToUpper() == "ADMIN" ? "ADMIN" : "USER",
            Active   = true
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Created($"/api/v1/auth/{user.Id}", new { user.Id, user.Username, user.Role });
    }

    /// <summary>Autentica o usuário e retorna o token JWT</summary>
    /// <param name="request">Credenciais do usuário</param>
    /// <returns>Token JWT com username, role e data de expiração</returns>
    /// <remarks>
    /// O token retornado deve ser enviado no header `Authorization: Bearer {token}`
    /// em todas as requisições protegidas.
    ///
    ///     POST /api/v1/auth/login
    ///     {
    ///         "username": "henrique",
    ///         "password": "senha123"
    ///     }
    ///
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null || !user.Active || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized(new { message = "Credenciais inválidas." });

        return Ok(jwtService.GenerateToken(user));
    }
}
