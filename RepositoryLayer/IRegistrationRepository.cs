using SharedLibrary.DbModels.Request;
using SharedLibrary.DbModels.Response;

namespace RepositoryLayer
{
	public interface IRegistrationRepository
	{
		public Task<UserActivityDataResponse> GetUserActivityData(UserEmailRequest userEmailRequest);
		public Task<int> GenerateUser(User userData);

		public Task ChangeUserToActive(int userId);

		public Task<LoginResponseDB> GetLoginProcessResponse(LoginRequestDB loginRequest);

		public Task<bool> DeleteUserWithId(int userId);
	}
}
