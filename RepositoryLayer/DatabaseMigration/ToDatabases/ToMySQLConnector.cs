using MySql.Data.MySqlClient;
using RepositoryLayer.DatabaseMigration.BaseDatabases;
using RepositoryLayer.Helper;
using SharedLibrary.DbModels;
using SharedLibrary.Enum;
using System.Text;

namespace RepositoryLayer.DatabaseMigration.ToDatabases
{
	public class ToMySQLConnector : BaseMySQLRepository, IToDatabaseConnector
	{
		public ToMySQLConnector(string connectionPath) : base(connectionPath) { }

		public async Task CreateTable(ServerType fromServerType, string tableName, IEnumerable<ColumnInfo> columnsInfo)
		{
			try
			{
				await OpenConnectionAsync();
				await BeginTransactionAsync();

				var createTableQuery = new StringBuilder();
				createTableQuery.Append($"CREATE TABLE `{tableName}` (");

				var primaryKeyColumns = new List<string>();

				foreach (var column in columnsInfo)
				{
					createTableQuery.Append($"`{column.Name}` {ColumnTypeGenerator.GetColumnDatatType(fromServerType, ServerType.MySQL, column?.DataType)} ");

					if (!column.IsNullable)
					{
						createTableQuery.Append("NOT NULL ");
					}

					if (column.IsPrimaryKey)
					{
						primaryKeyColumns.Add($"`{column.Name}`");
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
				//SqlCommand!.ExecuteNonQuery();

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
                var escapedTableName = $"`{tableName}`";
                var columns = string.Join(", ", tableData.First().Keys.Select(key => $"`{key}`"));
                var valuesList = new List<string>();
                var parameters = new List<MySqlParameter>();

                int paramCounter = 0; // Ensure unique parameter names

                foreach (var row in tableData)
                {
                    var paramNames = new List<string>();

                    foreach (var keyValue in row)
                    {
                        string paramName = $"@p{paramCounter}";
                        paramNames.Add(paramName);
                        parameters.Add(new MySqlParameter(paramName, keyValue.Value ?? DBNull.Value));
                        paramCounter++; // Increment after adding the parameter
                    }

                    valuesList.Add($"({string.Join(", ", paramNames)})");
                }

                // Build the final query with all the rows in one insert statement
                string query = $" INSERT INTO {escapedTableName} ({columns}) VALUES {string.Join(", ", valuesList)}";
				SqlCommand.CommandText += query;
                // Execute the query in one go
                //OpenCommand(query);
                SqlCommand!.Parameters.AddRange(parameters.ToArray());
                await SqlCommand!.ExecuteNonQueryAsync();


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
