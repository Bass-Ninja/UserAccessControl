using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAccessControl.Core.Database;
using UserAccessControl.Core.Entities;

namespace UserAccessControl.Application.User;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task AddAsync(UserEntity user);
    Task DeleteAsync(UserEntity user);

    // Access Grants
    Task<AccessGrantEntity> AddAccessGrantAsync(AccessGrantEntity grant);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<UserEntity>> GetAllAsync()
    {
        return _context.Users.ToListAsync();
    }

    public Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return _context.Users.Include(u => u.AccessGrants).ThenInclude(u => u.Resource).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserEntity user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<AccessGrantEntity> AddAccessGrantAsync(AccessGrantEntity grant)
    {
        bool exists = await _context.AccessGrants
            .AnyAsync(ag => ag.UserId == grant.UserId && ag.ResourceId == grant.ResourceId);
        if (exists)
            throw new Exception("Access already granted");

        _context.AccessGrants.Add(grant);
        await _context.SaveChangesAsync();
        return grant;
    }
}

