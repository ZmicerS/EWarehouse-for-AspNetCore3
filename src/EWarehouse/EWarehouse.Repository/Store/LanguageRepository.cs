using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository.Store
{
    public class LanguageRepository : RepositoryBase<Language>
    {
        public LanguageRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }
    }
}
