using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantManagement.Interfaces.Services;
using RestaurantManagement.Data;
using AutoMapper;
using NLog;
using System.IO;
using RestaurantManagement.BusinessLogic.Services;
using RestaurantManagement.WebApp.Extensions;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces;
using RestaurantManagement.IdentityServer;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;

namespace RestaurantManagement.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityServer DB setup
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // FOR MAPPING ENTITIES WITH MODELS
            services.AddAutoMapper(typeof(Startup));

            // FOR LOGGING SETUP
            services.ConfigureLoggerService();

            //FOR IOPTIONS SETUP
            services.Configure<ConfigurationSettings>(Configuration.GetSection(
                                        ConfigurationSettings.InitialData));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            services.AddControllersWithViews();

            // REGISTER IdentityServer 4 in ASP.NET Core
            services.AddIdentityServer()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddTestUsers(Config.GetUsers())
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(ConfigurationSettings.GetIdentityServerConnectionString(), sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(ConfigurationSettings.GetIdentityServerConnectionString(), sql => sql.MigrationsAssembly(migrationsAssembly));
                });
                

            // ADDING Authentication MiddleWare to the Pipeline
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    // Name of Web Api resource
                    options.ApiName = "RestaurantManagementAPI";
                    // URL on which IdentityServer is running
                    options.Authority = "https://localhost:44370";
                });

            // GLOBAL AUTHORIZATION
            //services.AddMvc().AddMvcOptions(
            //    options => options.Filters.Add(new AuthorizeFilter())
            //    );

            services
                .AddScoped<IDataLoader, DataLoader>()
                .AddScoped<IDishProductService, DishProductService>()
                .AddScoped<IDishService, DishService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IRestaurantTablesService, RestaurantTableService>()
                .AddScoped<IStaffService, StaffService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IOrderItemService, OrderItemService>()
                .AddScoped<ILogicHandler, LogicHandler>()
                .AddScoped<IUserLogService, UserLogService>()
                .AddScoped<IPersonRoleService, PersonRoleService>()
                .AddScoped<IUnitOfMeasurementService, UnitOfMeasurementService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddHttpContextAccessor();

            // Configuration.GetConnectionString("yourconnection sttring"); this way is more clean
            services.AddDbContext<RestaurantManagementCodeFirstContext>(options => options.UseSqlServer(Contracts.Settings.ConfigurationSettings.GetConnectionStringCodeFirst()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();


            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant Management");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
