using System.Security.Claims;

namespace Tests.Helpers;

public static class ClaimsData
{
    private static List<Claim> GetBasicClaims()
    {
        return new()
        {
            new Claim("username", "testuser"),
            new Claim("email", "test@test.com"),
            new Claim("nameidentifier", "1")
        };
    }

    public static List<Claim> GetAdminClaims()
    {
        var claims = GetBasicClaims();
        claims.Add(new Claim(ClaimTypes.Role, "admin"));
        return claims;
    }

    public static List<Claim> GetSchoolClaims()
    {
        var claims = GetBasicClaims();
        claims.Add(new Claim(ClaimTypes.Role, "school"));
        return claims;
    }

    public static List<Claim> GetBranchClaims()
    {
        var claims = GetBasicClaims();
        claims.Add(new Claim(ClaimTypes.Role, "branch"));
        claims.Add(new Claim("schoolId", "1"));
        return claims;
    }

    public static List<Claim> GetTrainerClaims()
    {
        var claims = GetBasicClaims();
        claims.Add(new Claim(ClaimTypes.Role, "trainer"));
        claims.Add(new Claim("specialization", "test-specialization"));
        return claims;
    }

    public static List<Claim> GetStudentClaims()
    {
        var claims = GetBasicClaims();
        claims.Add(new Claim(ClaimTypes.Role, "student"));
        claims.Add(new Claim("branchId", "1"));
        claims.Add(new Claim("parentId", "1"));
        return claims;
    }

    public static List<Claim> GetParentClaims()
    {
        var claims = GetBasicClaims();
        claims.Add(new Claim(ClaimTypes.Role, "parent"));
        return claims;
    }

    public static List<Claim> GetUnauthorizedClaims()
    {
        // Sadece temel claims'leri döndür, rol claim'i ekleme
        return GetBasicClaims();
    }
}