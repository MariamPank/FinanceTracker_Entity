using FinanceTracker_Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        //public DbSet<Category> Categories { get; set; } ????
        public DbSet<TransactionCategory> TransactionCategories { get; set; }

        public static User CurrentUser { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=mssql-206521-0.cloudclusters.net,10100;Initial Catalog=FinanceTracker;User ID=mp;Password=Mp123456;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }



        // ეს ქვედა მჭირდება ეხლა? რა შემთხვევაში გამოიყენება?
        //protected override void OnModelCreating(ModelBuilder model)
        //{
        //    // One-to-One
        //    model.Entity<User>()
        //        .HasOne(u => u.Profile)
        //        .WithOne(p => p.User)
        //        .HasForeignKey<UserProfile>(p => p.UserId);

        //    // One-to-Many
        //    model.Entity<User>()
        //        .HasMany(u => u.Transactions)
        //        .WithOne(t => t.User)
        //        .HasForeignKey(t => t.UserId);

        //    // Many-to-Many
        //    model.Entity<TransactionCategory>()
        //        .HasKey(x => new { x.TransactionId, x.CategoryId });

        //    model.Entity<TransactionCategory>()
        //        .HasOne(x => x.Transaction)
        //        .WithMany(t => t.TransactionCategories)
        //        .HasForeignKey(x => x.TransactionId);

        //    model.Entity<TransactionCategory>()
        //        .HasOne(x => x.Category)
        //        .WithMany(c => c.TransactionCategories)
        //        .HasForeignKey(x => x.CategoryId);
        //}
    }
}
