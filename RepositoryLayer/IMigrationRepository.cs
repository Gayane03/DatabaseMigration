using SharedLibrary.RequestModels;

namespace RepositoryLayer
{
    public interface IMigrationRepository
    {
        Task<IEnumerable<string>> GetTables(ServerRequest databaseRequest);
        //Task<bool> TransferTable(TransferTableRequest transferTableRequest);
    }
}
