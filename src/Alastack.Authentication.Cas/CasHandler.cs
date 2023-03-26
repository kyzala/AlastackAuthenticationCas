using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Authentication handler for CAS based authentication.
/// </summary>
public class CasHandler: RemoteAuthenticationHandler<CasOptions>
{
    /// <summary>
    /// The handler calls methods on the events which give the application control at certain points where processing is occurring. 
    /// If it is not provided a default instance is supplied which does nothing when the methods are called.
    /// </summary>
    protected new CasEvents Events
    {
        get { return (CasEvents)base.Events; }
        set { base.Events = value; }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CasHandler"/>.
    /// </summary>
    /// <inheritdoc />
    public CasHandler(IOptionsMonitor<CasOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {            
    }

    /// <summary>
    /// Creates a new instance of the events instance.
    /// </summary>
    /// <returns>A new instance of the events instance.</returns>
    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new CasEvents());

    /// <inheritdoc />
    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync() 
    {        
        var state = Request.Query["state"];
        var properties = Options.StateDataFormat.Unprotect(state);
        if (properties == null)
        {
            return HandleRequestResult.Fail("The state was missing or invalid.");
        }
        
        if (!ValidateCorrelationId(properties))
        {
            return HandleRequestResult.Fail("Correlation failed.", properties);
        }

        var ticket = Request.Query["ticket"];
        if (string.IsNullOrEmpty(ticket))
        {
            return HandleRequestResult.Fail("Ticket was not found.", properties);
        }
        
        var identity = new ClaimsIdentity(ClaimsIssuer);

        var authTicket = await CreateTicketAsync(identity, properties, ticket, state);
        if (authTicket != null)
        {
            return HandleRequestResult.Success(authTicket);
        }
        return HandleRequestResult.Fail("Failed to retrieve user information from remote server.", properties);
    }

    /// <summary>
    /// Creates an <see cref="AuthenticationTicket"/> from the specified <paramref name="ticket"/>.
    /// </summary>
    /// <param name="identity">The <see cref="ClaimsIdentity"/>.</param>
    /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
    /// <param name="ticket">The CAS ticket.</param>
    /// <returns>The <see cref="AuthenticationTicket"/>.</returns>
    protected virtual async Task<AuthenticationTicket?> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, string ticket, string state)
    {
        var serviceUrl = BuildServiceUri(state);
        var parameters = new Dictionary<string, string>
        {            
            { "service", serviceUrl },
            { "ticket", ticket }
        };
        var validateUrl = QueryHelpers.AddQueryString(Options.Server + Options.ServiceValidatePath, parameters!);

        var request = new HttpRequestMessage(HttpMethod.Get, validateUrl);

        var response = await Options.Backchannel.SendAsync(request, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"An error occurred when retrieving user information: {response.StatusCode}.");
        }
        var payload = await response.Content.ReadAsStringAsync();
        var casResponse = new CasResponse(payload);
        var authenticationTicket = await Options.TicketValidator.ValidateAsync(Context, Scheme, Options, new ClaimsPrincipal(identity), properties, casResponse);
                
        return authenticationTicket;
    }

    /// <inheritdoc />
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        if (string.IsNullOrEmpty(properties.RedirectUri))
        {
            //properties.RedirectUri = OriginalPathBase + OriginalPath + Request.QueryString;
            properties.RedirectUri = CurrentUri;
        }
        
        GenerateCorrelationId(properties);

        var state = Options.StateDataFormat.Protect(properties);
        var authorizationEndpoint = BuildChallengeUri(state);

        var redirectContext = new RedirectContext<CasOptions>(Context, Scheme, Options, properties, authorizationEndpoint);
        await Events.RedirectToAuthorizationEndpoint(redirectContext);

        var location = Context.Response.Headers[HeaderNames.Location];
        if (location == StringValues.Empty)
        {
            location = "(not set)";
        }
        var cookie = Context.Response.Headers[HeaderNames.SetCookie];
        if (cookie == StringValues.Empty)
        {
            cookie = "(not set)";
        }
        Logger.HandleChallenge(location, cookie);
    }

    /// <summary>
    /// Build challenge uri.
    /// </summary>
    /// <param name="state">State value.</param>
    /// <returns>Challenge uri string.</returns>
    protected virtual string BuildChallengeUri(string state)
    {  
        var servicUrl = BuildServiceUri(state);

        var parameters = new Dictionary<string, string>
        {            
            { "service", servicUrl }                
        };

        if (Options.Renew)
        {
            parameters["renew"] = "true";
        }

        if (Options.Gateway)
        {
            parameters["gateway"] = "true";
        }

        return QueryHelpers.AddQueryString(Options.Server + Options.LoginPath, parameters!);
    }

    /// <summary>
    /// Build service uri.
    /// </summary>
    /// <param name="state">State value.</param>
    /// <returns>Service uri string.</returns>
    protected string BuildServiceUri(string state)
    {   
        if (String.IsNullOrEmpty(Options.Service))
        {
            return $"{Request.Scheme}://{Request.Host}{OriginalPathBase}{Options.CallbackPath}?state={state}";
        }
        return $"{Options.Service}{OriginalPathBase}{Options.CallbackPath}?state={state}";
    }        
}
