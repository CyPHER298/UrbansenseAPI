namespace UrbansenseAPI.Domain.Models;

public class AppUser
{
    public long   Id       { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role     { get; set; } = "USER";
    public bool   Active   { get; set; } = true;
}
