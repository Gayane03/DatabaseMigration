using BusinessLayer.Helper;
using BusinessLayer.Helper.ModelHelper;
using RepositoryLayer;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services
{
    public class DataService : IDataService
    {
        private readonly IMigrationRepository migrationRepository;
        public DataService(IMigrationRepository migrationRepository)
        {
                this.migrationRepository = migrationRepository;
        }

        public async Task<Result<IEnumerable<MigratedTableRequest>>> GetTables(ServerRequest databaseRequest)
        {
            try
            {
                var tables = await migrationRepository.GetTables(databaseRequest);

                var migratedTableRequest = new List<MigratedTableRequest>();

                foreach (var table in tables)
                {
                    migratedTableRequest.Add(new MigratedTableRequest() { TableName = table ,IsContainFK = false});
                }

                if (tables == default(IEnumerable<string>) || !tables.Any()) 
                {
                    return Result<IEnumerable<MigratedTableRequest>>.Failure(Message.TableGettingProblem);
                }

                return Result<IEnumerable<MigratedTableRequest>>.Success(migratedTableRequest);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MigratedTableRequest>>.Failure(Message.SystemError);
            }
        }
    }
}
