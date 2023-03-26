using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Specifies events which the <see cref="CasHandler"/> invokes to enable developer control over the authentication process.
/// </summary>
public class CasEvents : RemoteAuthenticationEvents
{
    /// <summary>
    /// Gets or sets the function that is invoked when the CreatingTicket method is invoked.
    /// </summary>
    public Func<CasCreatingTicketContext, Task> OnCreatingTicket { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Gets or sets the delegate that is invoked when the RedirectToAuthorizationEndpoint method is invoked.
    /// </summary>
    public Func<RedirectContext<CasOptions>, Task> OnRedirectToAuthorizationEndpoint { get; set; } = context =>
    {
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    /// <summary>
    /// Invoked after the provider successfully authenticates a user.
    /// </summary>
    /// <param name="context">Contains information about the login session as well as the user <see cref="ClaimsIdentity"/>.</param>
    /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
    public virtual Task CreatingTicket(CasCreatingTicketContext context) => OnCreatingTicket(context);

    /// <summary>
    /// Called when a Challenge causes a redirect to authorize endpoint in the CAS handler.
    /// </summary>
    /// <param name="context">Contains redirect URI and <see cref="AuthenticationProperties"/> of the challenge.</param>
    public virtual Task RedirectToAuthorizationEndpoint(RedirectContext<CasOptions> context) => OnRedirectToAuthorizationEndpoint(context);
}
