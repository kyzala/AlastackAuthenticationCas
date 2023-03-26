using System.Security.Claims;

namespace Alastack.Authentication.Cas;

/// <summary>
/// The default implementation of <see cref="ICasClaimsFilter"/>.
/// </summary>
public class DefaultCasClaimsFilter : ICasClaimsFilter
{
    /// <inheritdoc />
    public void Select(CasOptions options, ClaimsIdentity identity, CasResponse response)
    {
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, response.UserName));
        identity.AddClaim(new Claim(ClaimTypes.Name, response.UserName));

        foreach (var pair in response.Attributes)
        {
            identity.AddClaim(new Claim(pair.Key, pair.Value, null, options.ClaimsIssuer));
        }
    }
}
