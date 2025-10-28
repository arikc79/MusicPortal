using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicPortal.Data;
using MusicPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace MusicPortal.Services
{
    // Клас відповідає за логіку користувачів: реєстрація, авторизація, активація
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        // Отримати всіх користувачів
        public async Task<IEnumerable<User>> GetAllAsync() => await _userRepo.GetAllAsync();

        // Отримати користувача по ID
        public async Task<User> GetByIdAsync(int id) => await _userRepo.GetByIdAsync(id);

        // Реєстрація нового користувача (поки не активований)
        public async Task RegisterAsync(User user)
        {
            // Hashing реального plain password-а
            var hashed = _passwordHasher.HashPassword(user, user.PasswordHash);
            user.PasswordHash = hashed;
            user.IsActive = false;
            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();
        }


        // Активація користувача адміном
        public async Task ActivateAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                _userRepo.Update(user);
                await _userRepo.SaveAsync();
            }
        }

        // Авторизація користувача
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var users = await _userRepo.GetAllAsync();
            var user = users.FirstOrDefault(u => u.UserName == username);
            if (user == null || !user.IsActive) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }


        // Видалення користувача
        public async Task DeleteAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                _userRepo.Delete(user);
                await _userRepo.SaveAsync();
            }
        }
    }
}
