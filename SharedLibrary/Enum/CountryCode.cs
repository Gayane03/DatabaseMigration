using System.ComponentModel;

namespace SharedLibrary.Enum
{


	public enum Country
	{
		Armenia = 1,
		Russian = 375
	}

	/// <summary>
	/// 
	/// </summary>
	internal enum CountryCode
	{
		[Description($"+(374)")]
		Armenia = 374,

		[Description($"+(791)")]
		Russion = 791
	}

	internal static class CountryCodeHelper
	{ 
		internal static string CreateCountryCodeText(CountryCode countryCode)
		{
			return $"+({(int)countryCode})";
		}	
	}

}
