using Microsoft.EntityFrameworkCore;
using UserAccessControl.Core.Database;
using UserAccessControl.Core.Entities;

namespace UserAccessControl.Application.Resource;

public interface IResourceRepository
{
    Task<List<ResourceEntity>> GetAllAsync();
    Task<ResourceEntity?> GetByIdAsync(Guid id);
    Task AddAsync(ResourceEntity resource);
    Task DeleteAsync(ResourceEntity resource);
}

public class ResourceRepository : IResourceRepository
{
    private readonly AppDbContext _context;

    public ResourceRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<ResourceEntity>> GetAllAsync()
    {
        return _context.Resources.ToListAsync();
    }

    public Task<ResourceEntity?> GetByIdAsync(Guid id)
    {
        return _context.Resources.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(ResourceEntity Resource)
    {
        await _context.Resources.AddAsync(Resource);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ResourceEntity Resource)
    {
        _context.Resources.Remove(Resource);
        await _context.SaveChangesAsync();
    }
}

