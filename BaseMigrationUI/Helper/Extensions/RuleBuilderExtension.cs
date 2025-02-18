using FluentValidation;

namespace BaseMigrationUI.Helper.Extensions
{
	public static class RuleBuilderExtension
	{
		public static IRuleBuilderOptions<TModel, TField> IsRequired<TModel, TField>(this IRuleBuilder<TModel, TField> ruleBuilder, string fieldName)
		{
			return ruleBuilder.NotNull()
				              .NotEmpty()
							  .WithMessage($"{fieldName} is required.");
		}
	}
}
