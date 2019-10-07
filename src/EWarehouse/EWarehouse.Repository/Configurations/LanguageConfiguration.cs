using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWarehouse.Repository.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.ShortName).IsRequired().HasColumnType("NVARCHAR(10)");
            builder.Property(p => p.FullName).IsRequired().HasColumnType("NVARCHAR(50)");
            builder.HasData(
                new Language[]
                {
                   new Language { Id = 1, ShortName = "be", FullName = "Belorussian"},
                   new Language { Id = 2, ShortName = "en", FullName = "English"},
                   new Language { Id = 3, ShortName = "de", FullName = "German"},
                   new Language { Id = 4, ShortName = "ru", FullName = "Russian"}
                });
        }
    }
}

