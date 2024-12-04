using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OneToOneAttribute : Attribute
    {
        public string ForeignKeyProperty { get; }
        public OneToOneAttribute(string foreignKeyProperty)
        {
            ForeignKeyProperty = foreignKeyProperty;
        }
    }
}
