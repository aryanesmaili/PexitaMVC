using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IBillRepository :
        IGetRepository<BillModel>,
        IAddRepository<BillModel>,
        IUpdateRepository<BillModel>,
        IDeleteRepository<BillModel>
    {
        IEnumerable<BillModel> GetPayersBills(int PayerID);
    }
}
