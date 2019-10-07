using EWarehouse.Repository.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EWarehouse.Repository.Store
{
    public class BookRepository : RepositoryBase<Book>
    {
        public BookRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }
    }
}
