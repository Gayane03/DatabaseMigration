using MySql.Data.MySqlClient;
using System.Data;

namespace RepositoryLayer.DatabaseMigration.BaseDatabases
{
	public abstract class BaseMySQLRepository : BaseRepository
	{
		protected BaseMySQLRepository(string connectionPath) : base(connectionPath) { }

		public override async Task OpenConnectionAsync()
		{
			if (SqlConnection == null || SqlConnection.State != ConnectionState.Open)
			{
				SqlConnection = new MySqlConnection(connectionPath);
				await SqlConnection.OpenAsync();
			}
		}
		public override void OpenCommand(string commandText)
		{
			SqlCommand = new MySqlCommand(commandText, (MySqlConnection)SqlConnection);
		}
	}

}
