using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAccessControl.Application.AccessGrant;
using UserAccessControl.Application.Resource;
using UserAccessControl.Application.User;

namespace UserAccessControl.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<List<ListUserDto>> GetAll() => await _userService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<UserDto> GetById([FromRoute] Guid id) => await _userService.GetByIdAsync(id);

    [HttpPost]
    public async Task<UserDto> Create([FromBody] UserCreateDto dto) => await _userService.CreateAsync(dto);

    [HttpDelete("{id}")]
    public async Task<bool> Delete([FromRoute] Guid id) => await _userService.DeleteAsync(id);

    [HttpPost("{userId}/assign-access-grant")]
    public async Task<UserDto> AssignUserAccessGrants([FromRoute] Guid userId, [FromBody] AssignAccessDto assignAccessDto) => await _userService.AssignAccessAsync(userId, assignAccessDto);

    [HttpGet("{userId}/resources")]
    public async Task<List<ResourceDto>> GetUserResources([FromRoute] Guid userId) => await _userService.GetUserResourcesAsync(userId);
}