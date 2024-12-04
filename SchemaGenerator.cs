using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using OmegaOrm.Attributes;
namespace OmegaOrm
{
        public static class SchemaGenerator
        {
        public static string GenerateTableScript(Type modelType)
        {
            if (!modelType.IsDefined(typeof(TableAttribute), false))
                throw new InvalidOperationException($"Class {modelType.Name} does not have a [Table] attribute.");

            var tableAttr = modelType.GetCustomAttribute<TableAttribute>();
            var tableName = tableAttr.Name;

            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE [{tableName}] (");

            var properties = modelType.GetProperties();
            var primaryKeys = new List<string>();
            var foreignKeys = new List<string>();

            foreach (var property in properties)
            {
                var columnScript = GenerateColumnScript(property);
                if (!string.IsNullOrEmpty(columnScript))
                {
                    sb.AppendLine(columnScript + ",");
                }

                if (property.IsDefined(typeof(PrimaryKeyAttribute), false))
                {
                    var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                    primaryKeys.Add($"[{columnAttr?.Name ?? property.Name}]");
                }

                if (property.IsDefined(typeof(ForeignKeyAttribute), false))
                {
                    var foreignKeyAttr = property.GetCustomAttribute<ForeignKeyAttribute>();
                    var columnName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
                    foreignKeys.Add($"FOREIGN KEY ([{columnName}]) REFERENCES [{foreignKeyAttr.ReferencedTable}] ([{foreignKeyAttr.ReferencedColumn}])");
                }
            }

            if (primaryKeys.Any())
            {
                sb.AppendLine($"    PRIMARY KEY ({string.Join(", ", primaryKeys)})");
            }

            foreach (var foreignKey in foreignKeys)
            {
                sb.AppendLine($"    {foreignKey},");
            }

            sb.Length--; // Remove the last comma
            sb.AppendLine(");");
            return sb.ToString();
        }
        public static string GenerateJoinTableScript(PropertyInfo property)
        {
            if (!property.IsDefined(typeof(ManyToManyAttribute), false))
                return null;

            var manyToManyAttr = property.GetCustomAttribute<ManyToManyAttribute>();
            var tableName = manyToManyAttr.JoinTable;
            var joinColumn = manyToManyAttr.JoinColumn;
            var inverseJoinColumn = manyToManyAttr.InverseJoinColumn;

            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE [{tableName}] (");
            sb.AppendLine($"    [{joinColumn}] INT NOT NULL,");
            sb.AppendLine($"    [{inverseJoinColumn}] INT NOT NULL,");
            sb.AppendLine($"    PRIMARY KEY ([{joinColumn}], [{inverseJoinColumn}]),");
            sb.AppendLine($"    FOREIGN KEY ([{joinColumn}]) REFERENCES [{property.DeclaringType.GetCustomAttribute<TableAttribute>().Name}] ([Id]),");
            sb.AppendLine($"    FOREIGN KEY ([{inverseJoinColumn}]) REFERENCES [{property.PropertyType.GetGenericArguments()[0].GetCustomAttribute<TableAttribute>().Name}] ([Id])");
            sb.AppendLine(");");

            return sb.ToString();
        }


        private static string GenerateColumnScript(PropertyInfo property)
            {
                if (!property.IsDefined(typeof(ColumnAttribute), false)) return null;

                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttr.Name;
                var columnType = GetSqlType(property.PropertyType, columnAttr.Length);
                var isNullable = columnAttr.IsNullable ? "NULL" : "NOT NULL";

                var sb = new StringBuilder();
                sb.Append($"    [{columnName}] {columnType} {isNullable}");

                if (property.IsDefined(typeof(PrimaryKeyAttribute), false))
                {
                    var pkAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
                    if (pkAttr.IsIdentity)
                    {
                        sb.Append(" IDENTITY(1,1)");
                    }
                }

                if (property.IsDefined(typeof(DefaultValueAttribute), false))
                {
                    var defaultAttr = property.GetCustomAttribute<DefaultValueAttribute>();
                    sb.Append($" DEFAULT '{defaultAttr.Value}'");
                }

                return sb.ToString();
            }

            public static string GetSqlType(Type type, int length)
            {
                if (type == typeof(int)) return "INT";
                if (type == typeof(string)) return length > 0 ? $"NVARCHAR({length})" : "NVARCHAR(MAX)";
                if (type == typeof(DateTime)) return "DATETIME";
                if (type == typeof(bool)) return "BIT";
                if (type == typeof(decimal)) return "DECIMAL(18, 2)";
                if (type == typeof(float)) return "FLOAT";
                if (type == typeof(double)) return "DOUBLE";

                throw new NotSupportedException($"Type {type.Name} is not supported.");
            }
        }
    


}
