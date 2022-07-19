using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public class TransactionsDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions {get; set;}
        public TransactionsDbContext()
        {
        }
        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehaviour", true);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}