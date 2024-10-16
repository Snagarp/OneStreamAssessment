using Common.Validation.Property;

namespace VendorConfiguration.Application.Validations
{
    /// <summary>
    /// Provides extension methods for adding standard data validation rules for use with FluentValidation.
    /// </summary>
    public static class PropertyValidatorExtensions
    {
        /// <summary>
        /// Adds a validation rule to ensure that the string contains only uppercase alphabetic characters.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder to which the validation rule is added.</param>
        /// <returns>
        /// An <see cref="IRuleBuilderOptions{T,T}"/> that can be used to further customize the validation rule.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if the <paramref name="ruleBuilder"/> is <c>null</c>.
        /// </exception>
        public static IRuleBuilderOptions<T, string> MustBeAlphabeticUpperCase<T>(
            this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder switch
            {
                // Ensure ruleBuilder is not null to avoid CS8625 warning.
                null => throw new ArgumentNullException(nameof(ruleBuilder), "The rule builder cannot be null."),
                _ => ruleBuilder.SetValidator(new AlphaUpperCasePropertyValidator<T, string>()),
            };
    }
}