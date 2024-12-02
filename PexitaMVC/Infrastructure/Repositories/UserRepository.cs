using Microsoft.EntityFrameworkCore;
using PexitaMVC.Application.Exceptions;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;
using System.Linq.Expressions;

namespace PexitaMVC.Infrastructure.Repositories
{
    public class UserRepository(AppDBContext dbContext) : IUserRepository
    {
        private readonly AppDBContext _context = dbContext;

        public UserModel GetByID(int id)
        {
            var user = _context.Users
                .FromSqlInterpolated($"EXEC pr_GetUserByID @VarID = {id}")
                .FirstOrDefault() ?? throw new NotFoundException($"user with id {id} not found.");

            return user;
        }

        public async Task<UserModel> GetByIDAsync(int id)
        {
            var user = await _context.Users
                .FromSqlInterpolated($"EXEC pr_GetUserByID @VarID = {id}")
                .FirstOrDefaultAsync() ?? throw new NotFoundException($"user with id {id} not found.");

            return user;
        }

        public UserModel GetByUsername(string Username)
        {
            var user = _context.Users
                .FromSqlInterpolated($"EXEC pr_GetUserByUsername @VarUsername = {Username}")
                .FirstOrDefault() ?? throw new NotFoundException($"user with Username {Username} not found.");

            return user;
        }

        public async Task<UserModel> GetByUsernameAsync(string Username)
        {
            var user = await _context.Users
                .FromSqlInterpolated($"EXEC pr_GetUserByUsername @VarUsername = {Username}")
                .FirstOrDefaultAsync() ?? throw new NotFoundException($"user with Username {Username} not found.");

            return user;
        }

        public List<UserModel> GetUsersByUsernames(ICollection<string> usernames)
        {
            string input = string.Join(",", usernames.Select(username => $"'{username}'"));

            var users = _context.Users
                .FromSqlInterpolated($"EXEC pr_GetUsersByUsername @Usernames = {input}")
                .ToList();

            return users;
        }

        public async Task<List<UserModel>> GetUsersByUsernamesAsync(ICollection<string> usernames)
        {
            string input = string.Join(",", usernames.Select(username => $"'{username}'"));

            var users = await _context.Users
                .FromSqlInterpolated($"EXEC pr_GetUsersByUsername @Usernames = {input}")
                .ToListAsync();

            return users;
        }

        public UserModel GetWithRelations(int userID, params Expression<Func<UserModel, object>>[] includeExpressions)
        {
            IQueryable<UserModel> query = _context.Set<UserModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            // Execute the query to fetch the data
            return query.Where(x => x.Id == userID.ToString()).FirstOrDefault() ?? throw new NotFoundException($"User with ID {userID} not found.");
        }

        public async Task<UserModel> GetWithRelationsAsync(int userID, Expression<Func<UserModel, object>>[] includeExpressions)
        {
            // Start with IQueryable<UserModel>
            IQueryable<UserModel> query = _context.Set<UserModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            // Execute the query to fetch the data
            return await query.Where(x => x.Id == userID.ToString()).FirstOrDefaultAsync() ?? throw new NotFoundException($"User with ID {userID} not found.");
        }

        public void Update(UserModel entity)
        {
            _ = _context.Database.ExecuteSqlInterpolated(
                $"EXEC pr_UpdateUser @UserID = {entity.Id}, @Username = {entity.UserName}, @Name = {entity.Name}, @Email = {entity.Email}, @PhoneNumber = {entity.PhoneNumber}");
        }

        public async Task UpdateAsync(UserModel entity)
        {
            _ = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC pr_UpdateUser @UserID = {entity.Id}, @Username = {entity.UserName}, @Name = {entity.Name}, @Email = {entity.Email}, @PhoneNumber = {entity.PhoneNumber}");
        }
    }
}
