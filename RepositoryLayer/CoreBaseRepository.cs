using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Helper;
using System.Text;

namespace RepositoryLayer
{
	public abstract class CoreBaseRepository 
	{
		private readonly string connectionString;

		private SqlConnection? sqlConnection = null;
		private SqlCommand? sqlCommand = null;
		private SqlDataReader? sqlDataReader = null;

		private const int SingleUserProcessCount = 1;
 		protected CoreBaseRepository(IConfiguration configuration)
		{
			connectionString = configuration?.GetConnectionString("DefaultConnection");
		}

		protected async Task<TResult?> Get<T,TResult>(
			Func<SqlDataReader,TResult> func,
			Dictionary<string,object>? parameters = null,
			string? whereConditionBody =  null)
	    {

			try
			{
				sqlConnection = OpenSqlConnection();

				var modelProperties = BaseRepositoryHelper.GetPropertyNames<TResult>();
				var modelPropertiesJoinForQuery = string.Join(", ", modelProperties);
				var commandMessage = $"SELECT {modelPropertiesJoinForQuery} FROM [{typeof(T).Name}] ";

				sqlCommand = OpenSqlCommand(commandMessage, sqlConnection);

				if (parameters is not null && parameters.Any())
				{
					foreach (var parameter in parameters)
					{
						sqlCommand.Parameters.AddWithValue(@parameter.Key, parameter.Value);
					}
				}

				if (whereConditionBody is not null)
				{
					sqlCommand.CommandText += "WHERE " + whereConditionBody;
				}

				sqlDataReader = await OpenSqlDataReader(sqlCommand);

				if(sqlDataReader.Read())
				{
					return func.Invoke(sqlDataReader);
				}
				//else
				//{
				//	throw new Exception("SqlReader is not read.");
				//}
			}
			catch (Exception ex)
			{
                Console.WriteLine($"Process is nor correct `{ex.Message}");			
			}
			finally
			{
				Dispose();		
			}

			return default(TResult);
		}




		protected async Task<int> Insert<T>(Dictionary<string,object> parameteres)
		{

			try
			{
				sqlConnection = OpenSqlConnection();

				var modelProperties = BaseRepositoryHelper.GetPropertyNames<T>();
				var modelPropertiesJoinForQuery = string.Join(", ", modelProperties);
				var parametersKeys = parameteres.Select(p => p.Key).ToArray();
				var parametersKeysJoin = string.Join(", ", parametersKeys);
				var commandMessage = $"INSERT INTO [{typeof(T).Name}] ({modelPropertiesJoinForQuery})  OUTPUT INSERTED.Id VALUES ({parametersKeysJoin})";

				sqlCommand = OpenSqlCommand(commandMessage, sqlConnection);
				BaseRepositoryHelper.GenerateParametersValues(parameteres, sqlCommand);

				var insertedId = await sqlCommand.ExecuteScalarAsync();

				if(insertedId != DBNull.Value)
				{
					return Convert.ToInt32(insertedId);
				}

				throw new Exception("User is not insert.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Process is nor correct `{ex.Message}");
			}
			finally
			{
				Dispose();
			}

			return 0;
		}

		protected async Task Update<T>(Dictionary<string, object> updatingParameters, Dictionary<string, object>? whereConditionParameters = null,
			string? whereConditionBody = null)
		{

			try
			{
				sqlConnection = OpenSqlConnection();

				var commandBuilder = new StringBuilder($"UPDATE [{typeof(T).Name}] SET ");

				commandBuilder = BaseRepositoryHelper.SetUpdatingParametersInQuery(commandBuilder,updatingParameters);

				if (whereConditionBody is not null)
				{
					commandBuilder.Append($" WHERE {whereConditionBody}");
					updatingParameters = updatingParameters.Concat(whereConditionParameters).ToDictionary();
				}

				var commandMessage = commandBuilder.ToString();
				sqlCommand = OpenSqlCommand(commandMessage, sqlConnection);

				foreach (var parameter in updatingParameters)
				{
					sqlCommand.Parameters.AddWithValue(@parameter.Key, parameter.Value);
				}

				int rowsUpdated = await sqlCommand.ExecuteNonQueryAsync();

				//if(rowsUpdated != SingleUserProcessCount)
				//{

				//}
			}
			catch (Exception ex)
			{
				throw new Exception();			
			}
			finally
			{
				Dispose();
			}
		}

		protected async Task<bool> Delete<T>(Dictionary<string,object>? parameters =  null,string? whereConditionBody = null)
		{
			try
			{
				sqlConnection = OpenSqlConnection();
				string commandMessage = $"DELETE FROM [{typeof(T).Name}] ";

				sqlCommand = OpenSqlCommand(commandMessage, sqlConnection);

				if (whereConditionBody is not null)
				{
					sqlCommand.CommandText += " WHERE " +  whereConditionBody;
					BaseRepositoryHelper.GenerateParametersValues(parameters!, sqlCommand);
				}

				int rowsDeleted = await sqlCommand.ExecuteNonQueryAsync();
				if (rowsDeleted == SingleUserProcessCount)
				{
					return true;
				}
				else
				{
					throw new Exception();
				}
				
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


		protected SqlConnection OpenSqlConnection(string? remoteConnection =  null)
		{
			SqlConnection? localConnection = null;

			if(remoteConnection is not null)
			{
                localConnection = new SqlConnection(remoteConnection);
            }
			else
			{
                localConnection = new SqlConnection(connectionString);
            }

            localConnection.Open();
			return localConnection;
		}
		protected SqlCommand OpenSqlCommand(string commandMessage, SqlConnection connection)
		{
			return new SqlCommand(commandMessage, connection);
		}

		protected async Task<SqlDataReader> OpenSqlDataReader(SqlCommand sqlCommand)
		{
			var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
			return sqlDataReader;
		}

		protected void Dispose()
		{
			sqlDataReader?.Close();
			sqlCommand?.Dispose();
			sqlConnection?.Dispose();
		}		
	}
}
