using EWarehouse.Repository.Entities.Account;
using EWarehouse.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWarehouse.Repository.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Email).IsRequired().HasColumnType("NVARCHAR(50)");
            builder.Property(p => p.Password).IsRequired().HasColumnType("NVARCHAR(50)");
            builder.HasData(
                  new User[]
                  {
                   new User
                   {
                     Id =1, Email = "superadmin@yandex.ru", Password = HashPasswordService.GetHshedPassword("superadmin")
                   }
                  });
        }
    }
}
