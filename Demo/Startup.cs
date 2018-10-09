
using System;
using System.Reflection;
using Demo.Core.Commands;
using Demo.Core.Interfaces;
using Demo.Infrastructure;
using Demo.Infrastructure.Repository;
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
           
            var connectionString = Startup.Configuration["ConnectionStrings:demoConnection"];
            services.AddDbContext<DemoContext>(o => o.UseSqlServer(connectionString,
                x => x.MigrationsAssembly(typeof(DemoContext).GetTypeInfo().Assembly.GetName().Name)));

            services.AddScoped<IFacilityRepository,FacilityRepository>();
            services.AddMediatR(typeof(SaveFacility));

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider provider)
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
            
            EnsureMigrationOfContext<DemoContext>(provider);
            Log.Debug("Startup OK");
        }
        
        public static void EnsureMigrationOfContext<T>(IServiceProvider app) where T : DbContext
        {
            var contextName = typeof(T).Name;
            Log.Debug($"initializing Database context: {contextName}");
            var context = app.GetService<T>();
            try
            {
                context.Database.Migrate();
                Log.Debug($"initializing Database context: {contextName} [OK]");
            }
            catch (Exception e)
            {
                Log.Debug($"initializing Database context: {contextName} Error");
                Log.Debug($"{e}");
            }
        }
    }
}