using Alastack.Authentication.Cas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to configure CAS authentication.
/// </summary>
public static class CasExtensions
{
    /// <summary>
    /// Adds CAS based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
    /// The default scheme is specified by <see cref="CasDefaults.AuthenticationScheme"/>.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddCas(this AuthenticationBuilder builder)
        => builder.AddCas(CasDefaults.AuthenticationScheme, _ => { });

    /// <summary>
    /// Adds CAS based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
    /// The default scheme is specified by <see cref="CasDefaults.AuthenticationScheme"/>.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="CasOptions"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddCas(this AuthenticationBuilder builder, Action<CasOptions> configureOptions)
        => builder.AddCas(CasDefaults.AuthenticationScheme, configureOptions);

    /// <summary>
    /// Adds CAS based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
    /// The default scheme is specified by <see cref="CasDefaults.AuthenticationScheme"/>.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="authenticationScheme">The authentication scheme.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="CasOptions"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddCas(this AuthenticationBuilder builder, string authenticationScheme, Action<CasOptions> configureOptions)
        => builder.AddCas(authenticationScheme, CasDefaults.DisplayName, configureOptions);

    /// <summary>
    /// Adds CAS based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
    /// The default scheme is specified by <see cref="CasDefaults.AuthenticationScheme"/>.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <param name="authenticationScheme">The authentication scheme.</param>
    /// <param name="displayName">A display name for the authentication handler.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="CasOptions"/>.</param>
    /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
    public static AuthenticationBuilder AddCas(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<CasOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CasOptions>, CasPostConfigureOptions>());
        //builder.Services.AddSingleton<ICasClaimsSelector, DefaultCasClaimsSelector>();
        //builder.Services.AddSingleton<ICasResponseFactory, CasResponseFactory>();
        return builder.AddRemoteScheme<CasOptions, CasHandler>(authenticationScheme, displayName, configureOptions);
    }
}
