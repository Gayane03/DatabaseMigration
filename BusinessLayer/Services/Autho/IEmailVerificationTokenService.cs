namespace BusinessLayer.Autho
{
	public interface IEmailVerificationTokenService
	{
		public string GenerateVerificationToken(int userId, out string verificationCode);
		public (string,string) GetVerificationDataFromToken(string token);
	}
}
