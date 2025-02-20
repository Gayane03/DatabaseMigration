using RepositoryLayer.DatabaseMigration.BaseDatabases;
using RepositoryLayer.Helper;
using SharedLibrary.DbModels;
using SharedLibrary.Enum;

namespace RepositoryLayer.DatabaseMigration.FromDatabases
{
	public class FromPostgreSQLConnector : BasePostgreSQLRepository, IFromDatabaseConnector
	{
		private const ServerType serverType = ServerType.PostgreSQL;
		public FromPostgreSQLConnector(string connection) : base(connection) { }

		public async Task<IEnumerable<ColumnInfo>> GetTableSchema(string tableName)
		{
			var columnsInfo = new List<ColumnInfo>();
			try
			{
				await OpenConnectionAsync();
				var query = BaseRepositoryHelper.GenerateTableSchemaQuery(tableName, serverType);

				OpenCommand(query);
				await OpenDataReaderAsync();

				while (SqlReader!.Read())
				{
					var columnInfo = new ColumnInfo
					{
						Name = SqlReader!["column_name"].ToString(),
						DataType = SqlReader!["data_type"].ToString(),
						MaxLength = SqlReader!["character_maximum_length"] as int?,
						IsPrimaryKey = Convert.ToBoolean(SqlReader!["is_primary_key"]),
						IsNullable = Convert.ToBoolean(SqlReader!["is_nullable"])
					};

					columnsInfo.Add(columnInfo);
				}

				return columnsInfo;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				Dispose();
			}
		}
		public async Task<IEnumerable<Dictionary<string, object>>> GetTableData(string tableName)
		{
			var tableData = new List<Dictionary<string, object>>();

			try
			{
				await OpenConnectionAsync();

				string query = $"SELECT * FROM \"{tableName}\" ";
				OpenCommand(query);

				await OpenDataReaderAsync();
				while (SqlReader!.Read())
				{
					var row = new Dictionary<string, object>();
					for (int i = 0; i < SqlReader!.FieldCount; i++)
					{
						row[SqlReader!.GetName(i)] = SqlReader!.IsDBNull(i) ? null : SqlReader!.GetValue(i);
					}

					tableData.Add(row);
				}

				return tableData;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				Dispose();
			}
		}
	}
}
