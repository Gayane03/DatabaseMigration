namespace SharedLibrary.DbModels.Response
{
	public class LoginResponseDB
	{
        public int Id { get; set; }
		public int RoleId { get; set; }	
		public string PasswordHash { get; set; }
		public string Email { get; set; }	
    }
}
