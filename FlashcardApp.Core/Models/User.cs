
namespace FlashcardApp.Core.Models
{
    /// <summary>
    /// Represents a user of the flashcard application.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the user's display name or username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total experience points (XP) the user has accumulated.
        /// </summary>
        public int TotalXp { get; set; } = 0;

        /// <summary>
        /// Gets or sets the user's preferred daily limit for reviewing cards.
        /// </summary>
        public int DailyLimit { get; set; } = 20;

        /// <summary>
        /// Gets or sets the user's desired retention rate for the FSRS algorithm.
        /// </summary>
        public double DesiredRetention { get; set; } = 0.90;

        /// <summary>
        /// Gets or sets the experience points (XP) the user has accumulated per topic.
        /// </summary>
        public Dictionary<string, int> TopicXp { get; set; } = [];

        /// <summary>
        /// Gets or sets the user's collection of flashcards.
        /// </summary>
        public List<Flashcard> Deck { get; set; } = [];
    }
}