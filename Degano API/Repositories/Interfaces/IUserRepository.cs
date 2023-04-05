using Degano_API.Models.Entities;
using Models.Specifications;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserAsync(Guid id);

        public Task<IEnumerable<User>> GetUsersAsync();
        
        public User RemoveUser(User User);

        public User UpdateUser(User User);

        public Task<User> AddUserAsync(User User);

        public Task<bool> UserExistsAsync(Guid id);

        public Task SaveChangesAsync();

        public Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate);

        public Task<IEnumerable<User>> GetUsersAsync(ISpecification<User> spec);
    }
}
