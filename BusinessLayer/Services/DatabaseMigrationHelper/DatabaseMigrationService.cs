using AutoMapper.Configuration.Annotations;
using BusinessLayer.Helper;
using BusinessLayer.Helper.DesignPatterns;
using BusinessLayer.Helper.ModelHelper;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services.DatabaseMigrationHelper
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {

        public async Task<Result<bool>> TransferTables(IEnumerable<TransferTableRequest> transferTableRequest)
        {
			try
			{
				var tablesNonContainFK = transferTableRequest.Where(table => !table.IsContainFK).FirstOrDefault();

                var fromDatabase = DatabaseMigrationHelperFactory.GetFromDatabaseConnector(tablesNonContainFK.FromServerType, tablesNonContainFK.FromServerConnection);
				var currentTableColumnInfo = await fromDatabase.GetTableSchema(tablesNonContainFK!.FromTableName);
				var currentTableData = await fromDatabase.GetTableData(tablesNonContainFK!.FromTableName);	

				var toDatabase = DatabaseMigrationHelperFactory.GetToDatabaseConnector(tablesNonContainFK.ToServerType, tablesNonContainFK.ToServerConnection);
                await toDatabase.CreateTable(tablesNonContainFK.FromServerType, tablesNonContainFK.FromTableName, currentTableColumnInfo);
				await toDatabase.InsertTableData(tablesNonContainFK.FromTableName,currentTableData);

				return Result<bool>.Success(true);
			}
			catch (Exception ex)
			{
				return Result<bool>.Failure(Message.TableTransferProcessFail);
			}
        }
    }
}
