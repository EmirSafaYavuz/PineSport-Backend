using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Tests.Helpers.Token;

public static class MockJwtTokens
{
    private static readonly JwtSecurityTokenHandler s_tokenHandler = new ();
    private static string s_keyString = "!aCZX*_asdad*!ASDA*_*!AADSxvaz2x3XCS4v5B*_*";

    static MockJwtTokens()
    {
        var SKey = Encoding.UTF8.GetBytes(s_keyString);
        SecurityKey = new SymmetricSecurityKey(SKey);
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
    }

    public static string Issuer { get; } = "www.pinesport.com";
    public static string Audience { get; } = "www.pinesport.com";
    public static SecurityKey SecurityKey { get; }
    public static SigningCredentials SigningCredentials { get; }

    public static string GenerateJwtToken(IEnumerable<Claim> claims, double value = 5)
    {
        return s_tokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(value), SigningCredentials));
    }
}