using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; }
        public bool IsNullable { get; set; } = true;
        public int Length { get; set; } = -1; // For variable-length columns
        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
