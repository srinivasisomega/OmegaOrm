using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OneToManyAttribute : Attribute
    {
        public string ForeignKeyProperty { get; }
        public OneToManyAttribute(string foreignKeyProperty)
        {
            ForeignKeyProperty = foreignKeyProperty;
        }
    }

}
