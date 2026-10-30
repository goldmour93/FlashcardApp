namespace FlashcardApp.Core.Models
{
    /// <summary>
    /// A Data Transfer Object (DTO) representing the statistics of a completed study session.
    /// </summary>
    public class StudySessionResult
    {
        /// <summary>
        /// Gets or sets the total number of cards reviewed during the session.
        /// </summary>
        public int CardsReviewed { get; set; }

        /// <summary>
        /// Gets or sets the total experience points (XP) gained during the session.
        /// </summary>
        public int XpGained { get; set; }
    }
}