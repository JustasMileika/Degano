using Degano_API.Data;
using Degano_API.Models.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Models.Specifications;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appContext;

        public UserRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<User> AddUserAsync(User User)
        {
            await _appContext.Users.AddAsync(User);

            return User;
        }

        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _appContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _appContext.Users.ToListAsync();
        }

        public async Task<bool> UserExistsAsync(Guid id)
        {
            return await _appContext.Users.AnyAsync(e => e.Id == id);
        }


        public User RemoveUser(User User)
        {
            _appContext.Users.Remove(User);
            return User;
        }

        public User UpdateUser(User User)
        {
            _appContext.Entry(User).State = EntityState.Modified;

            return User;
        }

        public async Task SaveChangesAsync()
        {
            await _appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(ISpecification<User> spec)
        {
            var res = spec.Includes
                .Aggregate(_appContext.Users.AsQueryable(),
                    (current, include) => current.Include(include));

            res = spec.IncludeStrings
                .Aggregate(res,
                    (current, include) => current.Include(include));


            if (spec.IsPagingEnabled)
            {
                res = res.Skip(spec.Skip)
                             .Take(spec.Take);
            }

            if (spec.Criteria != null)
            {
                res = res.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                res = res.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                res = res.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.GroupBy != null)
            {
                res = res.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            return await res.ToListAsync();
        }

        public async Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate)
        {
            return await _appContext.Users.FirstOrDefaultAsync(predicate);
        }
    }
}
