
using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services
{
    /// <summary>
    /// Service responsible for managing study sessions and selecting cards to review.
    /// </summary>
    public static class StudySessionService
    {
        /// <summary>
        /// Retrieves a subset of due flashcards for a study session, capped by the daily limit.
        /// </summary>
        /// <param name="dueCards">The collection of all currently due flashcards.</param>
        /// <param name="dailyLimit">The maximum number of cards to review in the session.</param>
        /// <returns>An enumerable of flashcards to review, up to the daily limit.</returns>
        /// <exception cref="ArgumentNullException">Thrown when dueCards is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when dailyLimit is less than 0.</exception>
        public static IEnumerable<Flashcard> GetCardsForSession(IEnumerable<Flashcard> dueCards, int dailyLimit)
        {
            if (dueCards == null)
            {
                throw new ArgumentNullException(nameof(dueCards));
            }

            if (dailyLimit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dailyLimit), "Daily limit cannot be negative.");
            }

            // Cap the returned list at the daily limit
            return dueCards.Take(dailyLimit);
        }
    }
}