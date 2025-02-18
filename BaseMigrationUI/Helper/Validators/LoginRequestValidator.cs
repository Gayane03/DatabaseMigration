using BaseMigrationUI.Helper.Extensions;
using FluentValidation;
using SharedLibrary.RequestModels;

namespace BaseMigrationUI.Helper.Validators
{
	public class LoginRequestValidator:AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(m => m.Email).IsRequired("Email ");
			RuleFor(m => m.Password).IsRequired("Password ");
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<LoginRequest>.CreateWithOptions((LoginRequest)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
