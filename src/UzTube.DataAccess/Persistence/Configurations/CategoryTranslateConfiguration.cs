using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class CategoryTranslateConfiguration : IEntityTypeConfiguration<CategoryTranslate>
{
    public void Configure(EntityTypeBuilder<CategoryTranslate> builder)
    {
        builder.HasKey(ct => ct.Id);
        
        builder.HasOne(ct => ct.Owner)
            .WithMany(c => c.Translates)
            .HasForeignKey(ct => ct.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ct => ct.ColumnName)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}