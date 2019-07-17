using CK.AspNet.Auth;
using CK.DB.AspNet.Auth;
using CK.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using CK.Auth;
using System.Text;
using System.Security.Claims;
using inProjects.WebApp.Controllers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using inProjects.WebApp.Services;
using Microsoft.Extensions.DependencyInjection;
using inProjects.Data;
using inProjects.EmailJury;
using inProjects.WebApp.Hubs;

namespace WebApp
{
    public class Startup
    {
        readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)            
        {
            services.AddCors();
            services.AddSingleton<IWebFrontAuthLoginService, SqlWebFrontAuthLoginService>();
            services.AddSingleton<IAuthenticationTypeSystem, StdAuthenticationTypeSystem>();
            services.AddSingleton<IWebFrontAuthAutoCreateAccountService, AutoCreateAccountService>();

            services.Configure<SpaOptions>(o =>
            {
                o.Host = Configuration["Spa:Host"];
            });
            
            services.AddAuthentication( WebFrontAuthOptions.OnlyAuthenticationScheme )
                .AddOpenIdConnect( "Oidc", options =>
                {
                    options.SignInScheme = WebFrontAuthOptions.OnlyAuthenticationScheme;
                    options.Authority = "https://login.microsoftonline.com/" + Configuration["Authentication:Outlook:TenantId"];
                    options.ClientId = Configuration["Authentication:Outlook:ClientId"];
                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.CallbackPath = "/auth/signin-callback";
                    options.SignedOutRedirectUri = "https://localhost:8080/";
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.Events.OnTicketReceived = c => c.WebFrontAuthRemoteAuthenticateAsync<ICustomUserOidcInfos>( payload =>
                    {
                        payload.Email = c.Principal.FindFirst( ClaimTypes.Name ).Value;
                        payload.FirstName = c.Principal.FindFirst( ClaimTypes.GivenName ).Value;
                        payload.LastName = c.Principal.FindFirst( ClaimTypes.Surname ).Value;
                        payload.SchemeSuffix = "";
                        // Instead of "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                        // Use standard System.Security.Claims.ClaimTypes.
                        payload.Sub = c.Principal.FindFirst( ClaimTypes.NameIdentifier ).Value;
                    } );
                } )
                .AddWebFrontAuth( options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours( 1 );
                    options.SlidingExpirationTime = TimeSpan.FromHours( 1 );
                } )
                .AddCookie();

            if( _env.IsDevelopment() )
            {
                NormalizedPath dllPath = Configuration["StObjMap:Path"];
                if( !dllPath.IsEmptyPath )
                {
                    var solutionPath = new NormalizedPath( AppContext.BaseDirectory ).RemoveLastPart( 4 );
                    dllPath = solutionPath.Combine( dllPath ).AppendPart( "CK.StObj.AutoAssembly.dll" );
                    File.Copy( dllPath, Path.Combine( AppContext.BaseDirectory, "CK.StObj.AutoAssembly.dll" ), overwrite: true );
                }
            }

            services.AddCKDatabase( "CK.StObj.AutoAssembly" );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<Emailer>();
            services.Configure<EmailerOptions>(Configuration.GetSection("gmail"));

            services.AddOptions();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors( c =>
                    c.SetIsOriginAllowed( host => true )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

            app.UseAuthentication();

            app.UseSignalR( routes =>
             {
                 routes.MapHub<StaffMemberHub>( "/StaffMemberHub" );
                 routes.MapHub<ForumHub>( "/ForumHub" );
             } );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
        }
    }
}
