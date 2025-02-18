using Npgsql;
using System.Data;

namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
    public abstract class BasePostgreSQLRepository : BaseRepository, IBaseRepository
    {
        public BasePostgreSQLRepository(string connectionPath) : base(connectionPath) { }

        public async Task OpenConnectionAsync()
        {
            if (SqlConnection == null || SqlConnection.State != ConnectionState.Open)
            {
                SqlConnection = new NpgsqlConnection(connectionPath);
                await SqlConnection.OpenAsync();
            }
        }

        public void OpenCommand(string commandText)
        {
            SqlCommand = new NpgsqlCommand(commandText, (NpgsqlConnection)SqlConnection);          
        }

    }
}
