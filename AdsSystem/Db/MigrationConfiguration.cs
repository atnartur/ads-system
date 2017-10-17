using SoftGage.EntityFramework.Migrations.Extensions;

namespace AdsSystem.Db
{
    public class MigrationConfiguration : ExtendedConfiguration<Context>
    {
        public MigrationConfiguration()
        {
            // Set context key as you wish.
            ContextKey = "AdsSystem.Models";
        }
    }
}