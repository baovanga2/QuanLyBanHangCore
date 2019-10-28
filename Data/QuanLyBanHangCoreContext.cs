using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;

namespace QuanLyBanHangCore.Models
{
    public class QuanLyBanHangCoreContext : DbContext
    {
        public QuanLyBanHangCoreContext (DbContextOptions<QuanLyBanHangCoreContext> options)
            : base(options)
        {
        }

        public DbSet<QuanLyBanHangCore.Models.Product> Products { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Producer> Producers { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Category> Categies { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Customer> Customers { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Role> Roles { get; set; }
        public DbSet<QuanLyBanHangCore.Models.User> Users { get; set; }
        public DbSet<QuanLyBanHangCore.Models.Order> Orders { get; set; }
        public DbSet<QuanLyBanHangCore.Models.DetailOrder> DetailOrders { get; set; }
    }
}
