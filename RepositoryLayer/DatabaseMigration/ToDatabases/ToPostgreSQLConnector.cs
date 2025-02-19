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
                foreach (var row in tableData)
                {
                    var escapedTableName = $"\"{tableName}\"";

                    var columns = string.Join(", ", row.Keys.Select(key => $"\"{key}\""));

                    var parameters = string.Join(", ", row.Keys.Select((_, index) => $"@p{index}"));

                    string query = $"INSERT INTO {escapedTableName} ({columns}) VALUES ({parameters})";

                    OpenCommand(query);
                    int paramIndex = 0;
                    foreach (var keyValue in row)
                    {
                        SqlCommand!.Parameters.Add(new NpgsqlParameter($"@p{paramIndex}", keyValue.Value ?? DBNull.Value));
                        paramIndex++;
                    }
                    await SqlCommand!.ExecuteNonQueryAsync();
                }
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
