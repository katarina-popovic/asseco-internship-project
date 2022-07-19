using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Configuration
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transaction");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasMaxLength(10).IsRequired();
            builder.Property(t => t.BeneficiaryName).HasMaxLength(30);
            builder.Property(t => t.Date).IsRequired();
            builder.Property(t => t.Direction).IsRequired();
            builder.Property(t => t.Amount).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(30).IsRequired();
            builder.Property(t => t.Currency).HasMaxLength(5).IsRequired();
            builder.Property(t => t.Mcc).HasConversion<string>().HasMaxLength(5);
            builder.Property(t => t.Kind).HasConversion<string>().HasMaxLength(5).IsRequired();
        }
    }
}