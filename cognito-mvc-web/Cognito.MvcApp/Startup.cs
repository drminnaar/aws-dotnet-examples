using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Cognito.MvcApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment env)
        {
            _environment = env ?? throw new ArgumentNullException(nameof(env));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCognitoIdentity();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounts/SignIn";
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                const string ErrorHandlingPath = "/Errors/Index";
                app.UseExceptionHandler(ErrorHandlingPath);
            }

            app.UseStaticFiles();
            app.UseNodeModules();

            const string PathFormat = "/errors/{0}";
            app.UseStatusCodePagesWithReExecute(PathFormat);
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
