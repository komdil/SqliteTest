using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest
{
    internal class SQliteDBContext : DbContext
    {
        static DirectoryInfo GetCachePath()
        {
            var commonFolder = Environment.SpecialFolder.CommonApplicationData;
            var commonPath = Environment.GetFolderPath(commonFolder);
            return new DirectoryInfo(Path.Combine(commonPath, "Eagle2", "Cache"));
        }

        static string GetDatabasePath(string databaseName)
        {
            DirectoryInfo commonCachePath = GetCachePath();
            if (!commonCachePath.Exists)
                commonCachePath.Create();

            var databasePath = Path.Combine(commonCachePath.FullName, $"{databaseName}.db");
            return databasePath;
        }

        public string DatabaseName { get; }

        public SQliteDBContext(string databaseName)
        {
            DatabaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = GetDatabasePath(DatabaseName);
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().HasKey(d => d.Guid);
        }
    }

    internal class Student
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }
    }
}
