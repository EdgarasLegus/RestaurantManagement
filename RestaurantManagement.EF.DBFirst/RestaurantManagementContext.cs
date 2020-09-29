using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RestaurantManagement.Contracts.Entities;

namespace RestaurantManagement.EF.DBFirst.ModelsDB
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

        public virtual DbSet<DishProducts> DishProducts { get; set; }
        public virtual DbSet<Dishes> Dishes { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<RestaurantTables> RestaurantTables { get; set; }
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
            modelBuilder.Entity<DishProducts>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Portion).HasColumnType("decimal(19, 3)");

                entity.HasOne(d => d.Dish)
                    .WithMany()
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DishProdu__DishI__29572725");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DishProdu__Produ__2A4B4B5E");
            });

            modelBuilder.Entity<Dishes>(entity =>
            {
                entity.Property(e => e.DishName).IsRequired();

                entity.Property(e => e.DishType).IsRequired();
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Dish)
                    .WithMany()
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__DishI__2F10007B");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__2E1BDC42");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.OrderName).IsRequired();
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.StockAmount).HasColumnType("decimal(19, 3)");

                entity.Property(e => e.UnitOfMeasure).IsRequired();
            });

            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.Property(e => e.ReservationPersonName).IsRequired();

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.TableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__Table__33D4B598");
            });

            modelBuilder.Entity<RestaurantTables>(entity =>
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
