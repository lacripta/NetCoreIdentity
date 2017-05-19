using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MvcClient {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional : true, reloadOnChange : true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional : true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddAuthorization(options => {
                options.AddPolicy("SMS", policyAdmin => {
                    // policyAdmin.RequireClaim("role", "admin");
                    policyAdmin.Requirements.Add(new AuthorizationRequirement());
                });
            });
            services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationScheme = "Cookies",
                    CookieDomain = ".elibomdevelopment.com"
                //DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"c:\shared-auth-ticket-keys\"))
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions {
                AuthenticationScheme = "oidc",
                    SignInScheme = "Cookies",

                    Authority = "http://elibomdevelopment.com:5000",
                    RequireHttpsMetadata = false,

                    ClientId = "mvc",
                    ClientSecret = "secret",

                    ResponseType = "code id_token",
                    Scope = { "api1", "offline_access", "profile.customer" },

                    GetClaimsFromUserInfoEndpoint = true,
                    SaveTokens = true
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}