using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repostiories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
    }
}