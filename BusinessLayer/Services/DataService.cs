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

        public async Task<Result<IEnumerable<MigratedTableResponse>>> GetTables(ServerRequest databaseRequest)
        {
            try
            {
                var tablesWithReferencedTables = await migrationRepository.GetTableRelationships(databaseRequest);

				if (tablesWithReferencedTables == default(IEnumerable<string>) || !tablesWithReferencedTables.Any())
				{
					return Result<IEnumerable<MigratedTableResponse>>.Failure(Message.TableGettingProblem);
				}


				var migratedTableRequest = new List<MigratedTableResponse>();

                foreach (var table in tablesWithReferencedTables)
                {
                    var isContainFK = table.Value?.Any() ?? false;
                    migratedTableRequest.Add(new MigratedTableResponse() { Name = table.Key, IsContainFK = isContainFK , ReferencedTableNames = table.Value });
                }

				return Result<IEnumerable<MigratedTableResponse>>.Success(migratedTableRequest);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MigratedTableResponse>>.Failure(Message.SystemError);
            }
        }
    }
}
