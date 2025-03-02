using Microsoft.Extensions.Configuration;
using RepositoryLayer.Helper;
using SharedLibrary.DbModels.Request;
using SharedLibrary.DbModels.Response;

namespace RepositoryLayer
{
	public class RegistrationRepository : CoreBaseRepository, IRegistrationRepository
	{
		public RegistrationRepository(IConfiguration configuration) : base(configuration) { }

		public async Task<int> GenerateUser(User user)
		{
			var parameters = new Dictionary<string, object>();

			parameters.Add($"@{nameof(User.FirstName)}", user.FirstName);
			parameters.Add($"@{nameof(User.LastName)}", user.LastName);
			parameters.Add($"@{nameof(User.CountryId)}", user.CountryId);
			parameters.Add($"@{nameof(User.Email)}", user.Email);
			parameters.Add($"@{nameof(User.PasswordHash)}", user.PasswordHash);
			
			//parameters.Add($"@{nameof(User.DateOfBirth)}", "2025-02-07 16:06:26.977");
			parameters.Add($"@{nameof(User.IsActive)}", 0);
			parameters.Add($"@{nameof(User.RoleId)}", 1);

			var userId = await Insert<User>(parameters);
			return userId;
		}

		public async Task<UserActivityDataResponse> GetUserActivityData(UserEmailRequest userEmailRequest)
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add($"email", userEmailRequest.Email);

			var whereConditionBody = $"{nameof(UserEmailRequest.Email)} = @email";
			return await Get<User, UserActivityDataResponse>(ResponseModelGenerator.GetUserActivityData, parameters, whereConditionBody);
		}

		public async Task ChangeUserToActive(int userId)
		{
			var updatingParameters = new Dictionary<string, object>();
			updatingParameters.Add($"IsActive", true);

			var whereConditionBody = "Id = @Id";
			var conditionParameters = new Dictionary<string, object>();
			conditionParameters.Add($"Id", userId);

			await Update<User>(updatingParameters, conditionParameters, whereConditionBody);
		}

		public async Task<LoginResponseDB> GetLoginProcessResponse(LoginRequestDB loginRequest)
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add($"email", loginRequest.Email);
		
			var whereConditionBody = $" {nameof(LoginRequestDB.Email)} = @email ";

			return await Get<User, LoginResponseDB>(
				ResponseModelGenerator.GenerateLoginResponse, 
				parameters, 
				whereConditionBody);
		}

		public async Task<bool> DeleteUserWithId(int userId)
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add($"@Id", userId);

			var whereConditionBody = " Id = @Id";

			return await Delete<User>(parameters, whereConditionBody);

		}
		public async Task<int> GetUserRoleId(int userId)
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add($"Id", userId);

			var whereConditionBody = $" Id = @Id";
			var roleResponse = await Get<User, RoleResponse>(ResponseModelGenerator.GenerateRoleResponse, parameters, whereConditionBody);
			return roleResponse!.RoleId;
		}
	}
}
