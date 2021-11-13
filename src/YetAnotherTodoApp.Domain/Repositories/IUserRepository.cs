using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<bool> CheckIfEmailIsInUseAsync(string email);
        Task<bool> CheckIfUsernameIsInUseAsync(string username);
        Task<User> GetByIdAsync(Guid id);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
    }
}