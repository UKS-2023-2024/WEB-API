using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Auth;
using Microsoft.IdentityModel.Tokens;

namespace WEB_API.Shared.TokenHandler;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _config;
    public TokenHandler(IConfiguration configuration)
    {
        _config = configuration;
    }
    public string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"] ?? throw new InvalidOperationException()));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.PrimaryEmail),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var token = new JwtSecurityToken(_config["JwtSettings:Issuer"], _config["JwtSettings:Audience"],
            claims, expires: DateTime.Now.AddHours(2), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}