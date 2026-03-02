using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services
{
    /// <summary>
    /// Service responsible for managing user accounts and their data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </remarks>
    public class UserService(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Creates a new user or retrieves an existing one by username.
        /// </summary>
        public async Task<User> GetOrCreateUserAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));

            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                return existingUser;
            }

            var newUser = new User { Username = username };
            await _userRepository.SaveUserAsync(newUser);
            return newUser;
        }

        /// <summary>
        /// Adds XP to the user and saves the updated user state.
        /// </summary>
        public async Task AddXpToUserAsync(User user, string topic, int xpGained)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (xpGained < 0) throw new ArgumentOutOfRangeException(nameof(xpGained), "XP cannot be negative.");

            user.TotalXp += xpGained;

            if (!string.IsNullOrWhiteSpace(topic))
            {
                if (!user.TopicXp.ContainsKey(topic))
                {
                    user.TopicXp[topic] = 0;
                }
                user.TopicXp[topic] += xpGained;
            }

            await _userRepository.SaveUserAsync(user);
        }
    }
}