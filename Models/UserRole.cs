using OmegaOrm.Attributes;
namespace OmegaOrm.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        [PrimaryKey]
        public int RoleId { get; set; }

        [PrimaryKey]
        public int UserId { get; set; }

        [ManyToMany("UserRoles", "UserId", "RoleId")]
        public List<Role> Roles { get; set; }
    }
}
