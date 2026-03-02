
using FlashcardApp.Core.Services;


namespace FlashcardApp.Tests
{
    public class SettingsValidatorTests
    {
        // Boundary Value Analysis (BVA)
        // Valid range: 0.70 to 0.99
        // Boundaries to test: 0.69 (Invalid), 0.70 (Valid), 0.99 (Valid), 1.00 (Invalid)

        [Theory]
        [InlineData(0.70)] // Lower Boundary (Valid)
        [InlineData(0.85)] // Nominal Value (Valid)
        [InlineData(0.99)] // Upper Boundary (Valid)
        public void ValidateDesiredRetention_ValidBoundaries_DoesNotThrow(double retention)
        {
            // Act & Assert
            var exception = Record.Exception(() => SettingsValidator.ValidateDesiredRetention(retention));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(0.69)] // Just below lower boundary (Invalid)
        [InlineData(1.00)] // Just above upper boundary (Invalid)
        [InlineData(-0.50)] // Extreme invalid
        public void ValidateDesiredRetention_InvalidBoundaries_ThrowsArgumentOutOfRangeException(double retention)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => SettingsValidator.ValidateDesiredRetention(retention));
        }
    }
}