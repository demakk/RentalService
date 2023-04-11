using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentalService.Application.Options;

namespace RentalService.Application.Services;

public class IdentityService
{

    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;

    public IdentityService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
    }

    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();


    public SecurityToken CreateSecurityToken(ClaimsIdentity claimsIdentity)
    {
        var securityToken = CreateTokenDescriptor(claimsIdentity);
        return _tokenHandler.CreateToken(securityToken);
    }

    public string WriteToken(SecurityToken securityToken)
    {
        return _tokenHandler.WriteToken(securityToken);
    }


    private SecurityTokenDescriptor CreateTokenDescriptor(ClaimsIdentity claimsIdentity)
    {
        return new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddHours(2),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience[0],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };
    }
}