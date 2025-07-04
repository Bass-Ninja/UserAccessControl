using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAccessControl.Application.Auth;
using UserAccessControl.Application.User;

namespace UserAccessControl.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        // Simulate fixed user
        string email = "admin@ninja.si";
        string role = "Admin";

        string token = authService.GenerateToken(email, role);
        return Ok(new { token });
    }
}
