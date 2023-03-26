using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Checks the validity of a service ticket and returns the <see cref="AuthenticationTicket"/>.
/// </summary>
public interface ICasTicketValidator
{
    /// <summary>
    /// Validate CAS service ticket and create AuthenticationTicket object.
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/></param>
    /// <param name="scheme"><see cref="AuthenticationScheme"/></param>
    /// <param name="options"><see cref="CasOptions"/></param>
    /// <param name="principal"><see cref="ClaimsPrincipal"/></param>
    /// <param name="properties"><see cref="AuthenticationProperties"/></param>
    /// <param name="response"><see cref="CasResponse"/></param>
    /// <returns></returns>
    Task<AuthenticationTicket?> ValidateAsync(HttpContext context, AuthenticationScheme scheme, CasOptions options, ClaimsPrincipal principal, AuthenticationProperties properties, CasResponse response);
}
