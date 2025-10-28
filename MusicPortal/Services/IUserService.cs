using MusicPortal.Models;

namespace MusicPortal.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> AuthenticateAsync(string username, string password);
        Task RegisterAsync(User user, string password);
        Task ActivateAsync(int id);
        Task DeleteAsync(int id);
    }
}
