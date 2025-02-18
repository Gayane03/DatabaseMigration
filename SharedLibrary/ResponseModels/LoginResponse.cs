
namespace SharedLibrary.ResponseModels
{
	public class LoginResponse
    {
		public string Token { get; set; }

		public LoginResponse(string token)
		{
			this.Token = token;
		}
	}
}
