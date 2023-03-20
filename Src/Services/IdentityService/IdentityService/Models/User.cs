namespace IdentityService.Models;
public class User
{
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required bool IsActive { get; set; } = true;
    public required DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
