using EWarehouse.Repository.Configurations;
using EWarehouse.Repository.Entities.Account;
using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository
{
    public class ApplicationContext : DbContext
    {
        private readonly ILogger _logger;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _logger = loggerFactory.CreateLogger("ApplicationContext");

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation($"OnModelCreating");

            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookContentConfiguration());
            modelBuilder.ApplyConfiguration(new BookCoverConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookContent> BookContents { get; set; }
        public DbSet<BookCover> BookCovers { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
