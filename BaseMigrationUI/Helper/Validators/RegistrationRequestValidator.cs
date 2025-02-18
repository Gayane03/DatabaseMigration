using BaseMigrationUI.Helper.Extensions;
using BaseMigrationUI.Models;
using FluentValidation;

namespace BaseMigrationUI.Helper.Validators
{
	public class RegistrationRequestValidator : AbstractValidator<RegistrationModel> 
	{
		private const string RepeatPasswordErrorMessage = "Does not match your real password";

		public RegistrationRequestValidator()
		{
			RuleFor(m => m.FirstName).IsRequired("FirstName");

			RuleFor(m => m.LastName).IsRequired("LastName");

			RuleFor(m => m.Email).IsRequired("Email");

			RuleFor(m => m.Password).IsRequired("Password");

			RuleFor(m => m.RepeatPassword).Equal(m => m.Password).WithMessage(RepeatPasswordErrorMessage);
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<RegistrationModel>.CreateWithOptions((RegistrationModel)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};

	}
}
