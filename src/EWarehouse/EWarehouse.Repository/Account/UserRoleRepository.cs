using EWarehouse.Repository.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository.Account
{
    public class UserRoleRepository : RepositoryBase<UserRole>
    {
        public UserRoleRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }
    }
}
