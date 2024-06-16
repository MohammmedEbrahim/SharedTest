using Application;
using Application.Helpers;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

namespace SharedZone.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration , IHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            #region JWT_AuthenticationScheme

            services.Configure<JWT>(Configuration.GetSection("JWT"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddJwtBearer(o =>                // jwt scheme with authentication
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true ,               // validate for issuerSignKey (Secret Key)
                    ValidateIssuer = true ,                         // validate for Issuer Value
                    ValidateAudience = true ,                       // validate for Audience value 
                    ValidateLifetime = true ,                       // validate lifetime of token
                    ValidIssuer = Configuration["JWT:Issuer"] ,     // where he will get data and work with validate with it
                    ValidAudience = Configuration["JWT:Audience"] ,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])) ,
                    ClockSkew = TimeSpan.Zero                      // when lifetime end doesn't allow for token to Continue
                };
            });

            #endregion

            #region Resources_Localization

            services.AddLocalization(options => options.ResourcesPath = @"Application\Resourses");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo ("en"),
                    new CultureInfo ("ar")
                };
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
                options.SupportedCultures = supportedCultures;
            });

            #endregion

            services.AddPersistenceServices(Configuration);

            services.AddApplicationServices();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1" , new OpenApiInfo { Title = "SharedZone" , Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            #region Hangfire

            /*services.AddHangfire(x =>
                              x.UseSqlServerStorage(Configuration.GetConnectionString("DBConnect")));

            services.AddHangfireServer();*/

            #endregion
        }

        public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json" , "Web.Server v1"));
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

        }
    }
}