using System.Collections.Generic;
using AutoMapper;
using EWarehouse.Repository.Entities.Account;
using EWarehouse.Services;
using EWarehouse.Services.Entities.AccountModels;
using EWarehouse.Web.Models.Account;

namespace EWarehouse.Web.Services.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, RegisterServiceModel>();
            CreateMap<RegisterServiceModel, User>()
                .ForMember(dest => dest.Password, act => act.MapFrom(src => HashPasswordService.GetHshedPassword(src.Password)))
                .ForMember(dest => dest.UserRoles, act => act.MapFrom(src => new List<UserRole> { new UserRole { RoleId = (int)RolesOfUser.User } }));

            CreateMap<LoginViewModel, LoginServiceModel>();
            CreateMap<RegisterViewModel, AccountServiceModel>();
            CreateMap<RoleServiceModel, RoleViewModel>().ReverseMap();
        }
    }
}
