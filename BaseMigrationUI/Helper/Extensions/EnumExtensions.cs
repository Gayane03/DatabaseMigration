using System.ComponentModel;
using System.Reflection;

namespace BaseMigrationUI.Helper.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDescription<T>(this T value)
		{
			FieldInfo? field = value.GetType().GetField(value.ToString());
			DescriptionAttribute? attribute = field?.GetCustomAttribute<DescriptionAttribute>();
			return attribute?.Description ?? value.ToString();
		}
		
	}
}
