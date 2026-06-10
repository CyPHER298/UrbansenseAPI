namespace UrbansenseAPI.DTOs;

public record AdvisorResponse(
    string Answer,
    string City,
    DateTime GeneratedAt
);
