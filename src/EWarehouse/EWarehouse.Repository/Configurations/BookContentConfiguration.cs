using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EWarehouse.Repository.Entities.Store;

namespace EWarehouse.Repository.Configurations
{
    class BookContentConfiguration : IEntityTypeConfiguration<BookContent>
    {
        public void Configure(EntityTypeBuilder<BookContent> builder)
        {
            builder.ToTable("BookContents");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Content).IsRequired().HasColumnType("NVARCHAR(4000)");
            builder.Property(p => p.BookId).IsRequired().HasColumnType("INT");
        }
    }

}
