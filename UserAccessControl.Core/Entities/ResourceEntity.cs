﻿namespace UserAccessControl.Core.Entities;

public class ResourceEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<AccessGrantEntity>? AccessGrants { get; set; }

}