using System.Security.Claims;

namespace Alastack.Authentication.Cas;

/// <summary>
/// An interface used to select attributes from the CAS validation response and create Claims.
/// </summary>
public interface ICasClaimsFilter
{
    /// <summary>
    /// Select attributes and create Claims.
    /// </summary>
    /// <param name="options"><see cref="CasOptions"/></param>
    /// <param name="identity"><see cref="ClaimsIdentity"/></param>
    /// <param name="response"><see cref="CasResponse"/></param>
    void Select(CasOptions options, ClaimsIdentity identity, CasResponse response);
}







