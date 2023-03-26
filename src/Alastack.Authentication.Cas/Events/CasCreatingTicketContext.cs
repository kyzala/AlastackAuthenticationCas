using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Contains information about the login session as well as the user <see cref="ClaimsIdentity"/>.
/// </summary>
public class CasCreatingTicketContext : ResultContext<CasOptions>
{
    /// <summary>
    /// Initializes a new <see cref="CasCreatingTicketContext"/>.
    /// </summary>
    /// <param name="context">The http context.</param>
    /// <param name="scheme">The authentication scheme.</param>
    /// <param name="options">The options used by the authentication middleware.</param>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/>.</param>
    /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
    /// <param name="response">CAS validation Response.</param>
    public CasCreatingTicketContext(            
        HttpContext context,
        AuthenticationScheme scheme,
        CasOptions options,
        //HttpClient backchannel,
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        CasResponse response)
        : base(context, scheme, options)
    {
        //if (backchannel == null)
        //{
        //    throw new ArgumentNullException(nameof(backchannel));
        //}
        //Backchannel = backchannel; 
        Principal = principal;
        Properties = properties;
        CasResponse = response;
    }

    /// <summary>
    /// <see cref="CasResponse"/>
    /// </summary>
    public CasResponse CasResponse { get; set; }        

    /// <summary>
    /// Gets the main identity exposed by the authentication ticket.
    /// This property returns <c>null</c> when the ticket is <c>null</c>.
    /// </summary>
    public ClaimsIdentity? Identity => Principal?.Identity as ClaimsIdentity;

    /// <summary>
    /// Examines <see cref="Cas.CasResponse"/>, determine if the requisite data is present, and optionally add it to <see cref="ClaimsIdentity"/>.
    /// </summary>
    public void AppendClaims() => AppendClaims(CasResponse);

    /// <summary>
    /// Examines <see cref="Cas.CasResponse"/>, determine if the requisite data is present, and optionally add it to <see cref="ClaimsIdentity"/>.
    /// </summary>
    /// <param name="response">The <see cref="Cas.CasResponse"/>.</param>
    public void AppendClaims(CasResponse response)
    {
        if (Options.ClaimsFilter != null && Identity != null)
        {
            Options.ClaimsFilter.Select(Options, Identity, response);
        }
        //else 
        //{
        //    var selector = HttpContext.RequestServices.GetService<ICasClaimsResolverSelector>();
        //    if (selector != null)
        //    {
        //        var resolver = selector.Select(response.Data);
        //        resolver.Resolve(Options, Identity, response.Data);
        //    }
        //    else 
        //    {
        //        throw new InvalidOperationException("Can not find a CAS ClaimsResolver.");
        //    }
        //}
    }
}
