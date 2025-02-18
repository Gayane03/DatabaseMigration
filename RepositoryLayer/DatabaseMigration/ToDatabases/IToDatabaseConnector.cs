using SharedLibrary.DbModels;
using SharedLibrary.Enum;

namespace RepositoryLayer.DatabaseMigration.ToDatabases
{
    public interface IToDatabaseConnector
    {
        Task CreateTable(ServerType fromServerType, string tableName, IEnumerable<ColumnInfo> columnsInfo);
        Task InsertTableData(string tableName, IEnumerable<Dictionary<string, object>> tableData);
    }
}
