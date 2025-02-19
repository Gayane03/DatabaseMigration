using Microsoft.Extensions.Configuration;
using SharedLibrary.RequestModels;
using System.Data;

namespace RepositoryLayer
{
    public class MigrationRepository : CoreBaseRepository, IMigrationRepository
    {
        public MigrationRepository(IConfiguration configuration) : base(configuration){}


        public async Task<IEnumerable<string>> GetTables(ServerRequest databaseRequest)
        {
            DataTable? tables = null;
            List<string> tablesNames = new List<string>();
            try
            {
               var sqlConnection =  OpenSqlConnection(databaseRequest.DatabaseConnection);

                tables = await sqlConnection.GetSchemaAsync("Tables");
                
                foreach (DataRow row in tables.Rows)
                {
                    tablesNames.Add(row["TABLE_NAME"].ToString());
                }

                return tablesNames;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            finally
            {
                tables?.Dispose();
                Dispose();
            }
        }

    
    }
}
