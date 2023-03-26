using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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

                // OptionsValidationException: Cookie.Expiration is ignored, use ExpireTimeSpan instead.
                
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
                //options.ServiceValidatePath = Configuration["CAS:ServiceValidatePath"];
                //options.ProxyValidatePath = Configuration["CAS:ProxyValidatePath"];

                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}