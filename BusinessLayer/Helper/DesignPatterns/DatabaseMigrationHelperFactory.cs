using RepositoryLayer.DatabaseMigration;
using RepositoryLayer.DatabaseMigration.FromDatabases;
using RepositoryLayer.DatabaseMigration.ToDatabases;
using SharedLibrary.Enum;


namespace BusinessLayer.Helper.DesignPatterns
{
    public  static class DatabaseMigrationHelperFactory
    {
        public static IFromDatabaseConnector GetFromDatabaseConnector(ServerType serverType, string connectionPath)
        {
            return serverType switch
            {
                ServerType.MSSQL => new FromMSSQLConnector(connectionPath),
                //ServerType.PostgreSQL => new PostgreSqlControlRepository(connectionString),
                // "mysql" => new MySQLHelper(connectionString),
                _ => throw new ArgumentException("Unsupported database type.")
            };
        }

        public static IToDatabaseConnector GetToDatabaseConnector(ServerType serverType, string connectionPath)
        {
            return serverType switch
            {
                //ServerType.MSSQL => new FromMSSQLConnector(connectionPath),
                ServerType.PostgreSQL => new ToPostgreSQLConnector(connectionPath),
                // "mysql" => new MySQLHelper(connectionString),
                _ => throw new ArgumentException("Unsupported database type.")
            };
        }
    }
}
