using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository.Store
{
    class BookCoverRepository : RepositoryBase<BookCover>
    {
        public BookCoverRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }
    }
}
