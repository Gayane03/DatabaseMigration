using Microsoft.Data.SqlClient;
using System.Data;

namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
    public abstract class BaseMSSQLRepository : BaseRepository, IBaseRepository, IDisposable
    {
        protected BaseMSSQLRepository(string connectionPath) : base(connectionPath) { }
     
        public async Task OpenConnectionAsync()
        {
            if (SqlConnection == null || SqlConnection.State != ConnectionState.Open)
            {
                SqlConnection = new SqlConnection(connectionPath);
                await SqlConnection.OpenAsync();
            }
        }

        public void OpenCommand(string commandText)
        {
            SqlCommand = new SqlCommand(commandText, (SqlConnection)SqlConnection);
        }
    }
}
