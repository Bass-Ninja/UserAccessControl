using UserAccessControl.Core.Entities;

namespace UserAccessControl.Application.Resource;

public interface IResourceService
{
    Task<List<ResourceDto>> GetAllAsync();
    Task<ResourceDto> GetByIdAsync(Guid id);
    Task<ResourceDto> CreateAsync(ResourceCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class ResourceService : IResourceService
{
    private readonly IResourceRepository _repo;

    public ResourceService(IResourceRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ResourceDto>> GetAllAsync()
    {
        List<ResourceEntity> resources = await _repo.GetAllAsync();
        return resources.Select(u => new ResourceDto
        {
            Id = u.Id,
            Name = u.Name,
            Description = u.Description,
        }).ToList();
    }

    public async Task<ResourceDto> GetByIdAsync(Guid id)
    {
        ResourceEntity? resource = await _repo.GetByIdAsync(id);
        if (resource == null) throw new Exception("Resource not found");

        return new ResourceDto
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
        };
    }

    public async Task<ResourceDto> CreateAsync(ResourceCreateDto dto)
    {
        ResourceEntity resource = new ResourceEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Id = Guid.Empty,
        };

        await _repo.AddAsync(resource);
        return new ResourceDto
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        ResourceEntity? resource = await _repo.GetByIdAsync(id);
        if (resource == null) return false;

        await _repo.DeleteAsync(resource);
        return true;
    }
}