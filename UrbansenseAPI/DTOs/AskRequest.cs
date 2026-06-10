using System.ComponentModel.DataAnnotations;

namespace UrbansenseAPI.DTOs;

public record AskRequest(
    [Required][MinLength(3)] string Question,
    [Required][MinLength(2)] string City
);
