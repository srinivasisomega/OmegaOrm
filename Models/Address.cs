using OmegaOrm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Models
{
    [Table("Addresses")]
    public class Address
    {
        [PrimaryKey(IsIdentity = true)]
        public int Id { get; set; }

        [ForeignKey("Users", "Id")]
        public int UserId { get; set; }

        [Column("Street", Length = 200)]
        public string Street { get; set; }

        [OneToOne(nameof(UserId))]
        public User User { get; set; }
    }
}
