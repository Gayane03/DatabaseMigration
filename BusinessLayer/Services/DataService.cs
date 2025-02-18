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

        public async Task<Result<IEnumerable<string>>> GetTables(DatabaseRequest databaseRequest)
        {
            try
            {
                var tables = await migrationRepository.GetTables(databaseRequest);

                if (tables == default(IEnumerable<string>) || !tables.Any()) 
                {
                    return Result<IEnumerable<string>>.Failure(Message.TableGettingProblem);
                }

                return Result<IEnumerable<string>>.Success(tables);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<string>>.Failure(Message.SystemError);
            }
        }
    }
}
