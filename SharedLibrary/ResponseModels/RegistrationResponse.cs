namespace SharedLibrary.ResponseModels
{
	public class RegistrationResponse
    {
		public string? Token { get; }

		public RegistrationResponse(string? token = null)
		{
			Token = token;
		}
	}
}
