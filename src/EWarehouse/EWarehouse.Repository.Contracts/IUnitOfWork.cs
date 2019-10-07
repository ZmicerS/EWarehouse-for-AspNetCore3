using EWarehouse.Repository.Entities.Account;
using EWarehouse.Repository.Entities.Store;
using System;
using System.Threading.Tasks;

namespace EWarehouse.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<Book> Books { get; }
        IRepositoryBase<BookContent> BookContents { get; }
        IRepositoryBase<BookCover> BookCovers { get; }
        IRepositoryBase<Language> Languages { get; }
        IRepositoryBase<User> Users { get; }
        IRepositoryBase<Role> Roles { get; }
        IRepositoryBase<UserRole> UserRoles { get; }
        void Save();
        Task SaveAsync();
    }

}
