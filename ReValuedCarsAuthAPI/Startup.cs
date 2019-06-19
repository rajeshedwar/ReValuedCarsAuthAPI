using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReValuedCarsAuthAPI.Infrastructure;
using ReValuedCarsAuthAPI.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace ReValuedCarsAuthAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection"));
            });
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(config =>
                {
                    config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Authentication API",
                        Description = "Token Generation API",
                        Contact = new Contact { Email="Rajeshj2@hexaware.com", Name="Rajesh",Url="http://rajesh.hexaware.com" },
                        Version="1.0",
                        TermsOfService="Copywrighted by Hexaware Inc"
                    });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.RoutePrefix = "";
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Authenticate API");
            });

            app.UseMvc();
        }
    }
}
