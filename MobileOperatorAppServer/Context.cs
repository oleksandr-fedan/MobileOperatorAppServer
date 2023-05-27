using System;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer.Models;

namespace MobileOperatorAppServer
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminModel>().HasIndex(e => e.Username).IsUnique();
            modelBuilder.Entity<UserModel>().HasIndex(e => e.PhoneNumber).IsUnique();
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TariffModel> Tariffs { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<UserCodeModel> UserCodes { get; set; }
        public DbSet<UserConnectedServicesModel> UserConnectedServices { get; set; }
    }
}
