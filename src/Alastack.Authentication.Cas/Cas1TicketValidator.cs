using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace Alastack.Authentication.Cas;

/// <summary>
/// The CAS 1.0 implementation of <see cref="ICasTicketValidator"/>.
/// </summary>
public class Cas1TicketValidator : ICasTicketValidator
{
    /// <inheritdoc />
    public async Task<AuthenticationTicket?> ValidateAsync(HttpContext context, AuthenticationScheme scheme, CasOptions options, ClaimsPrincipal principal, AuthenticationProperties properties, CasResponse response)
    {
        var data = response.Data;
        var dataArray = data.Split('\n');
        if (dataArray.Length >= 2 && dataArray[0] == "yes")
        {
            response.ResponseType = CasResponseType.Validate;
            response.UserName = dataArray[1];

            var casCreatingTicketContext = new CasCreatingTicketContext(context, scheme, options, principal, properties, response);
            casCreatingTicketContext.AppendClaims();
            await options.Events.CreatingTicket(casCreatingTicketContext);
            return new AuthenticationTicket(principal, properties, scheme.Name);
        }
        return null;
    }
}
