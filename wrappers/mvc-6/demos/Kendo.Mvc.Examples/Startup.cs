﻿using Kendo.Mvc.Examples.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using System;
using System.IO;

namespace Kendo.Mvc.Examples
{
    public class Startup
    {
        public IConfigurationBuilder Configuration { get; set; }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            // Fully-qualify configuration path to avoid issues in functional tests. Just "config.json" would be fine
            // but Configuration uses CallContextServiceLocator.Locator.ServiceProvider to get IApplicationEnvironment.
            // Functional tests update that service but not in the static provider.
            var applicationEnvironment = services.BuildServiceProvider().GetRequiredService<IApplicationEnvironment>();
            var configurationPath = Path.Combine(applicationEnvironment.ApplicationBasePath, "config.json");

            // Setup configuration sources.
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationPath)
                .AddEnvironmentVariables();

            // Add EF services to the services container.
            services.AddEntityFramework()
                .AddSqlServer()
				.AddDbContext<SampleEntitiesDataContext>();

            // Add MVC services to the services container.
            services.AddMvc();

			// Add Kendo UI services to the services container
			services.AddKendo();

            // Uncomment the following line to add Web API servcies which makes it easier to port Web API 2 controllers.
            // You need to add Microsoft.AspNet.Mvc.WebApiCompatShim package to project.json
            // services.AddWebApiConventions();

        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.
            // Add the console logger.
            loggerfactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();			

			// Add MVC to the request pipeline.
			app.UseMvc(routes =>
            {
				routes.AddHyphenatedRoute();
				
				routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });				

				// Uncomment the following line to add a route for porting Web API 2 controllers.
				// routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
			});
        }
    }
}
