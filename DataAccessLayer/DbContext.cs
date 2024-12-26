using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccessLayer
{
    public class DbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IConfiguration _configuration;
        private IDbConnection DbConnection { get; }
        public DbContext(DbContextOptions<DbContext> options, IConfiguration configuration)
          : base(options)
        {
            _configuration = configuration;

            DbConnection = new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
        }

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            DbConnection = new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConnection.ConnectionString);
            }

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        DbSet<User> User { get; set; }
        DbSet<Role> Role { get; set; }
    }
}
