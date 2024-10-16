//2023 (c) TD Synnex - All Rights Reserved.


using FluentValidation;

namespace Common.Validation.Property
{
    /// <summary>
    /// Adds several standard data validation rules for FluentValidation
    /// </summary>
    public static class PropertyValidatorExtensions
    {
        /// <summary>Adds GUID validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new GuidPropertyValidator<T, string>());
        }

        /// <summary>Adds alphabetic validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeAlphabetic<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphaPropertyValidator<T, string>());
        }

        public static IRuleBuilderOptions<T, string> MustBeAlphabeticUpperCase<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ArgumentGuard.NotNull(ruleBuilder, nameof(ruleBuilder)).SetValidator(new AlphaUpperCasePropertyValidator<T, string>());
        }


        /// <summary>Adds alpha-numeric validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeAlphaNumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphaNumPropertyValidator<T, string>());
        }

        public static IRuleBuilderOptions<T, string> MustBeAlphaNumericSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphaNumericSpacePropertyValidator<T, string>());
        }

        public static IRuleBuilderOptions<T, string> MustBeAlphaNumericSpecial<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphaNumericSpecialPropertyValidator<T, string>());
        }

        public static IRuleBuilderOptions<T, string> MustBeAlphaSpecial<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphaSpecialPropertyValidator<T, string>());
        }

        /// <summary>Adds Base64-encoded string validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeBase64<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new Base64PropertyValidator<T, string>());
        }

        /// <summary>Adds numeric validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeNumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NumPropertyValidator<T, string>());
        }

        /// <summary>Adds name validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NamePropertyValidator<T, string>());
        }

        /// <summary>Adds separated words validation (e.g. 123-456.ABC_XYZ) to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeSeparatedWords<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SeparatedWordsPropertyValidator<T, string>());
        }

        /// <summary>Adds "safe text" validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeSafeText<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SafeTextPropertyValidator<T, string>());
        }

        /// <summary>Adds URL validation to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        public static IRuleBuilderOptions<T, string> MustBeUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UrlPropertyValidator<T, string>());
        }

        /// <summary>Adds "safe text" validation in JSON data string to the specified rule builder.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>Rule Builder Options</returns>
        /// <seealso cref="DataValidator.IsSafeText"/>
        public static IRuleBuilderOptions<T, string> MustBeSafeJson<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SafeJsonPropertyValidator<T, string>());
        }

    }
}
