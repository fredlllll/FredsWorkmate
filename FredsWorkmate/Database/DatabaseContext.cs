﻿using FredsWorkmate.Database.Models;
using FredsWorkmate.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UUIDNext;

namespace FredsWorkmate.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BankInformation> BankInformations { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoicePosition> InvoicePositions { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TrackedTime> TrackedTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().ToTable(nameof(Addresses));
            modelBuilder.Entity<BankInformation>().ToTable(nameof(BankInformations));
            modelBuilder.Entity<CompanyInformation>().ToTable(nameof(CompanyInformations));
            modelBuilder.Entity<Customer>().ToTable(nameof(Customers));
            modelBuilder.Entity<Invoice>().ToTable(nameof(Invoices));
            modelBuilder.Entity<InvoicePosition>().ToTable(nameof(InvoicePositions));
            modelBuilder.Entity<Note>().ToTable(nameof(Notes));
            modelBuilder.Entity<Project>().ToTable(nameof(Projects));
            modelBuilder.Entity<TrackedTime>().ToTable(nameof(TrackedTimes));
        }

        public object GetEntityDbSet(Type t)
        {
            var properties = GetType().GetProperties();
            object? set = null;
            foreach (var prop in properties)
            {
                var pt = prop.PropertyType;
                if (pt.IsGenericType && pt.GenericTypeArguments[0] == t)
                {
                    set = prop.GetValue(this);
                    break;
                }
            }
            if (set == null)
            {
                throw new InvalidDataException($"cant find db set for type {t}");
            }
            return set;
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
