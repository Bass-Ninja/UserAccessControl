using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using UserAccessControl.Application.User;
using UserAccessControl.Core.Entities;
using Xunit;

namespace UserAccessControl.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedUsers()
    {
        List<UserEntity> users = new()
        {
            new() { Id = Guid.NewGuid(), Email = "user1@ninja.si", FirstName = "User", LastName = "One" },
            new() { Id = Guid.NewGuid(), Email = "user2@ninja.si", FirstName = "User", LastName = "Two" },
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        List<ListUserDto> result = await _userService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Email == "user1@ninja.si");
        Assert.Contains(result, u => u.Email == "user2@ninja.si");
    }

    [Fact]
    public async Task GetByIdAsync_UserNotFound_ThrowsException()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((UserEntity)null);
        await Assert.ThrowsAsync<Exception>(async () => await _userService.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_AddsUserAndReturnsDto()
    {
        UserEntity savedUser = null!;
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(u => savedUser = u)
            .Returns(Task.CompletedTask);

        UserCreateDto dto = new()
        {
            Email = "test@ninja.si",
            FirstName = "Test",
            LastName = "User"
        };

        UserDto result = await _userService.CreateAsync(dto);

        Assert.NotNull(savedUser);
        Assert.Equal(dto.Email, savedUser.Email);
        Assert.Equal(dto.FirstName, savedUser.FirstName);
        Assert.Equal(dto.LastName, savedUser.LastName);

        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
    }
}
