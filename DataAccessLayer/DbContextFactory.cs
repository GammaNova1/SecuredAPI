using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO; // Directory.GetCurrentDirectory() için gerekli

namespace DataAccessLayer
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataAccessLayer.DbContext> // Tam sınıf adı
    {
        public DataAccessLayer.DbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataAccessLayer.DbContext>();

            // AppSettings.json veya başka bir kaynak üzerinden connection string'i alabilirsiniz
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            return new DataAccessLayer.DbContext(optionsBuilder.Options, configuration); // Tam sınıf adı
        }
    }
}
