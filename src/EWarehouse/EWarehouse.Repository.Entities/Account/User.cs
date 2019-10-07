using System.Collections.Generic;

namespace EWarehouse.Repository.Entities.Account
{
    public class User
    {
        public int Id { get; set; }
        public string Email { set; get; }
        public string Password { set; get; }
        public List<UserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
