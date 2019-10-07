using System.Collections.Generic;

namespace EWarehouse.Services.Entities.AccountModels
{
    public class UserServiceModel
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        public List<RoleServiceModel> AssignedRoles { set; get; }
    }

}
