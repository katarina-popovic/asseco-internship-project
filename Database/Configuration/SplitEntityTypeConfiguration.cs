using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Configuration
{
    public class SplitEntityTypeConfiguration : IEntityTypeConfiguration<SplitEntity>
    {
        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.ToTable("Split");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(c => c.Amount).IsRequired();
            builder.HasOne(c => c.Transaction).WithMany(c => c.SplitTransactionList).HasForeignKey(c => c.TransactionId);
            builder.HasOne(c => c.Category).WithMany(c => c.SplitCategoryList).HasForeignKey(c => c.CategoryCode);
        }
    }
}
