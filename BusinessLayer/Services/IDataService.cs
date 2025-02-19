using BusinessLayer.Helper.ModelHelper;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services
{
    public interface IDataService
    {
        Task<Result<IEnumerable<MigratedTableRequest>>> GetTables(ServerRequest databaseRequest);
          
    }
}
