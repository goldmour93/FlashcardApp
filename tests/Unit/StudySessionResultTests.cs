using FlashcardApp.Core.Models;
using Xunit;

namespace FlashcardApp.Tests
{
    public class StudySessionResultTests
    {
        [Fact]
        public void NewInstance_DefaultsToZero()
        {
            var result = new StudySessionResult();

            Assert.Equal(0, result.CardsReviewed);
            Assert.Equal(0, result.XpGained);
        }

        [Fact]
        public void Properties_CanBeSetAndReadBack()
        {
            var result = new StudySessionResult
            {
                CardsReviewed = 7,
                XpGained = 25
            };

            Assert.Equal(7, result.CardsReviewed);
            Assert.Equal(25, result.XpGained);
        }
    }
}

