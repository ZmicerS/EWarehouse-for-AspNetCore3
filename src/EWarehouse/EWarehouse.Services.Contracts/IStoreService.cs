using EWarehouse.Services.Entities.StoreModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EWarehouse.Services.Contracts
{
    public interface IStoreService
    {
        Task<int> AddBookAsync(BookServiceModel book);
        Task<List<BookCoreServiceModel>> GetBooksAsync();
        Task<List<LanguageServiceModel>> GetLanguagesAsync();
        Task<BookCoverServiceModel> GetBookCoverAsync(int bookId);
        Task<BookCoreServiceModel> GetBookAsync(int id);
        Task<int> UpdateBookAsync(BookServiceModel book);
        Task DeleteBookAsync(int id);
    }
}
