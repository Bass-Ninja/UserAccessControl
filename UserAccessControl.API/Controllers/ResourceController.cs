using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAccessControl.Application.Resource;

namespace UserAccessControl.API.Controllers;

[ApiController]
[Authorize]
[Route("api/resource")]
public class ResourceController : ControllerBase
{
    private readonly IResourceService _resourceService;

    public ResourceController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpGet]
    public async Task<List<ResourceDto>> GetAll() => await _resourceService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ResourceDto> GetById([FromRoute] Guid id) => await _resourceService.GetByIdAsync(id);

    [HttpPost]
    public async Task<ResourceDto> Create([FromBody] ResourceCreateDto dto) => await _resourceService.CreateAsync(dto);

    [HttpDelete("{id}")]
    public async Task<bool> Delete([FromRoute] Guid id) => await _resourceService.DeleteAsync(id);
}