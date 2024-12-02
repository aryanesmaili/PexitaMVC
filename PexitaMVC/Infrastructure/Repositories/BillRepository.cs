using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PexitaMVC.Application.Exceptions;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;
using System.Linq.Expressions;
using System.Text.Json;

namespace PexitaMVC.Infrastructure.Repositories
{
    public class BillRepository(AppDBContext context) : IBillRepository
    {
        private readonly AppDBContext _context = context;

        public BillModel Add(BillModel entity)
        {
            string payments = JsonSerializer.Serialize(entity.BillPayments.ToList());
            var bill = _context.Bills.FromSqlInterpolated(
                $"EXECUTE pr_InsertBill @Title ={entity.Title}, @TotalAmount = {entity.TotalAmount}, @UserID = {entity.UserID}, @PaymentsJson = {payments};"
                ).First();

            // EF doesn’t automatically populate navigation properties when executing raw SQL queries. 
            _context.Entry(bill).Reference(b => b.User).Load(); // Load the User
            _context.Entry(bill).Collection(b => b.BillPayments).Load(); // Load the Payments
            return bill;
        }

        public async Task<BillModel> AddAsync(BillModel entity)
        {
            string payments = JsonSerializer.Serialize(entity.BillPayments.ToList());

            var bill = await _context.Bills.FromSqlInterpolated(
                $"EXECUTE pr_InsertBill @Title ={entity.Title}, @TotalAmount = {entity.TotalAmount}, @UserID = {entity.UserID}, @PaymentsJson = {payments};"
                ).FirstAsync();

            // EF doesn’t automatically populate navigation properties when executing raw SQL queries. 
            await _context.Entry(bill).Reference(b => b.User).LoadAsync(); // Load the User
            await _context.Entry(bill).Collection(b => b.BillPayments).LoadAsync(); // Load the Payments
            return bill;
        }

        public void Delete(BillModel Entity)
        {
            int rowsAffected = _context.Database.ExecuteSqlInterpolated($"EXECUTE pr_DeleteBill @BILLID = {Entity.Id};");
            if (rowsAffected == 0)
                throw new NotFoundException($"Entity {Entity.Id} : {Entity.Title} Not Found.");
        }

        public async Task DeleteAsync(BillModel Entity)
        {
            int rowsAffected = await _context.Database.
                ExecuteSqlInterpolatedAsync($"EXECUTE pr_DeleteBill @BILLID = {Entity.Id};");

            if (rowsAffected == 0)
                throw new NotFoundException($"Entity {Entity.Id} : {Entity.Title} Not Found.");
        }

        public BillModel GetByID(int id)
        {
            var result = _context.Bills.FromSqlInterpolated($"EXECUTE pr_GetBillByID @BILLID={id};").FirstOrDefault();

            return result ?? throw new NotFoundException($"Bill With ID {id} Not Found.");
        }

        public async Task<BillModel> GetByIDAsync(int id)
        {
            var result = await _context.Bills.FromSqlInterpolated($"EXECUTE pr_GetBillByID @BILLID={id};").FirstOrDefaultAsync();

            return result ?? throw new NotFoundException($"Bill With ID {id} Not Found.");
        }

        public IEnumerable<BillModel> GetPayersBills(int PayerID)
        {
            var result = _context.Bills.FromSqlInterpolated($"EXECUTE pr_pr_GetPayersBills @PAYERID = {PayerID};").ToList();

            return result;
        }

        public async Task<IEnumerable<BillModel>> GetPayersBillsAsync(int PayerID)
        {
            var result = await _context.Bills.FromSqlInterpolated($"EXECUTE pr_pr_GetPayersBills @PAYERID = {PayerID};").ToListAsync();

            return result;
        }

        public BillModel GetWithRelations(int billID, params Expression<Func<BillModel, object>>[] includeExpressions)
        {
            IQueryable<BillModel> query = _context.Set<BillModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            return query.Where(x => x.Id == billID).FirstOrDefault() ?? throw new NotFoundException($"Bill with ID {billID} not found."); ;

        }

        public async Task<BillModel> GetWithRelationsAsync(int billID, params Expression<Func<BillModel, object>>[] includeExpressions)
        {
            IQueryable<BillModel> query = _context.Set<BillModel>();

            // Apply all the include expressions to the query
            foreach (var expression in includeExpressions)
            {
                query = query.Include(expression);
            }

            return await query.Where(x => x.Id == billID).FirstOrDefaultAsync() ?? throw new NotFoundException($"Bill with ID {billID} not found."); ;

        }

        public void Update(BillModel entity)
        {
            int rows = _context.Database.ExecuteSqlInterpolated($"EXECUTE pr_UpdateBill @BILLID = {entity.Id}, @Title = {entity.Title}, @TotalAmount = {entity.TotalAmount};");
            if (rows == 0)
                throw new NotFoundException($"Bill {entity.Id} : {entity.Title} Not Found");
        }

        public async Task UpdateAsync(BillModel entity)
        {
            int rows = await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE pr_UpdateBill @BILLID = {entity.Id}, @Title = {entity.Title}, @TotalAmount = {entity.TotalAmount};");

            if (rows == 0)
                throw new NotFoundException($"Bill {entity.Id} : {entity.Title} Not Found");
        }
    }
}
