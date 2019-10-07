using EWarehouse.Repository.Account;
using EWarehouse.Repository.Contracts;
using EWarehouse.Repository.Entities.Account;
using EWarehouse.Repository.Entities.Store;
using EWarehouse.Repository.Store;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EWarehouse.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _db;      
        private IRepositoryBase<Book> _books { set; get; }
        private IRepositoryBase<BookContent> _bookContents { set; get; }
        private IRepositoryBase<BookCover> _bookCovers { set; get; }
        private IRepositoryBase<Language> _languages { set; get; }

        private IRepositoryBase<User> _users { set; get; }
        private IRepositoryBase<Role> _roles { set; get; }
        private IRepositoryBase<UserRole> _userRoles { set; get; }
        private readonly ILogger _logger;

        public UnitOfWork(ApplicationContext db, ILoggerFactory loggerFactory)
        {           
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger("REPOSITORY");
            _db = db;
        }

        public IRepositoryBase<Book> Books
        {
            get
            {
                if (_books == null)
                {
                    _books = new BookRepository(_db, _logger);
                }
                return _books;
            }
        }

        public IRepositoryBase<BookContent> BookContents
        {
            get
            {
                if (_bookContents == null)
                {
                    _bookContents = new BookContentRepository(_db, _logger);
                }
                return _bookContents;
            }
        }

        public IRepositoryBase<BookCover> BookCovers
        {
            get
            {
                if (_bookCovers == null)
                {
                    _bookCovers = new BookCoverRepository(_db, _logger);
                }
                return _bookCovers;
            }
        }

        public IRepositoryBase<Language> Languages
        {
            get
            {
                if (_languages == null)
                {
                    _languages = new LanguageRepository(_db, _logger);
                }
                return _languages;
            }
        }

        public IRepositoryBase<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new UserRepository(_db, _logger);
                }
                return _users;
            }
        }

        public IRepositoryBase<Role> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new RoleRepository(_db, _logger);
                }
                return _roles;
            }
        }

        public IRepositoryBase<UserRole> UserRoles
        {
            get
            {
                if (_userRoles == null)
                {
                    _userRoles = new UserRoleRepository(_db, _logger);
                }
                return _userRoles;
            }
        }

        public void Save()
        {
            _logger.LogInformation($"UnitOfWork - Save");
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            _logger.LogInformation($"UnitOfWork - SaveAsync");
            await _db.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                disposed = true;
            }
        }
    }
}
