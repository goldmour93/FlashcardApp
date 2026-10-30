using System;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.Services;
using FSRS.Core.Interfaces;
using FSRS.Core.Enums;

namespace FlashcardApp.Core.Facades
{
    /// <summary>
    /// A facade that wraps the FSRS algorithm and integrates it with the application's business logic.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FlashcardEngineFacade"/> class.
    /// </remarks>
    /// <param name="scheduler">The FSRS scheduler algorithm.</param>
    public class FlashcardEngineFacade
    {
        private readonly IScheduler _scheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardEngineFacade"/> class.
        /// </summary>
        /// <param name="scheduler">The FSRS scheduler algorithm.</param>
        public FlashcardEngineFacade(IScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        /// <summary>
        /// Reviews a flashcard, updates its scheduling data using FSRS, and calculates the XP gained.
        /// </summary>
        /// <param name="card">The flashcard being reviewed.</param>
        /// <param name="rating">The user's rating (1=Again, 2=Hard, 3=Good, 4=Easy).</param>
        /// <returns>The amount of XP gained from the review.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the card is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the rating is invalid.</exception>
        public int ReviewCard(Flashcard card, int rating)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            // Validate the rating and calculate XP gained
            int xpGained = GamificationService.CalculateXp(rating);

            // Map the integer rating to the FSRS Rating enum
            Rating fsrsRating = rating switch
            {
                1 => Rating.Again,
                2 => Rating.Hard,
                3 => Rating.Good,
                4 => Rating.Easy,
                _ => throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be strictly between 1 and 4.")
            };

            // Use the FSRS algorithm to calculate the next due date and update the card's scheduling data
            var (updatedCard, _) = _scheduler.ReviewCard(card.FsrsCard, fsrsRating);

            // Update the flashcard's FSRS data with the new scheduling information
            card.FsrsCard = updatedCard;

            return xpGained;
        }
    }
}