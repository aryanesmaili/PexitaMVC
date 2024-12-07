using Microsoft.EntityFrameworkCore;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;

namespace PexitaMVC.Infrastructure.Repositories
{
    public class PaymentRepository(AppDBContext context) : IPaymentRepository
    {
        private readonly AppDBContext _context = context;

        /// <summary>
        /// Retrieves a payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the payment to retrieve.</param>
        /// <returns>The PaymentModel if found, otherwise null.</returns>
        public PaymentModel? GetByID(int id)
        {
            // Executes a stored procedure to fetch the payment by ID
            PaymentModel? result = _context.Payments
                .FromSqlInterpolated($"SELECT * FROM Payments WHERE Id = {id}")
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Retrieves a payment by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the PaymentModel if found, otherwise null.</returns>
        public async Task<PaymentModel?> GetByIDAsync(int id)   
        {
            // Executes a stored procedure asynchronously to fetch the payment by ID
            PaymentModel? result = await _context.Payments
                .FromSqlInterpolated($"SELECT * FROM Payments WHERE Id = {id}")
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Retrieves all payments associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose payments are to be retrieved.</param>
        /// <returns>A list of PaymentModel objects for the user, or null if no payments are found.</returns>
        public List<PaymentModel>? GetPaymentsOfUser(string userId)
        {
            // Executes a stored procedure to fetch payments associated with a user
            List<PaymentModel>? result = _context.Payments
                .FromSqlInterpolated($"SELECT * FROM Payments WHERE UserId = {userId}")
                .ToList();

            return result;
        }

        /// <summary>
        /// Retrieves all payments associated with a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose payments are to be retrieved.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of PaymentModel objects for the user, or null if no payments are found.</returns>
        public async Task<List<PaymentModel>?> GetPaymentsOfUserAsync(string userId)
        {
            // Executes a stored procedure asynchronously to fetch payments associated with a user
            List<PaymentModel>? result = await _context.Payments
                .FromSqlInterpolated($"SELECT * FROM Payments WHERE UserId = {userId}")
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Updates an existing payment in the database.
        /// </summary>
        /// <param name="entity">The payment entity to update.</param>
        /// <returns>The number of rows affected by the update operation.</returns>
        public int Update(PaymentModel entity)
        {
            // Executes a stored procedure to update a payment record
            int rowsAffected = _context.Database
                .ExecuteSqlInterpolated($"EXECUTE pr_UpdatePayment @PAYMENTID = {entity.Id}, @AMOUNT = {entity.Amount}, @ISPAID = {entity.IsPaid}");

            return rowsAffected;
        }

        /// <summary>
        /// Updates an existing payment in the database asynchronously.
        /// </summary>
        /// <param name="entity">The payment entity to update.</param>
        /// <returns>A Task representing the asynchronous operation, containing the number of rows affected by the update operation.</returns>
        public async Task<int> UpdateAsync(PaymentModel entity)
        {
            // Executes a stored procedure asynchronously to update a payment record
            int rowsAffected = await _context.Database
                .ExecuteSqlInterpolatedAsync($"EXECUTE pr_UpdatePayment @PAYMENTID = {entity.Id}, @AMOUNT = {entity.Amount}, @ISPAID = {entity.IsPaid}");

            return rowsAffected;
        }
    }
}
