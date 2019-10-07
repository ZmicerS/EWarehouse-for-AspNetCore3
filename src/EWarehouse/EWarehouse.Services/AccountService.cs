using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using EWarehouse.Repository.Contracts;
using EWarehouse.Repository.Entities.Account;
using EWarehouse.Services.Contracts;
using EWarehouse.Services.Entities.AccountModels;

namespace EWarehouse.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RegisterUserAsync(RegisterServiceModel registerServiceModel)
        {
            _logger.LogInformation($"AccountService (GetUser) - {registerServiceModel.Email}");
            var user = _mapper.Map<User>(registerServiceModel);
           
            await _unitOfWork.Users.CreateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserServiceModel> GetUserAsync(LoginServiceModel model)
        {
            _logger.LogInformation($"AccountService (GetUser) - {model.Email}");
            var hashPassword = HashPasswordService.GetHshedPassword(model.Password);
          
            var user = await Task.Run(() => _unitOfWork.Users.Get(u => u.Email == model.Email && u.Password == hashPassword).FirstOrDefault());
            
            if (user == null)
            {
                return null;
            }
            var userRoles = await Task.Run(() => _unitOfWork.UserRoles.Get(r => r.UserId == user.Id));

            var roles = await _unitOfWork.Roles.GetAllAsync();

            var assignedRoles = from ur in userRoles.AsEnumerable()
                                join rl in roles on ur.RoleId equals rl.Id
                                select new RoleServiceModel
                                {
                                    Id = ur.RoleId,
                                    RoleName = rl.RoleName
                                };

            var userServiceModel = new UserServiceModel
            {
                Id = user.Id,
                UserName = user.Email,
                AssignedRoles = assignedRoles.ToList()
            };

            return userServiceModel;
        }

        public async Task<bool> FindUserAsync(AccountServiceModel model)
        {
            _logger.LogInformation($"AccountService (FindUserAsync) - {model.Email}");
            var hashPassword = HashPasswordService.GetHshedPassword(model.Password);
            var user = await Task.Run(() => _unitOfWork.Users.Get(u => u.Email == model.Email && u.Password == hashPassword).FirstOrDefault());

            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<UserServiceModel>> GetUsersAsync()
        {
            _logger.LogInformation($"AccountService (GetUsersAsync) ");
            var userServiceCollection = new List<UserServiceModel>();

            var users = await _unitOfWork.Users.GetAllAsync();

            var roles = await _unitOfWork.Roles.GetAllAsync();

            foreach (var user in users)
            {
                var userRoles = _unitOfWork.UserRoles.Get(r => r.UserId == user.Id);

                var assignedRoles = (from ur in userRoles.AsEnumerable()
                                     join rl in roles on ur.RoleId equals rl.Id
                                     select new RoleServiceModel
                                     {
                                         Id = ur.RoleId,
                                         RoleName = rl.RoleName
                                     }).ToList();
                var userServiceModel = new UserServiceModel
                {
                    Id = user.Id,
                    UserName = user.Email,
                    AssignedRoles = assignedRoles
                };
                userServiceCollection.Add(userServiceModel);
            }

            return userServiceCollection;
        }

        public async Task<UserServiceModel> GetUserAsync(int id)
        {
            _logger.LogInformation($"AccountService (GetUserAsync)  - {id}");
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            var roles = await _unitOfWork.Roles.GetAllAsync();
            var userRoles = await Task.Run(() => _unitOfWork.UserRoles.Get(r => r.UserId == user.Id));
            var assignedRoles = (from ur in userRoles.AsEnumerable()
                                 join rl in roles on ur.RoleId equals rl.Id
                                 select new RoleServiceModel
                                 {
                                     Id = ur.RoleId,
                                     RoleName = rl.RoleName
                                 }).ToList();
            var userServiceModel = new UserServiceModel
            {
                Id = user.Id,
                UserName = user.Email,
                AssignedRoles = assignedRoles
            };
            return userServiceModel;
        }

        public async Task<List<RoleServiceModel>> GetRolesAsync()
        {
            _logger.LogInformation($"AccountService (GetRolesAsync)");
            var collection = await _unitOfWork.Roles.GetAllAsync();
            var roleServiceCollection = collection.Select(r => new RoleServiceModel
            {
                Id = r.Id,
                RoleName = r.RoleName
            }).ToList();

            return roleServiceCollection;
        }

        public async Task AssignRoleToUserAsync(UserServiceModel model)
        {
            _logger.LogInformation($"AccountService (UserServiceModel) - {model.UserName}");
            await Task.Run(() => _unitOfWork.UserRoles.Get(ur => ur.UserId == model.Id).ToList().ForEach(c => _unitOfWork.UserRoles.Delete(c)));
            var assignedRoles = model.AssignedRoles;
            foreach (var role in assignedRoles)
            {
                var userRole = new UserRole
                {
                    UserId = model.Id,
                    RoleId = role.Id,
                };
                await _unitOfWork.UserRoles.CreateAsync(userRole);
            };

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger.LogInformation($"AccountService (DeleteUserAsync)  - {id}");
            await Task.Run(() => _unitOfWork.UserRoles.Get(ur => ur.UserId == id).ToList().ForEach(c => _unitOfWork.UserRoles.Delete(c)));
            await Task.Run(() => _unitOfWork.Users.Delete(id));
            await _unitOfWork.SaveAsync();
        }

    }
}
