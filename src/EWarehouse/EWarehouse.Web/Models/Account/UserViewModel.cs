using System.Collections.Generic;

namespace EWarehouse.Web.Models.Account
{
    public class UserViewModel
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        public List<RoleViewModel> AssignedRoles { set; get; }
    }
}
