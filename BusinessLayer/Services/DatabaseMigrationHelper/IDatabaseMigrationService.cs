using BusinessLayer.Helper.ModelHelper;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services.DatabaseMigrationHelper
{
    public interface IDatabaseMigrationService
    {
        Task<Result<bool>> TransferTables(IEnumerable<TransferTableRequest> transferTableRequest);
    }
}
