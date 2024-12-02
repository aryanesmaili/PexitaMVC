using PexitaMVC.Core.Entites;

namespace PexitaMVC.Application.Interfaces
{
    public interface IUserService
    {
        bool IsUserInDebt(int UserID);
        Task<bool> IsUserInDebtAsync(int UserID);

        IEnumerable<BillModel> GetUnpaidBillsForUser(int UserID);
        Task<IEnumerable<BillModel>> GetUnpaidBillsForUserAsync(int UserID);
    }
}
