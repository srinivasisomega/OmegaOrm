using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ForeignKeyAttribute : Attribute
    {
        public string ReferencedTable { get; }
        public string ReferencedColumn { get; }

        public ForeignKeyAttribute(string referencedTable, string referencedColumn)
        {
            ReferencedTable = referencedTable;
            ReferencedColumn = referencedColumn;
        }
    }
}
