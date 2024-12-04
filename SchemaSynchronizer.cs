using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using OmegaOrm.Attributes;

namespace OmegaOrm
{
    public static class SchemaSynchronizer
    {
        public static void SynchronizeDatabase(string connectionString, IEnumerable<Type> modelTypes)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var existingTables = GetDatabaseTables(connection);

                foreach (var modelType in modelTypes)
                {
                    var tableAttr = modelType.GetCustomAttribute<TableAttribute>();
                    if (tableAttr == null)
                        continue;

                    var tableName = tableAttr.Name;

                    if (existingTables.ContainsKey(tableName))
                    {
                        UpdateTableSchema(connection, tableName, modelType, existingTables[tableName]);
                    }
                    else
                    {
                        CreateTable(connection, tableName, modelType);
                    }

                    // Handle join tables for many-to-many relationships
                    var manyToManyProps = modelType.GetProperties()
                        .Where(p => p.IsDefined(typeof(ManyToManyAttribute)));
                    foreach (var prop in manyToManyProps)
                    {
                        var joinTableScript = SchemaGenerator.GenerateJoinTableScript(prop); // Call the static method from SchemaGenerator
                        if (!string.IsNullOrEmpty(joinTableScript))
                        {
                            using (var command = new SqlCommand(joinTableScript, connection))
                            {
                                command.ExecuteNonQuery();
                                Console.WriteLine($"Created join table for property {prop.Name}");
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<string, Dictionary<string, string>> GetDatabaseTables(SqlConnection connection)
        {
            var tables = new Dictionary<string, Dictionary<string, string>>();

            var query = "SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS";
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tableName = reader.GetString(0);
                    var columnName = reader.GetString(1);
                    var dataType = reader.GetString(2);

                    if (!tables.ContainsKey(tableName))
                        tables[tableName] = new Dictionary<string, string>();

                    tables[tableName][columnName] = dataType;
                }
            }

            return tables;
        }

        private static void CreateTable(SqlConnection connection, string tableName, Type modelType)
        {
            var createScript = SchemaGenerator.GenerateTableScript(modelType);
            using (var command = new SqlCommand(createScript, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"Created table: {tableName}");
            }
        }

        private static void UpdateTableSchema(SqlConnection connection, string tableName, Type modelType, Dictionary<string, string> existingColumns)
        {
            var properties = modelType.GetProperties()
                .Where(p => p.IsDefined(typeof(ColumnAttribute)))
                .ToList();

            foreach (var property in properties)
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttr.Name;
                var columnType = SchemaGenerator.GetSqlType(property.PropertyType, columnAttr.Length);

                if (!existingColumns.ContainsKey(columnName))
                {
                    var alterScript = $"ALTER TABLE [{tableName}] ADD [{columnName}] {columnType} {(columnAttr.IsNullable ? "NULL" : "NOT NULL")};";
                    using (var command = new SqlCommand(alterScript, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Added column: {columnName} to table: {tableName}");
                    }
                }
                else
                {
                    // Optional: Add logic to update column type, constraints, or other properties.
                    Console.WriteLine($"Column {columnName} already exists in table {tableName}, skipping.");
                }
            }
        }
    }

}
