using System.Collections.Generic;
using System.Threading.Tasks;
using EWarehouse.Services.Entities.AccountModels;

namespace EWarehouse.Services.Contracts
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterServiceModel registerServiceModel);
        Task<UserServiceModel> GetUserAsync(LoginServiceModel loginServiceModel);
        Task<bool> FindUserAsync(AccountServiceModel model);
        Task<List<UserServiceModel>> GetUsersAsync();
        Task<UserServiceModel> GetUserAsync(int id);
        Task<List<RoleServiceModel>> GetRolesAsync();
        Task AssignRoleToUserAsync(UserServiceModel model);
        Task DeleteUserAsync(int id);
    }
}
