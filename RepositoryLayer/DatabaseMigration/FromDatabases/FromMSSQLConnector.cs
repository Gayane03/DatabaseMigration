using RepositoryLayer.DatabaseMigration.BaseDatabases;
using RepositoryLayer.Helper;
using SharedLibrary.DbModels;
using SharedLibrary.Enum;

namespace RepositoryLayer.DatabaseMigration.FromDatabases
{
    public class FromMSSQLConnector : BaseMSSQLRepository, IFromDatabaseConnector
    {
        private const ServerType serverType = ServerType.MSSQL;
        public FromMSSQLConnector(string connection) : base(connection){}

        public async Task<IEnumerable<ColumnInfo>> GetTableSchema(string tableName)
        {
            var columnsInfo = new List<ColumnInfo>();
            try
            {
                await OpenConnectionAsync();

               
                var query = BaseRepositoryHelper.GenerateTableSchemaQuery(tableName, serverType); 

                OpenCommand(query);
                await OpenDataReaderAsync();
               
                while (SqlReader!.Read())
                {
                    var columnInfo = new ColumnInfo
                    {
						Name = SqlReader!["COLUMN_NAME"].ToString(),
						DataType = SqlReader!["DATA_TYPE"].ToString(),
						MaxLength = SqlReader["MaxLength"] as int?,
						IsPrimaryKey = Convert.ToBoolean(SqlReader["IsPrimaryKey"]),
						IsNullable = Convert.ToBoolean(SqlReader["IsNullable"]),
						IsForeignKey = Convert.ToBoolean(SqlReader["IsForeignKey"]),
						ForeignKeyTable = SqlReader["ForeignKeyTable"] as string,
						ForeignKeyColumn = SqlReader["ForeignKeyColumn"] as string,
						IsUnique = Convert.ToBoolean(SqlReader["IsUnique"]),
						HasDefault = SqlReader["DefaultValue"] != DBNull.Value,
						DefaultValue = SqlReader["DefaultValue"] as string ?? string.Empty,
					};

                    columnsInfo.Add(columnInfo);
                }
                
                return columnsInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }

        }
        public async Task<IEnumerable<Dictionary<string, object>>> GetTableData(string tableName)
        {
            var tableData = new List<Dictionary<string, object>>();

            try
            {
                await OpenConnectionAsync();
               
                string query = $"SELECT * FROM [{tableName}]";
                OpenCommand(query);

                await OpenDataReaderAsync(); 

                while (SqlReader!.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < SqlReader!.FieldCount; i++)
                    {
                        row[SqlReader!.GetName(i)] = SqlReader!.IsDBNull(i) ? null : SqlReader!.GetValue(i);
                    }

                    tableData.Add(row);
                }
           
                return tableData;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }

        }
    }
}
