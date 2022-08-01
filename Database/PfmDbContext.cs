using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public class PfmDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions {get; set;}
        public DbSet<CategoryEntity> Categories { get; set;}
        public DbSet<SplitEntity> CategorizedTransactions {get; set;}
        public PfmDbContext()
        {
        }
        public PfmDbContext(DbContextOptions<PfmDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehaviour", true);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}