﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksApi.Models;

namespace StocksApi.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x=> x.HasKey(p => new { p.UserId, p.StockId }));

            builder.Entity<Portfolio>()
                .HasOne(x => x.User)
                .WithMany(x => x.Portfolios)
                .HasForeignKey(x => x.UserId);

            builder.Entity<Portfolio>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.Portfolios)
                .HasForeignKey(x => x.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Models.Stock>()
        //        .HasMany(s => s.Comments)
        //        .WithOne(c => c.Stock)
        //        .HasForeignKey(c => c.StockId);
        //}
    }
}
