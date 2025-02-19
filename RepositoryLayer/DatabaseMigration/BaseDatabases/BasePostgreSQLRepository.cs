using Npgsql;
using System.Data;

namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
    public abstract class BasePostgreSQLRepository : BaseRepository
    {
        public BasePostgreSQLRepository(string connectionPath) : base(connectionPath) { }

        public override async Task OpenConnectionAsync()
        {
            if (SqlConnection == null || SqlConnection.State != ConnectionState.Open)
            {
                SqlConnection = new NpgsqlConnection(connectionPath);
                await SqlConnection.OpenAsync();
            }
        }

        public override void OpenCommand(string commandText)
        {
            SqlCommand = new NpgsqlCommand(commandText, (NpgsqlConnection)SqlConnection);          
        }
    }
}
