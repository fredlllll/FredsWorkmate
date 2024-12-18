using FredsWorkmate.Database.Models;
using FredsWorkmate.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UUIDNext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FredsWorkmate.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TrackedTime> TrackedTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable(nameof(Customer));
            modelBuilder.Entity<Project>().ToTable(nameof(Project));
            modelBuilder.Entity<TrackedTime>().ToTable(nameof(TrackedTime));
        }

        public static string GetConnectionString()
        {
            string Host = Settings.GetNotNull<string>("postgresHost");
            int Port = Settings.GetNotNull<int>("postgresPort");
            string Database = Settings.GetNotNull<string>("postgresDatabase");
            string User = Settings.GetNotNull<string>("postgresUser");
            string Password = Settings.GetNotNull<string>("postgresPassword");

            string connectionString = $"Host={Host};Port={Port};Database={Database};User ID={User};";
            if (Password.Length > 0)
            {
                connectionString += "Password=" + Password + ";";
            }
            return connectionString;
        }

        public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
        {
            public DatabaseContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseNpgsql(GetConnectionString());

                return new DatabaseContext(optionsBuilder.Options);
            }
        }

        public string GetNewId<T>()
        {
            var t = typeof(T);
            var typeId = t.Name.ToLower();
            var id = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
            return $"{typeId}_{id}";
        }
    }
}
