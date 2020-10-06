using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Data
{
    public partial class RestaurantManagementCodeFirstContext : DbContext
    {
        public RestaurantManagementCodeFirstContext()
        {
        }

        public RestaurantManagementCodeFirstContext(DbContextOptions<RestaurantManagementCodeFirstContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<DishProduct> DishProduct { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<RestaurantTable> RestaurantTable { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationSettings.GetConnectionStringCodeFirst());
            }
        }
    }
}
