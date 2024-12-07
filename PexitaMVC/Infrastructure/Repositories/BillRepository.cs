using Microsoft.EntityFrameworkCore;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PexitaMVC.Infrastructure.Repositories
{
    public class BillRepository(AppDBContext context) : IBillRepository
    {
        private readonly AppDBContext _context = context;

        /// <summary>
        /// Adds a new bill to the database.
        /// </summary>
        /// <param name="entity">The bill to add.</param>
        /// <returns>The added BillModel with its generated ID and other details.</returns>
        public BillModel Add(BillModel entity)
        {
            // Serialize the bill payments to JSON for storage
            string payments = JsonSerializer.Serialize(
            entity.BillPayments.Select(bp => new Dictionary<string, object>
            {
                ["Amount"] = bp.Amount,
                ["IsPaid"] = bp.IsPaid,
                ["UserId"] = bp.UserId
            }));

            // Execute the stored procedure to insert the bill and return the result
            List<BillPaymentUserDBResult> result = _context.Database.
                SqlQuery<BillPaymentUserDBResult>(
                $"EXECUTE pr_InsertBill @Title = {entity.Title}, @TotalAmount = {entity.TotalAmount}, @UserID = {entity.OwnerID}, @PaymentsJson = {payments}"
                ).ToList();

            // Map the result to BillModel, assuming only one bill record is returned
            BillPaymentUserDBResult bill = result.First();
            BillModel billModel = new()
            {
                Id = bill.BillID,
                Title = bill.Title!,
                TotalAmount = bill.TotalAmount,
                OwnerID = bill.UserID!,
                Owner = new()
                {
                    Name = bill.UserName!,
                    UserName = bill.UserUserName,
                    Email = bill.UserEmail,
                    PhoneNumber = bill.UserPhoneNumber
                },
                BillPayments = result.Select(payment => new PaymentModel
                {
                    Id = payment.PaymentID,
                    Amount = payment.Amount,
                    IsPaid = payment.IsPaid,
                    UserId = payment.PaymentUserID!,
                    BillID = bill.BillID

                }).ToList()
            };
            return billModel;
        }

        /// <summary>
        /// Adds a new bill to the database asynchronously.
        /// </summary>
        /// <param name="entity">The bill to add.</param>
        /// <returns>A Task containing the added BillModel with its generated ID and other details.</returns>
        public async Task<BillModel> AddAsync(BillModel entity)
        {
            // Serialize the bill payments to JSON for storage
            string payments = JsonSerializer.Serialize(
            entity.BillPayments.Select(bp => new Dictionary<string, object>
            {
                ["Amount"] = bp.Amount,
                ["IsPaid"] = bp.IsPaid,
                ["UserId"] = bp.UserId
            }));

            // Execute the stored procedure to insert the bill asynchronously and return the result
            List<BillPaymentUserDBResult> result = await _context.Database
                .SqlQuery<BillPaymentUserDBResult>(
                $"EXECUTE pr_InsertBill @Title = {entity.Title}, @TotalAmount = {entity.TotalAmount}, @UserID = {entity.OwnerID}, @PaymentsJson = {payments}"
                ).ToListAsync();

            // Map the result to BillModel, assuming only one bill record is returned
            BillPaymentUserDBResult bill = result.First();
            BillModel billModel = new()
            {
                Id = bill.BillID,
                Title = bill.Title!,
                TotalAmount = bill.TotalAmount,
                OwnerID = bill.UserID!,
                Owner = new()
                {
                    Name = bill.UserName!,
                    UserName = bill.UserUserName,
                    Email = bill.UserEmail,
                    PhoneNumber = bill.UserPhoneNumber
                },
                BillPayments = result.Select(payment => new PaymentModel
                {
                    Id = payment.PaymentID,
                    Amount = payment.Amount,
                    IsPaid = payment.IsPaid,
                    UserId = payment.PaymentUserID!,
                    BillID = bill.BillID

                }).ToList()
            };

            return billModel;
        }

        /// <summary>
        /// Deletes a bill from the database.
        /// </summary>
        /// <param name="Entity">The bill to delete.</param>
        /// <returns>The number of rows affected by the delete operation.</returns>
        public int Delete(BillModel Entity)
        {
            // Execute the stored procedure to delete the bill and return the number of affected rows
            int rowsAffected = _context.Database
                .ExecuteSqlInterpolated($"EXECUTE pr_DeleteBill @BILLID = {Entity.Id}");

            return rowsAffected;
        }

        /// <summary>
        /// Deletes a bill from the database asynchronously.
        /// </summary>
        /// <param name="Entity">The bill to delete.</param>
        /// <returns>A Task representing the asynchronous operation, containing the number of rows affected by the delete operation.</returns>
        public async Task<int> DeleteAsync(BillModel Entity)
        {
            // Execute the stored procedure to delete the bill asynchronously and return the number of affected rows
            int rowsAffected = await _context.Database
                .ExecuteSqlInterpolatedAsync($"EXECUTE pr_DeleteBill @BILLID = {Entity.Id}");

            return rowsAffected;
        }

        /// <summary>
        /// Retrieves a bill by its ID.
        /// </summary>
        /// <param name="id">The ID of the bill to retrieve.</param>
        /// <returns>The BillModel if found, otherwise null.</returns>
        public BillModel? GetByID(int id)
        {
            // Execute the stored procedure to retrieve the bill by its ID
            BillModel? result = _context.Bills
                .FromSqlInterpolated($"SELECT * FROM Bills WHERE Id = {id}")
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Retrieves a bill by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the bill to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the BillModel if found, otherwise null.</returns>
        public async Task<BillModel?> GetByIDAsync(int id)
        {
            // Execute the stored procedure to retrieve the bill by its ID asynchronously
            BillModel? result = await _context.Bills
                .FromSqlInterpolated($"SELECT * FROM Bills WHERE Id = {id}")
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Retrieves all bills for a given payer ID.
        /// </summary>
        /// <param name="PayerID">The ID of the payer to retrieve bills for.</param>
        /// <returns>An IEnumerable of BillModel objects representing the payer's bills.</returns>
        public IEnumerable<BillModel>? GetPayersBills(string PayerID)
        {
            try
            {
                // Execute the stored procedure to retrieve bills for the given payer
                List<BillPaymentDBResult> results = _context.Database
                    .SqlQuery<BillPaymentDBResult>($"EXEC pr_GetAllBillsWithPayments @USERID = {PayerID}")
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
        /// Retrieves all bills for a given payer ID asynchronously.
        /// </summary>
        /// <param name="PayerID">The ID of the payer to retrieve bills for.</param>
        /// <returns>A Task representing the asynchronous operation, containing an IEnumerable of BillModel objects representing the payer's bills.</returns>
        public async Task<IEnumerable<BillModel>?> GetPayersBillsAsync(string PayerID)
        {
            try
            {
                // Execute the stored procedure to retrieve bills for the given payer asynchronously
                List<BillPaymentDBResult> results = await _context.Database
                        .SqlQuery<BillPaymentDBResult>($"EXEC pr_GetAllBillsWithPayments @USERID = {PayerID}")
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
        /// Retrieves a bill with related entities based on the provided include expressions.
        /// </summary>
        /// <param name="billID">The ID of the bill to retrieve.</param>
        /// <param name="includeExpressions">An array of expressions specifying related entities to include.</param>
        /// <returns>The BillModel with related entities, or null if not found.</returns>
        public BillModel? GetWithRelations(int billID, params Expression<Func<BillModel, object>>[] includeExpressions)
        {
            IQueryable<BillModel> query = _context.Set<BillModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            return query.Where(x => x.Id == billID).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a bill with related entities asynchronously based on the provided include expressions.
        /// </summary>
        /// <param name="billID">The ID of the bill to retrieve.</param>
        /// <param name="includeExpressions">An array of expressions specifying related entities to include.</param>
        /// <returns>A Task representing the asynchronous operation, containing the BillModel with related entities, or null if not found.</returns>
        public async Task<BillModel?> GetWithRelationsAsync(int billID, params Expression<Func<BillModel, object>>[] includeExpressions)
        {
            IQueryable<BillModel> query = _context.Set<BillModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            return await query.Where(x => x.Id == billID).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing bill in the database.
        /// </summary>
        /// <param name="entity">The bill to update.</param>
        /// <returns>The number of rows affected by the update operation.</returns>
        public int Update(BillModel entity)
        {
            // Execute the stored procedure to update the bill and return the number of affected rows
            int rowsAffected = _context.Database
                .ExecuteSqlInterpolated($"EXECUTE pr_UpdateBill @BILLID = {entity.Id}, @Title = {entity.Title}, @TotalAmount = {entity.TotalAmount}");

            return rowsAffected;
        }

        /// <summary>
        /// Updates an existing bill in the database asynchronously.
        /// </summary>
        /// <param name="entity">The bill to update.</param>
        /// <returns>A Task representing the asynchronous operation, containing the number of rows affected by the update operation.</returns>
        public async Task<int> UpdateAsync(BillModel entity)
        {
            // Execute the stored procedure to update the bill asynchronously and return the number of affected rows
            int rowsAffected = await _context.Database
                .ExecuteSqlInterpolatedAsync($"EXECUTE pr_UpdateBill @BILLID = {entity.Id}, @Title = {entity.Title}, @TotalAmount = {entity.TotalAmount}");

            return rowsAffected;
        }
    }
}
