using System;
using UserAccessControl.Application.Resource;
using UserAccessControl.Core.Entities.Enum;

namespace UserAccessControl.Application.AccessGrant;


public class AccessGrantDto
{
    public Guid Id { get; set; }
    public ResourceDto Resource { get; set; }
    public AccessLevel AccessLevel { get; set; }
}


public class AssignAccessDto
{
    public Guid ResourceId { get; set; }
    public AccessLevel AccessLevel { get; set; }
}
