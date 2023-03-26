using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace Alastack.Authentication.Cas;

/// <summary>
/// Used to setup defaults for the CasOptions.
/// </summary>
public class CasPostConfigureOptions : IPostConfigureOptions<CasOptions>
{
    private readonly IDataProtectionProvider _dp;

    /// <summary>
    /// Initializes the <see cref="CasPostConfigureOptions"/>.
    /// </summary>
    /// <param name="dataProtection">The <see cref="IDataProtectionProvider"/>.</param>
    public CasPostConfigureOptions(IDataProtectionProvider dataProtection)
    {
        _dp = dataProtection;
    }

    /// <summary>
    /// Invoked to post configure a CasOptions instance.
    /// </summary>
    /// <param name="name">The name of the CasOptions instance being configured.</param>
    /// <param name="options">The CasOptions instance to configure.</param>
    public void PostConfigure(string name, CasOptions options)
    {
        options.DataProtectionProvider ??= _dp;

        if (options.StateDataFormat == null)
        {
            var dataProtector = options.DataProtectionProvider.CreateProtector(typeof(CasHandler).FullName!, name, "v1");
            options.StateDataFormat = new PropertiesDataFormat(dataProtector);
        }

        if (options.Backchannel == null)
        {
            options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
            options.Backchannel.Timeout = options.BackchannelTimeout;
            options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            options.Backchannel.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core CAS handler");
            options.Backchannel.DefaultRequestHeaders.ExpectContinue = false;
        }

        options.TicketValidator ??= new CompositeCasTicketValidator();

        options.ClaimsFilter ??= new DefaultCasClaimsFilter();

        switch (options.ProtocolVersion) 
        {
            case "2.0":
                if (String.IsNullOrEmpty(options.ServiceValidatePath))
                {
                    options.ServiceValidatePath = CasDefaults.ServiceValidatePath;
                }
                if (String.IsNullOrEmpty(options.ProxyValidatePath))
                {
                    options.ProxyValidatePath = CasDefaults.ProxyValidatePath;
                }
                break;
            case "3.0":
                if (String.IsNullOrEmpty(options.ServiceValidatePath))
                {
                    options.ServiceValidatePath = CasDefaults.P3ServiceValidatePath;
                }
                if (String.IsNullOrEmpty(options.ProxyValidatePath))
                {
                    options.ProxyValidatePath = CasDefaults.P3ProxyValidatePath;
                }
                break;
            case "1.0":
                if (String.IsNullOrEmpty(options.ServiceValidatePath))
                {
                    options.ServiceValidatePath = CasDefaults.ValidatePath;
                }
                break;
        }
        
    }
}
