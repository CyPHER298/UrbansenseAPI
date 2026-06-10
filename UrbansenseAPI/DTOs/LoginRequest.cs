using System.ComponentModel.DataAnnotations;

namespace UrbansenseAPI.DTOs;

public record LoginRequest(
    [Required][MinLength(3)] string Username,
    [Required][MinLength(6)] string Password
);
