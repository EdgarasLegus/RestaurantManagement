using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RestaurantManagement.Contracts.Entities;

namespace RestaurantManagement.Data.ModelsDB
{
    public partial class RestaurantManagementContext : DbContext
    {
        public RestaurantManagementContext()
        {
        }

        public RestaurantManagementContext(DbContextOptions<RestaurantManagementContext> options)
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LT-LIT-SC-0513;Database=RestaurantManagement;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.DishName).IsRequired();

                entity.Property(e => e.DishType).IsRequired();
            });

            modelBuilder.Entity<DishProduct>(entity =>
            {
                entity.Property(e => e.Portion).HasColumnType("decimal(19, 3)");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishProduct)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DishProdu__DishI__49C3F6B7");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DishProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DishProdu__Produ__4AB81AF0");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderName).IsRequired();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__DishI__4E88ABD4");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__4D94879B");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.StockAmount).HasColumnType("decimal(19, 3)");

                entity.Property(e => e.UnitOfMeasure).IsRequired();
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.ReservationPersonName).IsRequired();

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.TableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__Table__33D4B598");
            });

            modelBuilder.Entity<RestaurantTable>(entity =>
            {
                entity.Property(e => e.TableName).IsRequired();
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.UserName).IsRequired();

                entity.Property(e => e.UserPassword).IsRequired();

                entity.Property(e => e.UserRole).IsRequired();
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(e => e.UserAction).IsRequired();

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.UserLog)
                    .HasForeignKey(d => d.ModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserLog__Modifie__36B12243");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
