using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Configuration
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(c => c.Code);
            builder.Property(c => c.Code).HasMaxLength(5).IsRequired();
            builder.Property(c => c.ParentCode).HasMaxLength(5);
            builder.Property(c => c.Name).HasMaxLength(30).IsRequired();
            builder.HasMany(t => t.SplitCategoryList).WithOne(t => t.Category).HasForeignKey(t => t.CategoryCode);
        }
    }
}
