using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repostiories;

namespace YetAnotherTodoApp.Infrastructure.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly YetAnotherTodoAppDbContext _dbContext;

        public UserRepository(YetAnotherTodoAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
            => await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}