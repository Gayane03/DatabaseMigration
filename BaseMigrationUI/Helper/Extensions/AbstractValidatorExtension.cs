using FluentValidation;

namespace BaseMigrationUI.Helper.Extensions
{
	public static class AbstractValidatorExtension
	{
		public static async Task<IEnumerable<string>> ValidateModelsAsync<T>(
		 this AbstractValidator<T> abstractValidator,
		 object model,
		 string propertyName)
		{
			if (model is not T typedModel)
				throw new ArgumentException($"The model is not of type {typeof(T)}");

			var result = await abstractValidator.ValidateAsync(
				ValidationContext<T>.CreateWithOptions(typedModel, x => x.IncludeProperties(propertyName))
			);

			return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
		}
	}
}
