using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public interface IJwtService
{
    TokenResponse GenerateToken(AppUser user);
}
