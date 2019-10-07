using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository.Store
{
    public class BookContentRepository : RepositoryBase<BookContent>
    {
        public BookContentRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }
    }
}
