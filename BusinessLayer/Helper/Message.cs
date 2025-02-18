namespace BusinessLayer.Helper
{
	internal static class Message
	{
		public const string SystemError = "There is problem with system. Please try again later.";
		public const string VerificationCodeIncorrected = "Verification code is incorrected.";
		public const string LoginFailError = "There is not user with current login data.";
		public const string EmailActivityError = "There is active user with current email.";
		public const string TableGettingProblem = "There is not table in current database";
        public const string TableTransferProcessFail = "Current table transfer process is failed.";
    }
}
