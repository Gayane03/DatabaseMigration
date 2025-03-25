using Npgsql;
using RepositoryLayer.DatabaseMigration.BaseDatabases;
using RepositoryLayer.Helper;
using SharedLibrary.DbModels;
using SharedLibrary.Enum;
using System.Text;

namespace RepositoryLayer.DatabaseMigration.ToDatabases
{
    public class ToPostgreSQLConnector : BasePostgreSQLRepository, IToDatabaseConnector
    {
        public ToPostgreSQLConnector(string connectionPath) : base(connectionPath) {}

		public async Task CreateTable(ServerType fromServerType, string tableName, IEnumerable<ColumnInfo> columnsInfo)
        {
            try
            {
                await OpenConnectionAsync();
                await BeginTransactionAsync();

				var createTableQuery = new StringBuilder();
                createTableQuery.Append($"CREATE TABLE \"{tableName}\" (");

                var primaryKeyColumns = new List<string>();
				var uniqueConstraints = new List<string>();
				var foreignKeys = new List<string>();

				foreach (var column in columnsInfo)
                {
                    var columnType = ColumnTypeGenerator.GetColumnDataType(fromServerType, ServerType.PostgreSQL, column?.DataType);

					createTableQuery.Append($"\"{column.Name}\" {columnType} ");

                    if (!column.IsNullable)
                    {
                        createTableQuery.Append("NOT NULL ");
                    }

                    if (column.IsPrimaryKey)
                    {
                        primaryKeyColumns.Add($"\"{column.Name}\"");
                    }

					if (column.HasDefault && !string.IsNullOrEmpty(column.DefaultValue))
					{
						createTableQuery.Append($"DEFAULT {ColumnTypeGenerator.GetColumnDefaultValue(fromServerType, ServerType.PostgreSQL,column.DefaultValue, column?.DataType)}");
					}

					if (column.IsUnique)
					{
						uniqueConstraints.Add($"UNIQUE (\"{column.Name}\")");
					}

					if (column.IsForeignKey && !string.IsNullOrEmpty(column.ForeignKeyTable) && !string.IsNullOrEmpty(column.ForeignKeyColumn))
					{
						foreignKeys.Add($"FOREIGN KEY (\"{column.Name}\") REFERENCES \"{column.ForeignKeyTable}\" (\"Id\")");
					}

					createTableQuery.Append(", ");
                }

                if (primaryKeyColumns.Count > 0)
                {
                    createTableQuery.Append($"PRIMARY KEY ({string.Join(", ", primaryKeyColumns)}), ");
                }

				if (uniqueConstraints.Count > 0)
				{
					createTableQuery.Append(string.Join(", ", uniqueConstraints) + ", ");
				}

				if (foreignKeys.Count > 0)
				{
					createTableQuery.Append(string.Join(", ", foreignKeys) + ", ");
				}

				createTableQuery.Length -= 2;
                createTableQuery.Append(");");

                var commandText = createTableQuery.ToString();
                OpenCommand(commandText);
                SqlCommand!.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task InsertTableData(string tableName, IEnumerable<Dictionary<string, object>> tableData)
        {
            try
            {
                if(tableData==  null || !tableData.Select(table => table.Keys).Any() || !tableData.Select(table => table.Values).Any())
                {
                    return;
                }


                var escapedTableName = $"\"{tableName}\"";
                var columns = string.Join(", ", tableData.First().Keys.Select(key => $"\"{key}\""));

                var valuesList = new List<string>(); // Collects value placeholders
                var parameters = new List<NpgsqlParameter>(); // Collects parameters
                int paramIndex = 0;

                foreach (var row in tableData)
                {
                    var paramNames = row.Keys.Select(_ => $"@p{paramIndex++}").ToList();
                    valuesList.Add($"({string.Join(", ", paramNames)})");

                    parameters.AddRange(row.Values.Select((value, i) => new NpgsqlParameter(paramNames[i], value ?? DBNull.Value)));
                }

                string query = $"INSERT INTO {escapedTableName} ({columns}) VALUES {string.Join(", ", valuesList)}";

                using var command = new NpgsqlCommand(query, (NpgsqlConnection)SqlConnection);
                command.Parameters.AddRange(parameters.ToArray());
                await command.ExecuteNonQueryAsync(); // 🚀 Executes everything in one call
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		public async Task CommitTransaction()
		{
			await SqlTransaction!.CommitAsync();
		}

		public async Task RollbackTransaction()
		{
		   await SqlTransaction!.RollbackAsync();
		}

		public void Dispose()
		{
            base.Dispose();
		}
	}
}
