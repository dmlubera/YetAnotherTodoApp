using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}