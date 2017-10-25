using AdsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdsSystem
{
    public class Db : DbContext, IDesignTimeDbContextFactory<Db>
    {
        private static Db _instance;
        public static Db Instance
        {
            get
            {
                if (_instance == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<Db>();
                    optionsBuilder.UseMySql("server=127.0.0.1;uid=ads;pwd=ads;database=ads");

                    _instance = new Db(optionsBuilder.Options);
                    _instance.Database.Migrate();
                }
                return _instance;
            }
        }

        public Db() : base() {}
        
        public Db(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        
        public Db CreateDbContext(string[] args) => Instance;
    }
}