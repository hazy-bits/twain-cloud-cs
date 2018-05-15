using System.Data.Entity;
using SQLite.CodeFirst;

namespace HazyBits.Twain.Manager.Storage
{
    public class CloudContext: DbContext
    {
        public CloudContext(): base("twainCloudDb")
        { }

        public DbSet<CloudScanner> Scanners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<CloudContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
