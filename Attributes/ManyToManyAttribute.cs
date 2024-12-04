using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ManyToManyAttribute : Attribute
    {
        public string JoinTable { get; }
        public string JoinColumn { get; }
        public string InverseJoinColumn { get; }

        public ManyToManyAttribute(string joinTable, string joinColumn, string inverseJoinColumn)
        {
            JoinTable = joinTable;
            JoinColumn = joinColumn;
            InverseJoinColumn = inverseJoinColumn;
        }
    }
}
