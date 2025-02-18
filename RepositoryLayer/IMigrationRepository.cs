using SharedLibrary.RequestModels;

namespace RepositoryLayer
{
    public interface IMigrationRepository
    {
        Task<IEnumerable<string>> GetTables(DatabaseRequest databaseRequest);
        //Task<bool> TransferTable(TransferTableRequest transferTableRequest);
    }
}
