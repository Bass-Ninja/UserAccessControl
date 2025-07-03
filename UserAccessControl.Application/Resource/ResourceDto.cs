namespace UserAccessControl.Application.Resource;

public class ResourceDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class ResourceCreateDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}