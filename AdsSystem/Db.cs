using System;
using AdsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AdsSystem
{
    public class Db : DbContext, IDesignTimeDbContextFactory<Db>
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        public static Db Instance
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<Db>();

                var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1";
                var user = Environment.GetEnvironmentVariable("DB_USER") ?? "ads";
                var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "ads";
                var name = Environment.GetEnvironmentVariable("DB_NAME") ?? "ads";

                optionsBuilder.UseMySql("server=" + host + ";uid=" + user + ";pwd=" + pass + ";database=" + name);
                optionsBuilder.UseLoggerFactory(MyLoggerFactory);
                return new Db(optionsBuilder.Options);
            }
        }

        public Db() : base() { }

        private Db(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<BannersZones> BannersZones { get; set; }
        public DbSet<Models.View> Views { get; set; }
        public DbSet<DayStats> DayStats { get; set; }

        public Db CreateDbContext(string[] args) => Instance;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BannersZones>()
                .HasKey(c => new { c.BannerId, c.ZoneId });
        }
    }
}