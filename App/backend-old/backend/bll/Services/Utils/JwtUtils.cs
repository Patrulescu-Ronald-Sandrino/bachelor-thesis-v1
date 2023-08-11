using System.Security.Claims;

namespace bll.Services.Utils;

public class JwtUtils
{
    public static Claim[] ConvertMapToClaimsArray(Dictionary<string, string> claimsMap) =>
        claimsMap.Select(pair => new Claim(pair.Key, pair.Value)).ToArray();
}