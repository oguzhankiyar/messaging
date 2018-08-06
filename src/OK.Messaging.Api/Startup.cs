using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OK.Messaging.Api.Filters;
using OK.Messaging.Api.Middlewares;
using OK.Messaging.Core.Logging;
using OK.Messaging.DataAccess;
using OK.Messaging.Engine;
using System.Text;

namespace OK.Messaging.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                        };
                    });

            services.AddDataAccessLayer(_configuration.GetConnectionString("MessagingConnection"));

            services.AddEngineLayer();

            services.AddMvc((options) =>
                    {
                        options.Filters.Add(new ValidateModelActionFilter());
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            logger.SetGlobalProperty("ConnectionString", _configuration.GetConnectionString("MessagingConnection"));
            logger.SetGlobalProperty("Channel", "OK.Messaging.Api");

            app.UseAuthentication();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestInformationMiddleware>();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}