using System.Collections.Generic;

namespace EWarehouse.Repository.Entities.Account
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { set; get; }
        public List<UserRole> UserRoles { get; set; }

        public Role()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
