using Microsoft.Extensions.Configuration;
using SharedLibrary.RequestModels;


namespace RepositoryLayer
{
    public class MigrationRepository : CoreBaseRepository, IMigrationRepository
    {
        public MigrationRepository(IConfiguration configuration) : base(configuration){}



		public async Task<Dictionary<string, List<string>?>> GetTableRelationships(ServerRequest databaseRequest)
		{
			Dictionary<string, List<string>?> tableRelationships = new Dictionary<string, List<string>?>();

			try
			{
				//if(databaseRequest.DatabaseConnection.IsNullOrEmpty())
				//{
				//	throw new SystemException();
				//}

				var sqlConnection = OpenSqlConnection(databaseRequest.DatabaseConnection);
				string query = string.Empty;

			
					query = @"
               SELECT 
                   t.name AS TableName,
                  ISNULL(pk_tab.name, NULL) AS ReferencedTable
              FROM sys.tables t
              LEFT JOIN sys.foreign_keys fk ON t.object_id = fk.parent_object_id
              LEFT JOIN sys.tables pk_tab ON fk.referenced_object_id = pk_tab.object_id;";
			

			   var command = OpenSqlCommand(query, sqlConnection);
			   var reader = await OpenSqlDataReader(command);

				while (await reader.ReadAsync())
				{
					string tableName = reader.GetString(0);
					string referencedTable = reader.IsDBNull(1) ? null : reader.GetString(1);

					if (!tableRelationships.ContainsKey(tableName))
						tableRelationships[tableName] = null; // Default to null for tables without relations

					if (referencedTable != null)
					{
						if (tableRelationships[tableName] == null)
							tableRelationships[tableName] = new List<string>();

						tableRelationships[tableName]!.Add(referencedTable);
					}
				}

				return tableRelationships;
			}
			catch (SystemException ex)
			{
				throw new SystemException("There is not db connection");
			}
			catch (Exception ex)
			{
				throw new Exception("Error retrieving table relationships", ex);
			}
		}





		//public async Task<IEnumerable<string>> GetTables(ServerRequest databaseRequest)
  //      {
  //          DataTable? tables = null;
  //          List<string> tablesNames = new List<string>();
  //          try
  //          {
  //             var sqlConnection =  OpenSqlConnection(databaseRequest.DatabaseConnection);

  //              tables = await sqlConnection.GetSchemaAsync("Tables");
                
  //              foreach (DataRow row in tables.Rows)
  //              {
  //                  tablesNames.Add(row["TABLE_NAME"].ToString());
  //              }

  //              return tablesNames;
  //          }
  //          catch (Exception ex)
  //          {
  //              throw new Exception();
  //          }
  //          finally
  //          {
  //              tables?.Dispose();
  //              Dispose();
  //          }
  //      }

    
    }
}
