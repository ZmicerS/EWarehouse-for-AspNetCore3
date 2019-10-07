using EWarehouse.Repository.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EWarehouse.Repository.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.RoleName).IsRequired().HasColumnType("NVARCHAR(30)");
            builder.HasData(
                  new Role[]
                  {
                   new Role { Id=1, RoleName ="SuperAdmin" },
                   new Role { Id=2, RoleName ="Admin"},
                   new Role { Id=3, RoleName ="User"}
                  });
        }
    }
}
