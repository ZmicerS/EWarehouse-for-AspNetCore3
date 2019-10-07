using AutoMapper;
using EWarehouse.Repository.Entities.Store;
using EWarehouse.Services.Entities.AccountModels;
using EWarehouse.Services.Entities.StoreModels;
using EWarehouse.Web.Models.Account;
using EWarehouse.Web.Models.Store;

namespace EWarehouse.Web.Services.Mappers
{
    public class StoreMappingProfile : Profile
    {
        public StoreMappingProfile()
        {
            CreateMap<UserServiceModel, UserViewModel>().ReverseMap();
            CreateMap<BookCoverViewModel, BookCoverServiceModel>();
            CreateMap<AddBookViewModel, BookServiceModel>();
            CreateMap<UpdateBookViewModel, BookServiceModel>();
            CreateMap<LanguageViewModel, LanguageServiceModel>().ReverseMap();
            CreateMap<Language, LanguageServiceModel>();

            CreateMap<BookCoreVieweModel, BookCoreServiceModel>().ReverseMap();
            CreateMap<BookCoverServiceModel, BookCover>().ReverseMap();
            CreateMap<BookServiceModel, BookContent>()
             .ForMember(dest => dest.Content, act => act.MapFrom(src => src.Content))
             .ForMember(dest => dest.BookId, act => act.MapFrom(src => src.Id));
            CreateMap<BookServiceModel, BookCover>()
                .ForMember(dest => dest.BookId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.BodyOfFile, act => act.MapFrom(src => src.imageOfCover.BodyOfFile))
                .ForMember(dest => dest.NameOfFile, act => act.MapFrom(src => src.imageOfCover.NameOfFile))
                .ForMember(dest => dest.LengthOfFile, act => act.MapFrom(src => src.imageOfCover.LengthOfFile))
                .ForMember(dest => dest.TypeOfFile, act => act.MapFrom(src => src.imageOfCover.TypeOfFile));

            CreateMap<BookServiceModel, Book>()
            .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
            .ForMember(dest => dest.Author, act => act.MapFrom(src => src.Author))
            .ForMember(dest => dest.Isbn, act => act.MapFrom(src => src.Isbn))
            .ForMember(dest => dest.Price, act => act.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.LanguageId, act => act.MapFrom(src => src.LanguageId))
            .ForPath(dest => dest.Content.Content, act => act.MapFrom(src => src.Content))
            .ForPath(dest => dest.Content.Id, act => act.MapFrom(src => src.Id))
            .ForMember(dest => dest.Cover, opt => opt.Ignore());

            CreateMap<Book, BookCoreServiceModel>();
        }
    }
}

