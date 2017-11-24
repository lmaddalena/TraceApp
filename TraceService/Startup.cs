using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Repository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace TraceService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DataContext>(options =>             
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMemoryCache();

            services.AddCors();

            services.AddMvc();

            // AddMvcCore is the same of AddMvc but doesn't have Razor's support.
            // if you build WebAPI AddMvcCore is a good choice
            //services.AddMvcCore();


            // add application services
            services.AddTransient<ITraceRepository, TraceRepository>();
            services.AddTransient<IOriginsRepository, OriginsRepository>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            // add global exception handler
            app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    context.Response.StatusCode = 500; 
                    context.Response.ContentType ="application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if(error != null)
                    {
                        var ex = error.Error;

                        //await context.Response.WriteAsync(ex.Message, Encoding.UTF8); // must be json formatted
                        await context.Response.WriteAsync("{ \"Error\": \"Unhandled exception occurred\" }", Encoding.UTF8);
                    }
                });
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost"));

            app.UseMvc();
        }

    }
}
