using SharedLibrary.RequestModels;

namespace RepositoryLayer
{
    public interface IMigrationRepository
    {
		Task<Dictionary<string, List<string>?>> GetTableRelationships(ServerRequest databaseRequest);
		//Task<bool> TransferTable(TransferTableRequest transferTableRequest);
	}
}
