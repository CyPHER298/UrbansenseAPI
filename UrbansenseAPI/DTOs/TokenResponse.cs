namespace UrbansenseAPI.DTOs;

/// <summary>Token JWT retornado após autenticação bem-sucedida</summary>
public record TokenResponse(
    /// <summary>Token JWT para usar no header Authorization: Bearer {token}</summary>
    string Token,
    /// <summary>Nome do usuário autenticado</summary>
    string Username,
    /// <summary>Role do usuário: USER ou ADMIN</summary>
    string Role,
    /// <summary>Data e hora de expiração do token (UTC)</summary>
    DateTime ExpiresAt
);
