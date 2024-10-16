namespace VendorConfiguration.Tests.Validators
{
    /// <summary>
    /// Contains unit tests for the <see cref="ModifyCountryCommandValidator"/> class to validate modification of a country.
    /// </summary>
    public class ModifyCountryCommandValidatorTests
    {
        /// <summary>
        /// The system under test (sut), an instance of <see cref="ModifyCountryCommandValidator"/>.
        /// </summary>
        private readonly ModifyCountryCommandValidator sut = new();

        /// <summary>
        /// Validates that a validation error occurs when the CountryId is missing.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorGivenMissingId()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "name",
                    Iso3166CountryCode2 = "xx",
                    Iso3166CountryCode3 = "xxx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.CountryId);
        }

        /// <summary>
        /// Validates that a validation error occurs when the CountryName is too short.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorGivenNameTooShort()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "x",
                    Iso3166CountryCode2 = "xx",
                    Iso3166CountryCode3 = "xxx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.RequestData.CountryName);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 2-character ISO country code is too short.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorGivenShortCountryCode2()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "name",
                    Iso3166CountryCode2 = "x",
                    Iso3166CountryCode3 = "xxx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.RequestData.Iso3166CountryCode2);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 2-character ISO country code is too long.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorGivenLongCountryCode2()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                CountryId = 1,
                RequestData = new CountryRequest
                {
                    CountryName = "name",
                    Iso3166CountryCode2 = "xxx",
                    Iso3166CountryCode3 = "xxx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.RequestData.Iso3166CountryCode2);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 3-character ISO country code is too short.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorGivenShortCountryCode3()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                CountryId = 1,
                RequestData = new CountryRequest
                {
                    CountryName = "name",
                    Iso3166CountryCode2 = "xx",
                    Iso3166CountryCode3 = "xx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.RequestData.Iso3166CountryCode3);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 3-character ISO country code is too long.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorGivenLongCountryCode3()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                CountryId = 1,
                RequestData = new CountryRequest
                {
                    CountryName = "name",
                    Iso3166CountryCode2 = "xx",
                    Iso3166CountryCode3 = "xxxx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.RequestData.Iso3166CountryCode3);
        }

        /// <summary>
        /// Validates that a validation error occurs when no fields are specified.
        /// </summary>
        [Fact]
        public void ShouldHaveValidationErrorWhenNoFieldSpecified()
        {
            // Arrange
            var model = new ModifyCountryCommand
            {
                CountryId = 1,
                RequestData = new CountryRequest()
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.RequestData);
        }
    }
}
