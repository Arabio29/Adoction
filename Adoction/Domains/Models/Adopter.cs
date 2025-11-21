namespace Adoction.Domains.Models;

public class Adopter
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserId { get; set; } = default!; // FK a Identity
}
