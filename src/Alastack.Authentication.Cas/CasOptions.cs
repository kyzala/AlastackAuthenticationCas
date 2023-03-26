using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Defines a set of options used by <see cref="CasHandler"/>.
/// </summary>
public class CasOptions : RemoteAuthenticationOptions
{
    /// <summary>
    /// Initializes a new instance of <see cref="CasOptions"/>.
    /// </summary>
    public CasOptions() 
    {
        ClaimsIssuer = CasDefaults.Issuer;
        CallbackPath = CasDefaults.CallbackPath;
        ProtocolVersion = CasDefaults.ProtocolVersion;
        
        LoginPath = CasDefaults.LoginPath;
        LogoutPath = CasDefaults.LogoutPath;
        Events = new CasEvents();
    }

    /// <summary>
    /// the identifier of the application the client is trying to access. 
    /// <para>
    /// In almost all cases, this will be the URL of the application. 
    /// Note that as an HTTP request parameter, this URL value MUST be URL-encoded as described in Section 2.2 of RFC 1738 [4]. 
    /// If a service is not specified and a single sign-on session does not yet exist, CAS SHOULD request credentials from the user to initiate a single sign-on session. 
    /// If a service is not specified and a single sign-on session already exists, CAS SHOULD display a message notifying the client that it is already logged in.
    /// </para>
    /// </summary>
    public string Service { get; set; } = default!;

    /// <summary>
    /// if this parameter is set, single sign-on will be bypassed.
    /// In this case, CAS will require the client to present credentials regardless of the existence of a single sign-on session with CAS. 
    /// </summary>
    /// <remarks>
    /// This parameter is not compatible with the “gateway” parameter.
    /// Services redirecting to the /login URI and login form views posting to the /login URI SHOULD NOT set both the “renew” and “gateway” request parameters. 
    /// Behavior is undefined if both are set.It is RECOMMENDED that CAS implementations ignore the “gateway” parameter if “renew” is set.
    /// It is RECOMMENDED that when the renew parameter is set itsvalue be “true”.
    /// </remarks>
    public bool Renew { get; set; }

    /// <summary>
    /// if this parameter is set, CAS will not ask the client for credentials.
    /// If the client has a pre-existing single sign-on session with CAS, or if a single sign-on session can be established through non-interactive means(i.e.trust authentication), 
    /// CAS MAY redirect the client to the URL specified by the “service” parameter, appending a valid service ticket. 
    /// (CAS also MAY interpose an advisory page informing the client that a CAS authentication has taken place.) If the client does not have a single sign-on session with CAS, 
    /// and a non-interactive authentication cannot be established, CAS MUST redirect the client to the URL specified by the “service” parameter with no “ticket” parameter appended to the URL.
    /// If the “service” parameter is not specified and “gateway” is set, the behavior of CAS is undefined.
    /// It is RECOMMENDED that in this case, CAS request credentials as if neither parameter was specified.
    /// </summary>
    /// <remarks>
    /// This parameter is not compatible with the “renew” parameter.
    /// Behavior is undefined if both are set.
    /// It is RECOMMENDED that when the gateway parameter is set its value be “true”.
    /// </remarks>
    public bool Gateway { get; set; }

    /// <summary>
    /// CAS Server Uri.
    /// </summary>
    public string Server { get; set; } = default!;

    /// <summary>
    /// CAS protocol version.
    /// </summary>
    public string ProtocolVersion { get; set; } = CasDefaults.ProtocolVersion;

    /// <summary>
    /// Credential requestor / acceptor.
    /// </summary>
    public PathString LoginPath { get; set; } = CasDefaults.LoginPath;

    /// <summary>
    /// Destroy CAS session (logout).
    /// </summary>
    public PathString LogoutPath { get; set; } = CasDefaults.LogoutPath;

    /// <summary>
    /// Service ticket validation
    /// </summary>
    public PathString ServiceValidatePath { get; set; } = default!;

    /// <summary>
    /// Service/proxy ticket validation
    /// </summary>
    public PathString ProxyValidatePath { get; set; } = default!;


    /// <summary>
    /// Gets or sets the <see cref="CasEvents"/> used to handle authentication events.
    /// </summary>
    public new CasEvents Events
    {
        get => (CasEvents)base.Events;
        set => base.Events = value;
    }

    /// <summary>
    /// Used to select attributes from the CAS validation response and create Claims.
    /// </summary>
    public ICasClaimsFilter ClaimsFilter { get; set; } = default!;    

    /// <summary>
    /// Checks the validity of a service ticket and returns the <see cref="AuthenticationTicket"/>.
    /// </summary>
    public ICasTicketValidator TicketValidator { get; set; } = default!;

    /// <summary>
    /// Used by <see cref="CasHandler"/> to validate the CAS ticket and return an AuthenticationTicket with the user's CAS identity.
    /// </summary>
    ///public ICasTicketValidator TicketValidator { get; set; }

    /// <summary>
    /// Gets or sets the type used to secure data handled by the middleware.
    /// </summary>
    public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; } = default!;

    /// <summary>
    /// Check that the options are valid.  Should throw an exception if things are not ok.
    /// </summary>
    public override void Validate()
    {
        base.Validate();

        if (Renew && Gateway)
        {
            throw new ArgumentException($"The Gateway parameter is not compatible with the Renew parameter.");
        }

        if (string.IsNullOrEmpty(Server))
        {
            throw new ArgumentException("Options.Server must be provided.", nameof(Server));
        }

        if (string.IsNullOrEmpty(LoginPath))
        {
            throw new ArgumentException("Options.LoginEndpoint must be provided.", nameof(LoginPath));
        }

        if (string.IsNullOrEmpty(LogoutPath))
        {
            throw new ArgumentException("Options.LogoutPath must be provided.", nameof(LogoutPath));
        }

        if (string.IsNullOrEmpty(ServiceValidatePath))
        {
            throw new ArgumentException("Options.ValidationEndpoint must be provided.", nameof(ServiceValidatePath));
        }

        //if (string.IsNullOrEmpty(ProxyValidatePath))
        //{
        //    throw new ArgumentException("Options.ProxyValidatePath must be provided", nameof(ProxyValidatePath));
        //}

    }
}
