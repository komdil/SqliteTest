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
        public string DatabaseName { get; }

        public SQliteDBContext(string databaseName)
        {
            DatabaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = SubscriberCacheConstants.GetDatabasePath(DatabaseName);
            optionsBuilder.UseSqlite($"FileName={databasePath}");
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

    public class SubscriberCacheConstants
    {
        public static DirectoryInfo GetCachePath()
        {
            var commonFolder = Environment.SpecialFolder.ApplicationData; //containers don't have access to write to common files
            var commonPath = Environment.GetFolderPath(commonFolder);
            return new DirectoryInfo(Path.Combine(commonPath, "Eagle3", "Cache"));
        }

        public static string GetDatabasePath(string databaseName)
        {
            DirectoryInfo commonCachePath = GetCachePath();
            if (!commonCachePath.Exists)
                commonCachePath.Create();

            var databasePath = Path.Combine(commonCachePath.FullName, $"{databaseName}.db");
            return databasePath;
        }

        public static string GetDatabaseNameRoot()
        {
            string mode = "RELEASE";
#if DEBUG 
            mode = "DEBUG";
#endif
            return $"{mode}_Vision_Demo_";
        }

        public static string GetDatabaseName(string apiVersion = "")
        {
            if (string.IsNullOrEmpty(apiVersion))
                apiVersion = "1.1.1.1";

            apiVersion = apiVersion.Replace('.', '_'); //need to add the api version so that when there are two servers running they have different cache files
            var root = GetDatabaseNameRoot();
            return $"{root}_{apiVersion}_Vision_Demo";
        }
    }
}
