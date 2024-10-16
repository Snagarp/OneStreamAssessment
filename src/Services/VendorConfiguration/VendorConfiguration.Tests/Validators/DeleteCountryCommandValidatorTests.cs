namespace VendorConfiguration.Tests.Validators
{
    /// <summary>
    /// Contains unit tests for the <see cref="DeleteCountryCommandValidator"/> class to validate the DeleteCountryCommand.
    /// </summary>
    public class DeleteCountryCommandValidatorTests
    {
        /// <summary>
        /// The system under test (sut), an instance of <see cref="DeleteCountryCommandValidator"/>.
        /// </summary>
        private readonly IValidator<DeleteCountryCommand> sut = new DeleteCountryCommandValidator();

        /// <summary>
        /// Validates that a validation error occurs when the CountryId is zero.
        /// </summary>
        [Fact]
        public void ShouldHaveErrorWhenCountryIdIsZero()
        {
            // Arrange
            var command = new DeleteCountryCommand { CountryId = 0 };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CountryId);
        }
    }
}
