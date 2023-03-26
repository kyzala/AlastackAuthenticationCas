// https://apereo.github.io/cas/6.6.x/protocol/CAS-Protocol-Specification.html#252-response

namespace Alastack.Authentication.Cas;

/// <summary>
/// CAS validation Response.
/// </summary>
public class CasResponse
{
    public CasResponseType ResponseType { get; set; } = CasResponseType.Unspecified;

    public string Data { get; }

    /// <summary>
    ///  Cas service response user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Cas service response proxyGrantingTicket.
    /// </summary>
    public string ProxyGrantingTicket { get; set; }

    public IList<string> Proxies { get; }

    public string ProxyTicket { get; set; }

    public string FailureCode { get; set; }

    public string Error { get; set; }

    public IDictionary<string, string> Attributes { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="CasResponse"/>.
    /// </summary>
    /// <param name="data">response raw data</param>
    public CasResponse(string data)
    {
        Data = data;
        Proxies = new List<string>();
        Attributes = new Dictionary<string, string>();
    }
}
