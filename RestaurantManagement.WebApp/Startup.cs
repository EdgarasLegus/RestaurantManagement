using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantManagement.Contracts.Interfaces.Repositories;
using RestaurantManagement.Contracts.Interfaces.Services;
using RestaurantManagement.Data;

namespace RestaurantManagement.WebApp
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
            services.AddControllersWithViews();

            services.AddIdentityServer()
                .AddInMemoryClients(new List<Client>())
                .AddInMemoryIdentityResources(new List<IdentityResource>())
                .AddInMemoryApiResources(new List<ApiResource>())
                .AddInMemoryApiScopes(new List<ApiScope>())
                .AddTestUsers(new List<TestUser>())
                .AddDeveloperSigningCredential();

            services
                .AddScoped<IDataLoader, BusinessLogic.Services.DataLoader>()
                .AddScoped<IDishProductService, BusinessLogic.Services.DishProductService>()
                .AddScoped<IDishService, BusinessLogic.Services.DishService>()
                .AddScoped<IProductService, BusinessLogic.Services.ProductService>()
                .AddScoped<IRestaurantTablesService, BusinessLogic.Services.RestaurantTableService>()
                .AddScoped<IStaffService, BusinessLogic.Services.StaffService>()
                .AddScoped<IDishRepo, BusinessLogic.Repository.DishRepository>()
                .AddScoped<IDishProductRepo, BusinessLogic.Repository.DishProductRepository>()
                .AddScoped<IProductRepo, BusinessLogic.Repository.ProductRepository>()
                .AddScoped<IRestaurantTableRepo, BusinessLogic.Repository.RestaurantTableRepository>()
                .AddScoped<IStaffRepo, BusinessLogic.Repository.StaffRepository>()
                .AddScoped<ILogicHandler, BusinessLogic.Services.LogicHandler>()
                .AddScoped<IUserLogRepo, BusinessLogic.Repository.UserLogRepository>()
                .AddScoped<IPersonRoleService, BusinessLogic.Services.PersonRoleService>()
                .AddScoped<IPersonRoleRepo, BusinessLogic.Repository.PersonRoleRepository>()
                .AddHttpContextAccessor();

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
