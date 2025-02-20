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

                foreach (var column in columnsInfo)
                {
                    createTableQuery.Append($"\"{column.Name}\" {ColumnTypeGenerator.GetColumnDatatType(fromServerType, ServerType.PostgreSQL, column?.DataType)} ");

                    if (!column.IsNullable)
                    {
                        createTableQuery.Append("NOT NULL ");
                    }

                    if (column.IsPrimaryKey)
                    {
                        primaryKeyColumns.Add($"\"{column.Name}\"");
                    }

                    createTableQuery.Append(", ");
                }

                if (primaryKeyColumns.Count > 0)
                {
                    createTableQuery.Append($"PRIMARY KEY ({string.Join(", ", primaryKeyColumns)}), ");
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
