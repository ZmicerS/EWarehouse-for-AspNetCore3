using AutoMapper;
using EWarehouse.Repository.Contracts;
using EWarehouse.Repository.Entities.Store;
using EWarehouse.Services.Contracts;
using EWarehouse.Services.Entities.StoreModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWarehouse.Services
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreService> _logger;

        public StoreService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StoreService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> AddBookAsync(BookServiceModel book)
        {
            _logger.LogInformation("AddBookAsync");
            var bookDto = _mapper.Map<Book>(book);
            await _unitOfWork.Books.CreateAsync(bookDto);
        
            var coverDto = _mapper.Map<BookCover>(book);
            if (coverDto.LengthOfFile > 0)
            {
                coverDto.BookId = bookDto.Id;
                coverDto.Book = bookDto;

                await _unitOfWork.BookCovers.CreateAsync(coverDto);
            }
            await _unitOfWork.SaveAsync();

            return bookDto.Id;
        }

        public async Task<List<LanguageServiceModel>> GetLanguagesAsync()
        {
            _logger.LogInformation("GetLanguagesAsync");
            var languages = await _unitOfWork.Languages.GetAllAsync();

            var languageServiceCollection = _mapper.Map<List<LanguageServiceModel>>(languages);

            return languageServiceCollection;
        }

        public async Task<List<BookCoreServiceModel>> GetBooksAsync()
        {
            _logger.LogInformation("GetBooksAsync");
            var books = _unitOfWork.Books.GetAll();
            var bookContents = _unitOfWork.BookContents.GetAll();

            var bs = from b in books
                     join bc in bookContents on b.Id equals bc.BookId
                     select new BookCoreServiceModel
                     {
                         Id = b.Id,
                         Author = b.Author,
                         Name = b.Name,
                         Isbn = b.Isbn,
                         Price = b.Price,
                         Quantity = b.Quantity,
                         LanguageId = b.LanguageId,
                         Content = bc.Content
                     };
            var bookServiceModels = await Task.Run(() => bs.ToList().OrderBy(n => n.Name));

            return bookServiceModels.ToList();
        }

        public async Task<BookCoverServiceModel> GetBookCoverAsync(int bookId)
        {
            _logger.LogInformation("GetBookCoverAsync");
            var cover = await Task.Run(() => _unitOfWork.BookCovers.Get(c => c.BookId == bookId).FirstOrDefault());

            var bookCoverServiceModel = _mapper.Map<BookCoverServiceModel>(cover);
            return bookCoverServiceModel;
        }

        public async Task<BookCoreServiceModel> GetBookAsync(int id)
        {
            _logger.LogInformation("GetBookAsync");
            var bookDto = await _unitOfWork.Books.GetByIdAsync(id);
            var contentDto = await Task.Run(() => _unitOfWork.BookContents.Get(c => c.BookId == bookDto.Id).FirstOrDefault());

            var book = _mapper.Map<BookCoreServiceModel>(bookDto);
            book.Content = contentDto?.Content;
            return book;
        }

        public async Task<int> UpdateBookAsync(BookServiceModel book)
        {
            _logger.LogInformation("UpdateBookAsync");
            var bookDto = await _unitOfWork.Books.GetByIdAsync(book.Id);
            bookDto.Name = book.Name;
            bookDto.Author = book.Author;
            bookDto.Isbn = book.Isbn;
            bookDto.Price = book.Price;
            bookDto.Quantity = book.Quantity;
            bookDto.LanguageId = book.LanguageId;

            _unitOfWork.Books.Update(bookDto);         

            if (book.imageOfCover.LengthOfFile > 0)
            {
                var coverDto = await Task.Run(() => _unitOfWork.BookCovers.Get(bc => bc.BookId == book.Id).FirstOrDefault());
                if (coverDto != null)
                {
                    coverDto.BodyOfFile = book.imageOfCover.BodyOfFile;
                    coverDto.NameOfFile = book.imageOfCover.NameOfFile;
                    coverDto.LengthOfFile = book.imageOfCover.LengthOfFile;
                    coverDto.TypeOfFile = book.imageOfCover.TypeOfFile;
                    _unitOfWork.BookCovers.Update(coverDto);
                }
                else
                {
                    coverDto = new BookCover
                    {
                        BodyOfFile = book.imageOfCover.BodyOfFile,
                        NameOfFile = book.imageOfCover.NameOfFile,
                        LengthOfFile = book.imageOfCover.LengthOfFile,
                        TypeOfFile = book.imageOfCover.TypeOfFile,
                        BookId = book.Id
                    };
                    await _unitOfWork.BookCovers.CreateAsync(coverDto);
                }
            }
            await _unitOfWork.SaveAsync();
            return (book.Id);
        }

        public async Task DeleteBookAsync(int id)
        {
            _logger.LogInformation("DeleteBookAsync");
            var book = _unitOfWork.Books.GetById(id);

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.SaveAsync();
        }
    }

}
