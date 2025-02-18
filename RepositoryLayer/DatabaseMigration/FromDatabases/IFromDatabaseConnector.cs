using SharedLibrary.DbModels;

namespace RepositoryLayer.DatabaseMigration.FromDatabases
{
    public interface IFromDatabaseConnector
    {
        Task<IEnumerable<ColumnInfo>> GetTableSchema(string tableName);
        Task<IEnumerable<Dictionary<string, object>>> GetTableData(string tableName);
    }
}
