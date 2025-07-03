using UserAccessControl.Application.AccessGrant;

namespace UserAccessControl.Application.User;

public class ListUserDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public ICollection<AccessGrantDto>? AccessGrants { get; set; }
}

public class UserCreateDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}