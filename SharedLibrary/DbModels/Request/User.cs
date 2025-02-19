using SharedLibrary.Enum;

namespace SharedLibrary.DbModels.Request
{
	public class User
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int CountryId { get; set; } = (int)Country.Armenia;
		//public DateTime? DateOfBirth { get; set; } = DateTime.UtcNow;
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public bool IsActive { get; set; } = false;
		public int RoleId { get; set; } = 1;

	}
}
