using Microsoft.EntityFrameworkCore;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;
using System.Linq.Expressions;

namespace PexitaMVC.Infrastructure.Repositories
{
    public class UserRepository(AppDBContext dbContext) : IUserRepository
    {
        private readonly AppDBContext _context = dbContext;

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The UserModel if found, otherwise null.</returns>
        public UserModel? GetByID(string id)
        {
            // Execute the stored procedure to retrieve the user by their ID
            UserModel? user = _context.Users
                .FromSqlInterpolated($"SELECT * FROM dbo.AspNetUsers WHERE Id = {id}")
                .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Retrieves a user by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the UserModel if found, otherwise null.</returns>
        public async Task<UserModel?> GetByIDAsync(string id)
        {
            UserModel? user = await _context.Users
                .FromSqlInterpolated($"SELECT * FROM dbo.AspNetUsers WHERE Id = {id}")
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="Username">The username of the user to retrieve.</param>
        /// <returns>The UserModel if found, otherwise null.</returns>
        public UserModel? GetByUsername(string Username)
        {
            UserModel? user = _context.Users
                .FromSqlInterpolated($"SELECT * FROM dbo.AspNetUsers WHERE UserName = {Username}")
                .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Retrieves a user by their username asynchronously.
        /// </summary>
        /// <param name="Username">The username of the user to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the UserModel if found, otherwise null.</returns>
        public async Task<UserModel?> GetByUsernameAsync(string Username)
        {
            UserModel? user = await _context.Users
                .FromSqlInterpolated($"SELECT * FROM dbo.AspNetUsers WHERE UserName = {Username}")
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Retrieves a list of unpaid bills for a given user.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve unpaid bills.</param>
        /// <returns>A list of BillModel containing the unpaid bills for the user, or null if no unpaid bills are found.</returns>
        public IEnumerable<BillModel>? GetUnpaidBillsForUser(string userId)
        {
            try
            {
                // Execute the stored procedure to retrieve unpaid bills for the user
                List<BillPaymentDBResult> results = _context.Set<BillPaymentDBResult>()
                    .FromSqlInterpolated($"EXEC pr_GetUnpaidBillsWithPaymentsForUser @UserID = {userId};")
                    .ToList();

                // Group the results by BillId and map to BillModel with related PaymentModels
                return results.GroupBy(r => new { r.Id, r.Title, r.TotalAmount, r.BillUserID })
                    .Select(group => new BillModel
                    {
                        Id = group.Key.Id,
                        Title = group.Key.Title,
                        TotalAmount = group.Key.TotalAmount,
                        OwnerID = group.Key.BillUserID,
                        BillPayments = group.Select(payment => new PaymentModel
                        {
                            Id = payment.PaymentId,
                            Amount = payment.PaymentAmount,
                            IsPaid = payment.PaymentIsPaid,
                            UserId = payment.PaymentUserID,
                            BillID = payment.PaymentBillID
                        }).ToList()
                    }).ToList();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of unpaid bills for a given user asynchronously.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve unpaid bills.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of BillModel with the unpaid bills, or null if no unpaid bills are found.</returns>
        public async Task<IEnumerable<BillModel>?> GetUnpaidBillsForUserAsync(string userId)
        {
            try
            {
                // Execute the stored procedure to retrieve unpaid bills for the user asynchronously
                List<BillPaymentDBResult> results = await _context.Set<BillPaymentDBResult>()
                    .FromSqlInterpolated($"EXEC pr_GetUnpaidBillsWithPaymentsForUser @UserID = {userId};")
                    .ToListAsync();

                // Group the results by BillId and map to BillModel with related PaymentModels
                return results.GroupBy(r => new { r.Id, r.Title, r.TotalAmount, r.BillUserID })
                    .Select(group => new BillModel
                    {
                        Id = group.Key.Id,
                        Title = group.Key.Title,
                        TotalAmount = group.Key.TotalAmount,
                        OwnerID = group.Key.BillUserID,
                        BillPayments = group.Select(payment => new PaymentModel
                        {
                            Id = payment.PaymentId,
                            Amount = payment.PaymentAmount,
                            IsPaid = payment.PaymentIsPaid,
                            UserId = payment.PaymentUserID,
                            BillID = payment.PaymentBillID
                        }).ToList()
                    }).ToList();
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of users based on their usernames.
        /// </summary>
        /// <param name="usernames">A collection of usernames to filter users by.</param>
        /// <returns>A list of UserModel representing the users with the given usernames.</returns>
        public List<UserModel>? GetUsersByUsernames(ICollection<string> usernames)
        {
            // Convert the collection of usernames to a comma-separated string
            string usernamesStr = string.Join(",", usernames.Select(u => $"{u}"));

            // Use the string_split function to break the comma-separated string into individual values
            List<UserModel> users = _context.Users
                .FromSqlInterpolated($"SELECT * FROM dbo.AspNetUsers WHERE UserName IN (SELECT value FROM string_split({usernamesStr}, ','))")
                .ToList();

            return users;
        }

        /// <summary>
        /// Retrieves a list of users based on their usernames asynchronously.
        /// </summary>
        /// <param name="usernames">A collection of usernames to filter users by.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of UserModel for the given usernames.</returns>
        public async Task<List<UserModel>?> GetUsersByUsernamesAsync(ICollection<string> usernames)
        {
            // Convert the collection of usernames to a comma-separated string
            string usernamesStr = string.Join(",", usernames.Select(u => $"{u}"));

            // Use the string_split function to break the comma-separated string into individual values
            List<UserModel> users = await _context.Users
                .FromSqlInterpolated($"SELECT * FROM dbo.AspNetUsers WHERE UserName IN (SELECT value FROM string_split({usernamesStr}, ','))")
                .ToListAsync();

            return users;
        }


        /// <summary>
        /// Retrieves a user with related entities based on the provided include expressions.
        /// </summary>
        /// <param name="userID">The ID of the user to retrieve.</param>
        /// <param name="includeExpressions">An array of expressions specifying related entities to include in the result.</param>
        /// <returns>The UserModel with related entities, or null if not found.</returns>
        public UserModel? GetWithRelations(int userID, params Expression<Func<UserModel, object>>[] includeExpressions)
        {
            IQueryable<UserModel> query = _context.Set<UserModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            return query.Where(x => x.Id == userID.ToString()).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a user with related entities asynchronously based on the provided include expressions.
        /// </summary>
        /// <param name="userID">The ID of the user to retrieve.</param>
        /// <param name="includeExpressions">An array of expressions specifying related entities to include in the result.</param>
        /// <returns>A Task representing the asynchronous operation, containing the UserModel with related entities, or null if not found.</returns>
        public async Task<UserModel?> GetWithRelationsAsync(int userID, Expression<Func<UserModel, object>>[] includeExpressions)
        {
            IQueryable<UserModel> query = _context.Set<UserModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            return await query.Where(x => x.Id == userID.ToString()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="entity">The user entity to update.</param>
        /// <returns>The number of rows affected by the update operation.</returns>
        public int Update(UserModel entity)
        {
            int rowsAffected = _context.Database.ExecuteSqlInterpolated(
                $"EXEC pr_UpdateUser @UserID = {entity.Id}, @Username = {entity.UserName}, @Name = {entity.Name}, @Email = {entity.Email}, @PhoneNumber = {entity.PhoneNumber};");

            return rowsAffected;
        }

        /// <summary>
        /// Updates an existing user in the database asynchronously.
        /// </summary>
        /// <param name="entity">The user entity to update.</param>
        /// <returns>A Task representing the asynchronous operation, containing the number of rows affected by the update operation.</returns>
        public async Task<int> UpdateAsync(UserModel entity)
        {
            int rowsAffected = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC pr_UpdateUser @UserID = {entity.Id}, @Username = {entity.UserName}, @Name = {entity.Name}, @Email = {entity.Email}, @PhoneNumber = {entity.PhoneNumber};");

            return rowsAffected;
        }
    }
}
