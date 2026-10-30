
using FSRS.Core.Models;

namespace FlashcardApp.Core.Models
{
    /// <summary>
    /// Represents a flashcard with its content and spaced repetition scheduling data.
    /// </summary>
    public class Flashcard
    {
        /// <summary>
        /// Gets or sets the unique identifier for the flashcard.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the front content of the flashcard (the question or prompt).
        /// </summary>
        public string Front { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the back content of the flashcard (the answer).
        /// </summary>
        public string Back { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the topic of the flashcard.
        /// </summary>
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the FSRS scheduling data for this flashcard.
        /// </summary>
        public Card FsrsCard { get; set; } = new Card();
    }
}