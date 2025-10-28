using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task RegisterAsync(User user);
        Task ActivateAsync(int userId);
        Task<User> AuthenticateAsync(string username, string password);
        Task DeleteAsync(int id);
    }
}
