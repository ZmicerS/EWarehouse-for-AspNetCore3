using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWarehouse.Repository.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Name).HasColumnType("NVARCHAR(100)").IsRequired();
            builder.Property(p => p.Author).HasColumnType("NVARCHAR(100)").IsRequired();
            builder.Property(p => p.Isbn).HasColumnType("NVARCHAR(100)").IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("DECIMAL(18, 2)");
            builder.Property(p => p.Quantity).IsRequired().HasColumnType("int");
            builder.HasOne(b => b.Content).WithOne(b => b.Book).HasForeignKey<BookContent>("BookId");
            builder.HasOne(b => b.Cover).WithOne(b => b.Book).HasForeignKey<BookCover>("BookId");
            builder.HasOne(l => l.Language).WithMany(b => b.Books).HasForeignKey("LanguageId");
        }
    }

}
