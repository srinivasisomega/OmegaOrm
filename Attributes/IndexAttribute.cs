using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IndexAttribute : Attribute
    {
        public string Name { get; }
        public bool IsUnique { get; set; } = false;
        public IndexAttribute(string name)
        {
            Name = name;
        }
    }
}
