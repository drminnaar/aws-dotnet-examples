using Amazon.CognitoIdentityProvider;
using Cognito.MvcApi.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Cognito.MvcApi
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Setup AWS configuration and AWS Cognito Identity
            var defaultOptions = _configuration.GetAWSOptions();
            var cognitotOptions = _configuration.GetAWSCognitoClientOptions();
            services.AddDefaultAWSOptions(defaultOptions);
            services.AddSingleton(cognitotOptions);
            services.AddSingleton(new CognitoClientSecret(cognitotOptions));
            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            // Setup authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var authority = $"https://cognito-idp.us-east-1.amazonaws.com/{cognitotOptions.UserPoolId}";
                    var audience = cognitotOptions.UserPoolClientId;

                    options.Audience = audience;
                    options.Authority = authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authority,
                        ValidAudience = audience,
                        IssuerSigningKey = new CognitoSigningKey(cognitotOptions.UserPoolClientSecret).ComputeKey()
                    };
                });

            // Setup CORS
            services.AddCors(setup =>
            {
                setup.AddPolicy("OpenSeason", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyOrigin();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors("OpenSeason");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
