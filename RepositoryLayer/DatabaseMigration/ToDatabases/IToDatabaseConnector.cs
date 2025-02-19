using RepositoryLayer.DatabaseMigration.BaseDatabases;
using SharedLibrary.DbModels;
using SharedLibrary.Enum;

namespace RepositoryLayer.DatabaseMigration.ToDatabases
{
    public interface IToDatabaseConnector : IDatabaseTransaction, IDisposable
    {
		public async Task StructedTableWithData(
			string fromTableName,
			ServerType fromServerType,
			IEnumerable<ColumnInfo> columnsInfo,
			IEnumerable<Dictionary<string, object>> tableData)
		{
			try
			{
				await CreateTable(fromServerType, fromTableName, columnsInfo);

				await InsertTableData(fromTableName, tableData);

				await CommitTransaction();
			}
			catch (Exception ex)
			{
				await RollbackTransaction();
				throw;
			}
			finally
			{
				Dispose();
			}
		}
		 Task CreateTable(ServerType fromServerType, string tableName, IEnumerable<ColumnInfo> columnsInfo);
         Task InsertTableData(string tableName, IEnumerable<Dictionary<string, object>> tableData);
    }
}
