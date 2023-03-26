using Microsoft.AspNetCore.Authentication;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Default values for CAS authentication.
/// </summary>
public static class CasDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "CAS";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public const string DisplayName = "Central Authentication Service";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public const string Issuer = "CAS";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public const string CallbackPath = "/signin-cas";

    /// <summary>
    /// CAS protocol version. Default value for <see cref="CasOptions.ProtocolVersion"/>.
    /// </summary>
    public const string ProtocolVersion = "2.0";

    /// <summary>
    /// Credential requestor / acceptor. Default value for <see cref="CasOptions.LoginPath"/>.
    /// </summary>
    public const string LoginPath = "/login";
    /// <summary>
    /// Destroy CAS session (logout). Default value for <see cref="CasOptions.LogoutPath"/>.
    /// </summary>
    public const string LogoutPath = "/logout";
    /// <summary>
    /// Service ticket validation.
    /// </summary>
    public const string ValidatePath = "/validate";
    /// <summary>
    /// Service ticket validation [CAS 2.0]
    /// </summary>
    public const string ServiceValidatePath = "/serviceValidate";
    /// <summary>
    /// Service/proxy ticket validation [CAS 2.0]
    /// </summary>
    public const string ProxyValidatePath = "/proxyValidate";
    /// <summary>
    /// Proxy ticket service [CAS 2.0]
    /// </summary>
    public const string ProxyPath = "/proxy";
    /// <summary>
    /// Service ticket validation [CAS 3.0]
    /// </summary>
    public const string P3ServiceValidatePath = "/p3/serviceValidate";
    /// <summary>
    /// Service/proxy ticket validation [CAS 3.0]
    /// </summary>
    public const string P3ProxyValidatePath = "/p3/proxyValidate";
}
