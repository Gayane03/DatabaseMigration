using BaseMigrationUI.Helper.Extensions;
using FluentValidation;
using SharedLibrary.RequestModels;

namespace BaseMigrationUI.Helper.Validators
{
	public class VerifyEmailRequestValidator : AbstractValidator<EmailVerificationCodeRequest>
	{

		public VerifyEmailRequestValidator()
		{
			RuleFor(m => m.VerificationCode).IsRequired("Email verification code ");
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<EmailVerificationCodeRequest>.CreateWithOptions((EmailVerificationCodeRequest)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
