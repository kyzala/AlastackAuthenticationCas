using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Xml;

namespace Alastack.Authentication.Cas;

/// <summary>
/// The CAS 2.0/3.0 implementation of <see cref="ICasTicketValidator"/>.
/// </summary>
public class Cas23TicketValidator : ICasTicketValidator
{
    private const string _ns = "http://www.yale.edu/tp/cas";

    /// <inheritdoc />
    public async Task<AuthenticationTicket?> ValidateAsync(HttpContext context, AuthenticationScheme scheme, CasOptions options, ClaimsPrincipal principal, AuthenticationProperties properties, CasResponse response)
    {
        var data = response.Data;
        //var start = data.IndexOf("<");
        //data = data.Substring(start);

        using var reader = new StringReader(data);
        var doc = new XmlDocument();
        doc.Load(reader);

        var nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("cas", _ns);
        var userNode = doc.DocumentElement.SelectSingleNode("/cas:serviceResponse/cas:authenticationSuccess/cas:user", nsmgr);
        if (userNode != null)
        {
            response.ResponseType = CasResponseType.ServiceValidate;
            response.UserName = userNode.InnerText;
            var attributesNode = doc.DocumentElement.SelectSingleNode("/cas:serviceResponse/cas:authenticationSuccess/cas:attributes", nsmgr);
            if (attributesNode != null)
            {
                foreach (XmlNode childNode in attributesNode.ChildNodes)
                    response.Attributes.Add(childNode.LocalName, childNode.InnerText);
            }

            var casCreatingTicketContext = new CasCreatingTicketContext(context, scheme, options, principal, properties, response);
            casCreatingTicketContext.AppendClaims();
            await options.Events.CreatingTicket(casCreatingTicketContext);
            return new AuthenticationTicket(principal, properties, scheme.Name);
        }

        return null;
    }
}
