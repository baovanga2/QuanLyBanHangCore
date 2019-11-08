using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;

namespace QuanLyBanHangCore.Models
{
    public class QuanLyBanHangCoreContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public QuanLyBanHangCoreContext (DbContextOptions<QuanLyBanHangCoreContext> options)
            : base(options)
        {
        }

        public DbSet<QuanLyBanHangCore.Models.Product> Products { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Producer> Producers { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Category> Categories { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Customer> Customers { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Order> Orders { get; set; }
        public DbSet<QuanLyBanHangCore.Models.DetailOrder> DetailOrders { get; set; }
        public DbSet<QuanLyBanHangCore.Models.ProductPrice> ProductPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DetailOrder>()
                .HasKey(d => new { d.OrderID, d.ProductID });
        }
    }
}
