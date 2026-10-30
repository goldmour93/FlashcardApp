
using FlashcardApp.Core.Services;


namespace FlashcardApp.Tests
{
    public class GamificationServiceTests
    {
        // Equivalence Partitioning (EP)
        // Valid Partition: Ratings 1, 2, 3, 4
        // Invalid Partitions: Ratings < 1, Ratings > 4

        [Theory]
        [InlineData(1, 0)]  // Again -> 0 XP
        [InlineData(2, 5)]  // Hard -> 5 XP
        [InlineData(3, 10)] // Good -> 10 XP
        [InlineData(4, 10)] // Easy -> 10 XP
        public void CalculateXp_ValidRatings_ReturnsExpectedXp(int rating, int expectedXp)
        {
            // Act
            int actualXp = GamificationService.CalculateXp(rating);

            // Assert
            Assert.Equal(expectedXp, actualXp);
        }

        [Theory]
        [InlineData(0)]  // Invalid Partition (Too low)
        [InlineData(-5)] // Invalid Partition (Negative)
        [InlineData(5)]  // Invalid Partition (Too high)
        [InlineData(10)] // Invalid Partition (Extreme high)
        public void CalculateXp_InvalidRatings_ThrowsArgumentOutOfRangeException(int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => GamificationService.CalculateXp(rating));
        }
    }
}