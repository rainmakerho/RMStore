using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMStore.Domain
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductID = 1,
                    Name = "活性碳口罩",
                    Description = "可吸附有機氣體和毒性粉塵，但並不能有效的過濾空氣微粒和細菌",
                    Category = "口罩",
                    Price = 20
                },
                new Product
                {
                    ProductID = 2,
                    Name = "Apple iPhone 11 64G 6.1吋智慧型手機",
                    Description = "6.1 吋 全螢幕LCD IP68等級防潑、抗水與防塵 超廣角與廣角雙相機系統",
                    Category = "手機",
                    Price = 25900
                }
                );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Email = "rm@gss.com.tw",
                    FirstName ="rainmaker",
                    LastName = "ho",
                    Password = "0001"
                },
                new User
                {
                    UserID = 2,
                    Email = "tony@gss.com.tw",
                    FirstName = "tony",
                    LastName = "lee",
                    Password = "0002"
                }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
