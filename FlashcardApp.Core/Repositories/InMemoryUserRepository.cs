using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Repositories
{
    /// <summary>
    /// A simple in-memory repository for storing users during runtime.
    /// This can be replaced with a JSON file repository for persistent storage.
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User> _users = [];

        public Task<User?> GetUserByIdAsync(Guid id)
        {
            _users.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = _users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task SaveUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _users[user.Id] = user;
            return Task.CompletedTask;
        }
    }
}