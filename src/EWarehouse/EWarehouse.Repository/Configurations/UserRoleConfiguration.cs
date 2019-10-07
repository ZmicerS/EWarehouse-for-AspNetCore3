using EWarehouse.Repository.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EWarehouse.Repository.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(u => new { u.RoleId, u.UserId });

            builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

            builder.HasOne(ur => ur.User)
           .WithMany(r => r.UserRoles)
           .HasForeignKey(ur => ur.UserId);

            builder.HasData(
                 new UserRole[]
                 {
                   new UserRole { RoleId = 1, UserId = 1},
                   new UserRole { RoleId = 2, UserId = 1},
                   new UserRole { RoleId = 3, UserId = 1}
                 });

        }
    }
}
