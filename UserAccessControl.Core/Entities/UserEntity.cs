
using Microsoft.AspNetCore.Identity;

namespace UserAccessControl.Core.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public ICollection<AccessGrantEntity>? AccessGrants { get; set; }

}