﻿using AdsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdsSystem
{
    public class Db : DbContext, IDesignTimeDbContextFactory<Db>
    {
        public static Db Instance
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<Db>();
                optionsBuilder.UseMySql("server=127.0.0.1;uid=ads;pwd=ads;database=ads");
                return new Db(optionsBuilder.Options);
            }
        }

        public Db() : base() {}
        
        public Db(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<BannersZones> BannersZones { get; set; }
        
        public Db CreateDbContext(string[] args) => Instance;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BannersZones>()
                .HasKey(c => new { c.BannerId, c.ZoneId });
        }
    }
}