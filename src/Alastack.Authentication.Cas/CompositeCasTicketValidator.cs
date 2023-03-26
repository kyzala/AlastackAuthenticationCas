using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Alastack.Authentication.Cas;

/// <summary>
/// The Composite implementation of <see cref="ICasTicketValidator"/>.
/// </summary>
public class CompositeCasTicketValidator : ICasTicketValidator
{
    /// <inheritdoc />
    public async Task<AuthenticationTicket?> ValidateAsync(HttpContext context, AuthenticationScheme scheme, CasOptions options, ClaimsPrincipal principal, AuthenticationProperties properties, CasResponse response)
    {
        var ticket = await new Cas23TicketValidator().ValidateAsync(context, scheme, options, principal, properties, response);
        ticket ??= await new Cas1TicketValidator().ValidateAsync(context, scheme, options, principal, properties, response);

        return ticket;
    }
}
