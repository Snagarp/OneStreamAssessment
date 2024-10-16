namespace VendorConfiguration.Tests.Validators
{
    /// <summary>
    /// Contains unit tests for the <see cref="CountryQueryValidator"/> class to validate the behavior of country queries.
    /// </summary>
    public class CountryQueryValidatorTests
    {
        /// <summary>
        /// The system under test (sut), an instance of <see cref="CountryQueryValidator"/>.
        /// </summary>
        private readonly CountryQueryValidator sut = new();

        /// <summary>
        /// Validates that a validation error occurs when the CountryId is set to zero.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorWhenCountryIdIsZero()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryId = 0
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.CountryId);
        }

        /// <summary>
        /// Validates that no validation error occurs when the CountryId is non-zero.
        /// </summary>
        [Fact]
        public void ShouldNotHaveValidationErrorWhenCountryIdIsNonZero()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryId = 1
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(m => m.CountryId);
        }

        /// <summary>
        /// Validates that no validation error occurs when the CountryId is null.
        /// </summary>
        [Fact]
        public void ShouldNotHaveValidationErrorWhenCountryIdIsNull()
        {
            // Arrange
            var model = new CountryQuery
            {
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(m => m.CountryId);
        }

        /// <summary>
        /// Validates that no validation error occurs when the CountryCode is null.
        /// </summary>
        [Fact]
        public void ShouldNotHaveValidationErrorWhenCountryCodeIsNull()
        {
            // Arrange
            var model = new CountryQuery
            {
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(m => m.CountryCode);
        }

        /// <summary>
        /// Validates that no validation error occurs when the CountryCode is exactly 2 characters.
        /// </summary>
        [Fact]
        public void ShouldNotHaveValidationErrorWhenCountryCodeIs2Char()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryCode = "XX"
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(m => m.CountryCode);
        }

        /// <summary>
        /// Validates that no validation error occurs when the CountryCode is exactly 3 characters.
        /// </summary>
        [Fact]
        public void ShouldNotHaveValidationErrorWhenCountryCodeIs3Char()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryCode = "XXX"
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(m => m.CountryCode);
        }

        /// <summary>
        /// Validates that a validation error occurs when the CountryCode is too short (less than 2 characters).
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorWhenCountryCodeIsShort()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryCode = "x"
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.CountryCode);
        }

        /// <summary>
        /// Validates that a validation error occurs when the CountryCode is too long (more than 3 characters).
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorWhenCountryCodeIsLong()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryCode = "xxxx"
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.CountryCode);
        }

        /// <summary>
        /// Validates that a validation error occurs when both CountryId and CountryCode are provided.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorWhenIdIsPresentWithCode()
        {
            // Arrange
            var model = new CountryQuery
            {
                CountryId = 1,
                CountryCode = "xxx"
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m);
        }
    }
}
