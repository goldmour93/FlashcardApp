
namespace FlashcardApp.Core.Services
{
    /// <summary>
    /// Service responsible for calculating gamification rewards such as Experience Points (XP).
    /// </summary>
    public static class GamificationService
    {
        /// <summary>
        /// Calculates the XP gained based on the user's rating of a flashcard.
        /// </summary>
        /// <param name="userRating">The user's rating (1=Again, 2=Hard, 3=Good, 4=Easy).</param>
        /// <returns>The amount of XP gained.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the rating is not between 1 and 4.</exception>
        public static int CalculateXp(int userRating)
        {
            return userRating switch
            {
                1 => 0,   // Again
                2 => 5,   // Hard
                3 => 10,  // Good
                4 => 10,  // Easy
                _ => throw new ArgumentOutOfRangeException(nameof(userRating), "Rating must be strictly between 1 and 4.")
            };
        }
    }
}