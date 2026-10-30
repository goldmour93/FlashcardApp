using FlashcardApp.Core.Models;
using FlashcardApp.Core.Services;


namespace FlashcardApp.Tests
{
    public class StudySessionServiceTests
    {
        [Fact]
        public void GetCardsForSession_ValidLimit_ReturnsCappedList()
        {
            // Arrange
            var dueCards = new List<Flashcard>
            {
                new(), new(), new(), new(), new()
            };
            int dailyLimit = 3;

            // Act
            var result = StudySessionService.GetCardsForSession(dueCards, dailyLimit);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetCardsForSession_LimitGreaterThanDueCards_ReturnsAllDueCards()
        {
            // Arrange
            var dueCards = new List<Flashcard>
            {
                new(), new()
            };
            int dailyLimit = 5;

            // Act
            var result = StudySessionService.GetCardsForSession(dueCards, dailyLimit);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetCardsForSession_NullList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => StudySessionService.GetCardsForSession(null!, 5));
        }

        [Fact]
        public void GetCardsForSession_NegativeLimit_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var dueCards = new List<Flashcard>();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => StudySessionService.GetCardsForSession(dueCards, -1));
        }

        [Fact]
        public void GetCardsForSession_DailyLimitZero_ReturnsEmpty()
        {
            // Arrange
            var dueCards = new List<Flashcard>
            {
                new() { Id = Guid.NewGuid(), Front = "Q1", Back = "A1", Topic = "T" },
                new() { Id = Guid.NewGuid(), Front = "Q2", Back = "A2", Topic = "T" }
            };

            // Act
            var result = StudySessionService.GetCardsForSession(dueCards, dailyLimit: 0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetCardsForSession_DailyLimitGreaterThanDueCards_ReturnsAllCards()
        {
            // Arrange
            var dueCards = new List<Flashcard>
            {
                new() { Id = Guid.NewGuid(), Front = "Q1", Back = "A1", Topic = "T" },
                new() { Id = Guid.NewGuid(), Front = "Q2", Back = "A2", Topic = "T" },
                new() { Id = Guid.NewGuid(), Front = "Q3", Back = "A3", Topic = "T" }
            };

            // Act
            var result = StudySessionService.GetCardsForSession(dueCards, dailyLimit: dueCards.Count + 10).ToList();

            // Assert
            Assert.Equal(dueCards.Count, result.Count);
        }
    }
}