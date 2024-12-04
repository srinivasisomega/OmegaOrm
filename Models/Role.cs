using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmegaOrm.Attributes;

namespace OmegaOrm.Models
{
    [Table("Roles")]
    public class Role
    {
        [PrimaryKey(IsIdentity = true)]
        public int Id { get; set; }

        [Column("RoleName", Length = 50)]
        public string RoleName { get; set; }
    }
}
