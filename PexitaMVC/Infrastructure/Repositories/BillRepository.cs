using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;

namespace PexitaMVC.Infrastructure.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly AppDBContext _context;

        public BillRepository(AppDBContext context)
        {
            _context = context;
        }

        public void Add(BillModel entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(BillModel entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BillModel bill)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(BillModel bill)
        {
            throw new NotImplementedException();
        }

        public BillModel GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BillModel> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BillModel> GetPayersBills(int PayerID)
        {
            throw new NotImplementedException();
        }

        public void Update(BillModel entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BillModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
