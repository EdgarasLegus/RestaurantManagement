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
            // FOR MAPPING ENTITIES WITH MODELS
            services.AddAutoMapper(typeof(Startup));

            // FOR LOGGING SETUP
            services.ConfigureLoggerService();

            //FOR IOPTIONS SETUP
            services.Configure<ConfigurationSettings>(Configuration.GetSection(
                                        ConfigurationSettings.InitialData));

            services.AddControllersWithViews();

            services.AddIdentityServer()
                .AddInMemoryClients(new List<Client>())
                .AddInMemoryIdentityResources(new List<IdentityResource>())
                .AddInMemoryApiResources(new List<ApiResource>())
                .AddInMemoryApiScopes(new List<ApiScope>())
                .AddTestUsers(new List<TestUser>())
                .AddDeveloperSigningCredential();

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
