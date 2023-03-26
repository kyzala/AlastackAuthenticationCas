# Alastack.Authentication.Cas

A .NET client supports Central Authentication Service ([CAS](https://apereo.github.io/cas/6.6.x/protocol/CAS-Protocol.html)).

[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/kyzala/AlastackAuthenticationCas/dotnet.yml?branch=main)](https://github.com/kyzala/AlastackAuthenticationCas/actions/workflows/dotnet.yml)[![GitHub](https://github.com/kyzala/AlastackAuthenticationCas/blob/main/LICENSE)](LICENSE)[![Nuget](https://img.shields.io/nuget/v/Alastack.Authentication.Cas)](https://www.nuget.org/packages/Alastack.Authentication.Cas)

## Getting started

### Install package from the .NET CLI

```
dotnet add package Alastack.Authentication.Cas
```

### Dependency Injection

`AuthenticationBuilder` extension methods to configure CAS authentication.

```c#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
	options.LoginPath = "/signin";
	options.LogoutPath = "/signout";
	options.SlidingExpiration = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
	options.Cookie.Name = ".app.cookies";    
	//options.Cookie.MaxAge = TimeSpan.FromMinutes(1);
	//options.Cookie.Expiration = TimeSpan.FromMinutes(1);
})
.AddCas(options =>
{
	options.Service = builder.Configuration["CAS:Service"]!;
	options.Server = builder.Configuration["CAS:Server"]!;
	options.SaveTokens = true;
	//options.ProtocolVersion = Configuration["ProtocolVersion"];
	//options.LoginPath = Configuration["CAS:LoginPath"];                
	//options.LogoutPath = Configuration["CAS:LogoutPath"];
	//options.ServiceValidatePath = Configuration["CAS:ServiceValidePath"];
	//options.ProxyValidatePath = Configuration["CAS:ProxyValidatePath"];
	//options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default",	pattern: "controller=Home}/{action=Index}/{id?}");
app.Run();
```

### Configuration

Using json configuration. 

```JSON
{
  "CAS": {
    "Service": "[CAS integration app uri]", //https://www.somecasservice.io
    "Server": "[CAS service uri]", //https://www.somecasserver.io/cas
    "ProtocolVersion": "3.0",
    "LoginPath": "/login",
    "LogoutPath": "/logout",
    "ServiceValidatePath": "/serviceValidate",
    "ProxyValidatePath": "/proxyValidate"
  }
}
```

## CasOptions

The `CasOptions` object provides properties that connects to the CAS server.

| Property              | Description                                                  | Default                       |
| --------------------- | ------------------------------------------------------------ | ----------------------------- |
| `Service`             |                                                              | `null`                        |
| `Renew`               | if this parameter is set, single sign-on will be bypassed. In this case, CAS will require the client to present credentials regardless of the existence of a single sign-on session with CAS. This parameter is not compatible with the `gateway` parameter. | `false`                       |
| `Gateway`             | if this parameter is set, CAS will not ask the client for credentials. If the client has a pre-existing single sign-on session with CAS, or if a single sign-on session can be established through non-interactive means (i.e. trust authentication), CAS MAY redirect the client to the URL specified by the `service` parameter, appending a valid service ticket. (CAS also MAY interpose an advisory page informing the client that a CAS authentication has taken place.) If the client does not have a single sign-on session with CAS, and a non-interactive authentication cannot be established, CAS MUST redirect the client to the URL specified by the `service` parameter with no “ticket” parameter appended to the URL. If the `service` parameter is not specified and `gateway` is set, the behavior of CAS is undefined. | `false`                       |
| `Server`              | CAS Server Uri.                                              | `null`                        |
| `ProtocolVersion`     | CAS protocol version.                                        | `2.0`                         |
| `LoginPath`           | Credential requestor / acceptor.                             | `/login`                      |
| `LogoutPath`          | Destroy CAS session (logout).                                | `/logout`                     |
| `ServiceValidatePath` | Service ticket validation                                    | `null`                        |
| `ProxyValidatePath`   | Service/proxy ticket validation                              | `null`                        |
| `ClaimsFilter`        | Used to select values from the CAS validation response and create Claims. | `DefaultCasClaimsFilter`      |
| `TicketValidator`     | Checks the validity of a service ticket and returns the AuthenticationTicket. | `CompositeCasTicketValidator` |
| `StateDataFormat`     | Gets or sets the type used to secure data handled by the middleware. | `PropertiesDataFormat`        |

More information: [CAS - CAS Protocol Specification](https://apereo.github.io/cas/6.6.x/protocol/CAS-Protocol-Specification.html#)

## Samples

Visit the [samples](https://github.com/kyzala/AlastackAuthenticationCas/tree/main/samples) folder.