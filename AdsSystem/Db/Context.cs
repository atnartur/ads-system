using Microsoft.EntityFrameworkCore;
using SoftGage.EntityFramework.Migrations.Extensions;

namespace AdsSystem.Db
{
    public class Context : ExtendedDbContext
    {
        private static Context _instance;
        public static Context Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Context(@"Server=localhost;database=ads;uid=ads;pwd=ads;");
                return _instance;
            }
        }

        public Context(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

//        public DbSet<User> Users { get; set; }
    }
}