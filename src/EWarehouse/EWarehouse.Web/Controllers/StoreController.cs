using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using EWarehouse.Services.Contracts;
using EWarehouse.Services.Entities.StoreModels;
using EWarehouse.Web.Models.Store;
using EWarehouse.Web.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace EWarehouse.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStoreService _storeService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(IMapper mapper, IStoreService storeService,
                               IAuthorizationService authorizationService,
                               ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Roles = "Admin")]
        [Route("store/addbook")]
        public async Task<ActionResult> AddBook()
        {
            var book = FillingBookFromRequest(true) as AddBookViewModel;

            TryValidateModel(book);
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.SelectMany(x => x.Value.Errors)
                                       .Select(x => x.ErrorMessage).ToArray();
                _logger.LogError("AddBook error");
                return BadRequest(Errors);
            }
            var bookServiceModel = _mapper.Map<BookServiceModel>(book);
            var id = await _storeService.AddBookAsync(bookServiceModel);

            return Ok(new { result = id });
        }

        [HttpGet]
        [Route("store/getlanguages")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetLanguages()
        {
            var languageServiceModelCollection = await _storeService.GetLanguagesAsync();
            var languages = _mapper.Map<List<LanguageViewModel>>(languageServiceModelCollection);

            return Ok(languages);
        }

        [HttpGet]
        [Route("store/getbooks")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetBooks()
        {
            var bookCoreServiceModelCollection = await _storeService.GetBooksAsync();
            var books = _mapper.Map<List<BookCoreVieweModel>>(bookCoreServiceModelCollection);
            return Ok(books);
        }

        [HttpGet]
        [Route("store/getcover/{id?}")]
        public async Task<IActionResult> GetCover(int id = 0)
        {
            var img = await _storeService.GetBookCoverAsync(id);
            if (img?.LengthOfFile > 0)
            {
                byte[] fileBytes = img.BodyOfFile.Take(img.LengthOfFile).ToArray();
                return File(fileBytes, img.TypeOfFile);
            }
            return Ok();
        }

        [HttpGet]
        [Route("store/getbook/{id?}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetBook(int id = 0)
        {
            var bookServiceModel = await _storeService.GetBookAsync(id);
            var book = _mapper.Map<BookCoreVieweModel>(bookServiceModel);
            return Ok(book);
        }

        [HttpPut, DisableRequestSizeLimit]
        [Authorize(Roles = "Admin")]
        [Route("store/updatebook")]
        public async Task<IActionResult> UpdateBook()
        {
            var book = FillingBookFromRequest(false) as UpdateBookViewModel;

            TryValidateModel(book);
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.SelectMany(x => x.Value.Errors)
                                       .Select(x => x.ErrorMessage).ToArray();
                _logger.LogError("AddBook error");

                return BadRequest(Errors);
            }
            var bookServiceModel = _mapper.Map<BookServiceModel>(book);
            var id = await _storeService.UpdateBookAsync(bookServiceModel);

            return Ok(new { result = id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("store/deletebook")]
        public async Task<IActionResult> DeleteBook([FromBody]int id)
        {
            await _storeService.DeleteBookAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("store/getpermissionstore")]
        public async Task<IActionResult> GetPermissionStore()
        {
            var roles = new List<string> { "Admin" };

            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, null, new RolesRequirement(roles));
            if (result.Succeeded)
            {
                return Ok(new { result = true });
            }
            return Ok(new { result = false });
        }

        [HttpGet]
        [Route("store/getpermissionorder")]
        public async Task<IActionResult> GetPermissionOrder()
        {
            var roles = new List<string> { "User" };

            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, null, new RolesRequirement(roles));
            if (result.Succeeded)
            {
                return Ok(new { result = true });
            }
            return Ok(new { result = false });
        }


        private BookViewModel FillingBookFromRequest(bool addition = false)
        {
            BookViewModel book;
            if (addition)
            {
                book = new AddBookViewModel();
            }
            else
            {
                book = new UpdateBookViewModel();
            }

            if (!string.IsNullOrEmpty(Request.ContentType)
                && Request.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (Request.Form.TryGetValue("id", out StringValues idValue))
                {
                    var id = idValue.FirstOrDefault();
                    if (int.TryParse(id, out int result) && !addition)
                    {
                        (book as UpdateBookViewModel).Id = result;
                    }
                }
                if (Request.Form.TryGetValue("name", out StringValues nameValue))
                {
                    book.Name = nameValue.FirstOrDefault();
                }
                if (Request.Form.TryGetValue("author", out StringValues authorValue))
                {
                    book.Author = authorValue.FirstOrDefault();
                }
                if (Request.Form.TryGetValue("isbn", out StringValues isbnValue))
                {
                    book.Isbn = isbnValue.FirstOrDefault();
                }
                if (Request.Form.TryGetValue("price", out StringValues priceValue))
                {
                    var price = priceValue.FirstOrDefault();
                    if (decimal.TryParse(price, out decimal result))
                    {
                        book.Price = result;
                    }
                }
                if (Request.Form.TryGetValue("quantity", out StringValues quantityValue))
                {
                    var quantity = quantityValue.FirstOrDefault();
                    if (int.TryParse(quantity, out int result))
                    {
                        book.Quantity = result;
                    }
                }
                if (Request.Form.TryGetValue("content", out StringValues contentValue))
                {
                    book.Content = contentValue.FirstOrDefault();
                }
                if (Request.Form.TryGetValue("languageid", out StringValues languageidValue))
                {
                    var languageid = languageidValue.FirstOrDefault();
                    if (int.TryParse(languageid, out int result))
                    {
                        book.LanguageId = result;
                    }
                }

                var cover = new BookCoverViewModel();

                var count = Request.Form.Files.Count;
                if (count > 0)
                {
                    var file = Request.Form.Files[0];
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).Name.Trim().ToString();

                    string fileType = file.ContentType;

                    cover.NameOfFile = file.Name;
                    cover.LengthOfFile = (int)file.Length;
                    cover.TypeOfFile = fileType;
                    byte[] fileBytes = new byte[56000];

                    using var stream = new MemoryStream(fileBytes);
                    file.CopyTo(stream);
                    cover.BodyOfFile = stream.ToArray();
                }
                book.imageOfCover = cover;

            }
            return book;
        }

    }
}
