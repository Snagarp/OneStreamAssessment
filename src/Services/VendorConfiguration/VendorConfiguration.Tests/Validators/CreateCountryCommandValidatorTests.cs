namespace VendorConfiguration.Tests.Validators
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateCountryCommandValidator"/> class to validate the behavior of the country creation command.
    /// </summary>
    public class CreateCountryCommandValidatorTests
    {
        /// <summary>
        /// The system under test (sut), an instance of <see cref="CreateCountryCommandValidator"/>.
        /// </summary>
        private readonly CreateCountryCommandValidator sut = new();

        /// <summary>
        /// Validates that a validation error occurs when the CountryName is missing.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhenCountryNameIsMissing()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    Iso3166CountryCode2 = "XX",
                    Iso3166CountryCode3 = "XXX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.CountryName);
        }

        /// <summary>
        /// Validates that a validation error occurs when the CountryName is too short.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhenCountryNameIsShort()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "X",
                    Iso3166CountryCode2 = "XX",
                    Iso3166CountryCode3 = "XXX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.CountryName);
        }

        /// <summary>
        /// Validates that a validation error occurs when the CountryName is too long.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhenCountryNameIsLong()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                    Iso3166CountryCode2 = "XX",
                    Iso3166CountryCode3 = "XXX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.CountryName);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 2-character ISO country code is missing.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhen2CharIsMissing()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXX",
                    Iso3166CountryCode3 = "XXX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.Iso3166CountryCode2);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 2-character ISO country code is too short.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhen2CharIsShort()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXX",
                    Iso3166CountryCode2 = "X",
                    Iso3166CountryCode3 = "XXX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.Iso3166CountryCode2);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 2-character ISO country code is too long.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhen2CharIsLong()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXX",
                    Iso3166CountryCode2 = "XXX",
                    Iso3166CountryCode3 = "XXX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.Iso3166CountryCode2);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 3-character ISO country code is missing.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhen3CharIsMissing()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXX",
                    Iso3166CountryCode2 = "XX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.Iso3166CountryCode3);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 3-character ISO country code is too short.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhen3CharIsShort()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXX",
                    Iso3166CountryCode2 = "XX",
                    Iso3166CountryCode3 = "XX"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.Iso3166CountryCode3);
        }

        /// <summary>
        /// Validates that a validation error occurs when the 3-character ISO country code is too long.
        /// </summary>
        [Fact]
        public void ShouldBeValidationErrorWhen3CharIsLong()
        {
            // Arrange
            var model = new CreateCountryCommand
            {
                RequestData = new CountryRequest
                {
                    CountryName = "XXX",
                    Iso3166CountryCode2 = "XX",
                    Iso3166CountryCode3 = "xxxx"
                }
            };

            // Act
            var result = sut.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestData.Iso3166CountryCode3);
        }
    }
}
