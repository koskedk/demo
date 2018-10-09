using System;
using System.Reflection;
using Demo.Core.Commands;
using Demo.Infrastructure;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Demo
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMediatR(typeof(SaveFacility));
            
            var connectionString = Startup.Configuration["ConnectionStrings:eticaConnection"];
            
            services.AddDbContext<DemoContext>(o => o.UseSqlServer(connectionString,
                x => x.MigrationsAssembly(typeof(DemoContext).GetTypeInfo().Assembly.GetName().Name)));

            try
            {
                services.AddHangfire(o => o.UseSqlServerStorage(connectionString));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Startup Error on HangFire");
            }
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            
            try
            {
                app.UseHangfireDashboard("/api/hangfire");
                app.UseHangfireServer();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Hangfire is down !");
            }
            
            Log.Debug("Startup OK");
        }
    }
}