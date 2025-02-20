using Microsoft.Data.SqlClient;
using System.Data;

namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
	public abstract class BaseMSSQLRepository : BaseRepository
	{
		protected BaseMSSQLRepository(string connectionPath) : base(connectionPath) { }

		public override async Task OpenConnectionAsync()
		{
			if (SqlConnection == null || SqlConnection.State != ConnectionState.Open)
			{
				SqlConnection = new SqlConnection(connectionPath);             
                await SqlConnection.OpenAsync();
			}
		}

		public override void OpenCommand(string commandText)
		{
			SqlCommand = new SqlCommand(commandText, (SqlConnection)SqlConnection);
            SqlCommand!.CommandTimeout = 300;
        }
	}
}
