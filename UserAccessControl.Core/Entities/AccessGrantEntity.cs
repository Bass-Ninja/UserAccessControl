using UserAccessControl.Core.Entities.Enum;

namespace UserAccessControl.Core.Entities;

public class AccessGrantEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public Guid ResourceId { get; set; }
    public ResourceEntity? Resource { get; set; }
    public AccessLevel AccessLevel { get; set; }
}