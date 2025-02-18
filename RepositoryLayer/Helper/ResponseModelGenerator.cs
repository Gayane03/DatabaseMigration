using Microsoft.Data.SqlClient;
using SharedLibrary.DbModels.Response;

namespace RepositoryLayer.Helper
{
	internal static class ResponseModelGenerator
	{
		public static UserActivityDataResponse GetUserActivityData(SqlDataReader reader)
		{
			return new UserActivityDataResponse()
			{
				Id = reader.GetInt32(reader.GetOrdinal("Id")),
				IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
			};
		}
		public static LoginResponseDB GenerateLoginResponse(SqlDataReader reader)
		{
			return new() { Id = reader.GetInt32(reader.GetOrdinal("Id")) };
		}
	}
}
