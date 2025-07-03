using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace UserAccessControl.Application.Auth;

public interface IAuthService
{
    string GenerateToken(string email, string role);
}

public class AuthService : IAuthService
{
    private readonly string _jwtKey;

    public AuthService(IOptions<JwtSettings> jwtKey)
    {
        _jwtKey = jwtKey.Value.Key;
    }

    public string GenerateToken(string email, string role)
    {
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, "UserAccessControlClients"),
            new Claim(JwtRegisteredClaimNames.Iss, "UserAccessControl")
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}