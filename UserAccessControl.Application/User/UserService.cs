using Microsoft.Extensions.Configuration;
using UserAccessControl.Application.AccessGrant;
using UserAccessControl.Application.Resource;
using UserAccessControl.Core.Entities;

namespace UserAccessControl.Application.User;


public interface IUserService
{
    Task<List<ListUserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> CreateAsync(UserCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<UserDto> AssignAccessAsync(Guid userId, AssignAccessDto assignAccessDto);
    Task<List<ResourceDto>> GetUserResourcesAsync(Guid userId);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ListUserDto>> GetAllAsync()
    {
        List<UserEntity> users = await _repo.GetAllAsync();
        return users.Select(u => new ListUserDto
        {
            Id = u.Id,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName
        }).ToList();
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        UserEntity? user = await _repo.GetByIdAsync(id);
        if (user == null) throw new Exception("User not found");

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AccessGrants = user.AccessGrants?.Select(ag => new AccessGrantDto
            {
                Id = ag.Id,
                Resource = new ResourceDto
                {
                    Id = ag.Resource.Id,
                    Name = ag.Resource.Name,
                    Description = ag.Resource.Description
                },
                AccessLevel = ag.AccessLevel,
            }).ToList()
        };
    }

    public async Task<UserDto> CreateAsync(UserCreateDto dto)
    {
        UserEntity user = new()
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        await _repo.AddAsync(user);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        UserEntity? user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        await _repo.DeleteAsync(user);
        return true;
    }

    public async Task<UserDto> AssignAccessAsync(Guid userId, AssignAccessDto assignAccessDto)
    {
        // AccessLevel indicates the permission level (Read, Write, Admin) a user has for a resource.
        // Currently, this project stores and returns AccessLevel only for informational purposes, not restriction.

        AccessGrantEntity accessGrant = new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ResourceId = assignAccessDto.ResourceId,
            AccessLevel = assignAccessDto.AccessLevel,
        };

        await _repo.AddAccessGrantAsync(accessGrant);

        UserEntity? user = await _repo.GetByIdAsync(userId);

        UserDto userDto = new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            AccessGrants = user.AccessGrants.Select(ag => new AccessGrantDto
            {
                Id = ag.Id,
                Resource = new ResourceDto
                {
                    Id = ag.Resource.Id,
                    Name = ag.Resource.Name,
                    Description = ag.Resource.Description
                },
                AccessLevel = ag.AccessLevel,
            }).ToList()
        };

        return userDto;
    }

    public async Task<List<ResourceDto>> GetUserResourcesAsync(Guid userId)
    {
        UserEntity? user = await _repo.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        return user.AccessGrants?.Select(ag => new ResourceDto
        {
            Id = ag.Resource.Id,
            Name = ag.Resource.Name,
            Description = ag.Resource.Description
        }).ToList() ?? [];
    }
}