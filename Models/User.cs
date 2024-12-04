using OmegaOrm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey(IsIdentity = true)]
        public int Id { get; set; }

        [Column("Username", Length = 50, IsNullable = false)]
        [Unique]
        public string Username { get; set; }

        [Column("Email", Length = 100)]
        public string Email { get; set; }

        [OneToMany(nameof(Address.UserId))]
        public List<Address> Addresses { get; set; }
    }
}
