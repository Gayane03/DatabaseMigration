namespace SharedLibrary.ResponseModels
{
	public class EmailVerificationTokenResponse
    {
		public string VerificationToken { get; set; }  

		public EmailVerificationTokenResponse(string verificationToken)
		{
			VerificationToken = verificationToken;
		}
	}
}
