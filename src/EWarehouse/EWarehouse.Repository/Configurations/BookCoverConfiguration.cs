using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWarehouse.Repository.Configurations
{
    public class BookCoverConfiguration : IEntityTypeConfiguration<BookCover>
    {
        public void Configure(EntityTypeBuilder<BookCover> builder)
        {
            builder.ToTable("BookCovers");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.NameOfFile).IsRequired().HasColumnType("NVARCHAR(100)");
            builder.Property(p => p.TypeOfFile).IsRequired().HasColumnType("NVARCHAR(100)");
            builder.Property(p => p.LengthOfFile).IsRequired().HasColumnType("NVARCHAR(100)");
            builder.Property("BodyOfFile").IsRequired().HasColumnType("VARBINARY(MAX)");
        }
    }
}
