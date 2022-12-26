using Domain.Entities.Members;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;
public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions mOptions;
    private readonly int cTimeoutInMinutes = 60;
    private readonly IPermissionService mPermissionService;

    public JwtProvider(IOptions<JwtOptions> options, IPermissionService permissionService)
    {
        mOptions = options.Value;
        mPermissionService = permissionService;
    }

    public string Generate(MemberEntity member)
    {
        var claims = new Claim[] 
        {
            new(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, member.Email.Value.ToString())
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(mOptions.Secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            mOptions.Issuer,
            mOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(cTimeoutInMinutes),
            signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }
    public async Task<string> GenerateAsync(MemberEntity member)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, member.Email.Value.ToString())
        };

        HashSet<string> permissions = await mPermissionService
            .GetPermissionsAsync(member.Id);

        foreach (string permission in permissions)
            claims.Add(new(CustomClaims.Permissions, permission));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(mOptions.Secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            mOptions.Issuer,
            mOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(cTimeoutInMinutes),
            signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }
}
