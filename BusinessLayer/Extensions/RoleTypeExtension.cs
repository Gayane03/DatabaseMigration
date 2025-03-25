using BusinessLayer.Helper;

namespace BusinessLayer.Extensions
{
	public static class RoleTypeExtension
	{
		public static string ToStringRole(this RoleType role)
		{
			return role switch { 
				RoleType.Admin => "admin", 
				RoleType.User => "user", 
				_ => throw new Exception("Role is not.") 
			};
		}
	}
}
