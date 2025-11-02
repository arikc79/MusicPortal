using Microsoft.AspNetCore.Identity;
using MusicPortal.Data;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var users = await _repository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.UserName == username);

            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task RegisterAsync(User user, string password)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            user.IsActive = false; //  має бути активований вручну
            await _repository.AddAsync(user);
        }

        public async Task ActivateAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _repository.UpdateAsync(user);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user != null)
            {
                await _repository.DeleteAsync(user);
            }
        }
    }
}
