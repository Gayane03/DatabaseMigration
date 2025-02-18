using SharedLibrary.RequestModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BaseMigrationUI
{
	public class ApiController 
	{
		private readonly HttpClient httpClient;
        public ApiController(HttpClient httpClient)
        {
		    this.httpClient = httpClient;	
        }

		public async Task<HttpResponseMessage?> RegisterUser(RegistrationRequest registrationRequest)
		{
			return await httpClient.PostAsJsonAsync("User/register", registrationRequest);
		}

		public async Task<HttpResponseMessage?> VerifyUserEmail(EmailVerificationCodeRequest emailVerificationCodeRequest,string? token)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			return await httpClient.PostAsJsonAsync("User/verifyEmail", emailVerificationCodeRequest);
		}

		public async Task<HttpResponseMessage> LoginUser(LoginRequest loginRequest)
		{
			return await httpClient.PostAsJsonAsync("User/login", loginRequest);
		}

	}
}
