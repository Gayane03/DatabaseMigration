using BusinessLayer.Helper;
using BusinessLayer.Helper.DesignPatterns;
using BusinessLayer.Helper.ModelHelper;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services.DatabaseMigrationHelper
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        public async Task<Result<bool?>> TryMigrateTables(MigrationRequest migratedTablesRequest)
        {
			try
			{
				var migratedTablesWithoutFK = migratedTablesRequest.MigratedTablesInfoRequest.Where(table => !table.IsContainFK);

				var tasks = migratedTablesWithoutFK
					.Select(migratedTable => Task.Run(() => MigrateTableAsync(migratedTablesRequest.FromDatabaseRequest, 
					                                                          migratedTablesRequest.ToDatabaseRequest, 
																			  migratedTable))).ToArray();

				await Task.WhenAll(tasks);


				var migratedTablesWithFK = migratedTablesRequest.MigratedTablesInfoRequest.Where(table => table.IsContainFK);

				var tasksForWeaklyTables = migratedTablesWithFK
					.Select(migratedTable => Task.Run(() => MigrateTableAsync(migratedTablesRequest.FromDatabaseRequest,
																			  migratedTablesRequest.ToDatabaseRequest,
																			  migratedTable))).ToArray();

				await Task.WhenAll(tasksForWeaklyTables);

				return Result<bool?>.Success(true);
			}
			catch (Exception ex)
			{
				return Result<bool?>.Failure(Message.TableTransferProcessFail);
			}
        }

		private async Task MigrateTableAsync(ServerRequest fromServerRequest,ServerRequest toServerRequest,MigratedTableRequest migratedTable)
		{
			try
			{
				var fromServerType = fromServerRequest.ServerType;
				var fromConnectionPath = fromServerRequest.DatabaseConnection;

				var toServerType = toServerRequest.ServerType;
				var toConnectionPath = toServerRequest.DatabaseConnection;

				var migratedTableName = migratedTable.Name;

				var fromDatabaseConnector = DatabaseMigrationHelperFactory.GetFromDatabaseConnector(fromServerType,fromConnectionPath);

				var schema = await fromDatabaseConnector.GetTableSchema(migratedTableName);
				var data = await fromDatabaseConnector.GetTableData(migratedTableName);

				using (var toDatabaseConnector = DatabaseMigrationHelperFactory.GetToDatabaseConnector(toServerType,toConnectionPath))

				await toDatabaseConnector.StructedTableWithData(migratedTableName, fromServerType, schema, data);

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
