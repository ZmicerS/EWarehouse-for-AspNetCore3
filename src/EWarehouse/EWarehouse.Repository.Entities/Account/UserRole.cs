namespace EWarehouse.Repository.Entities.Account
{
    public class UserRole
    {
        public int RoleId { set; get; }
        public Role Role { get; set; }
        public int UserId { set; get; }
        public User User { get; set; }
    }
}
