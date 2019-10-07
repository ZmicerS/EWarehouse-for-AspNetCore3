using EWarehouse.Repository.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository.Account
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }
    }
}
