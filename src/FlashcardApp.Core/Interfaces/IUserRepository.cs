using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for user data persistence.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Saves a user to the underlying data store.
        /// </summary>
        Task SaveUserAsync(User user);
    }
}