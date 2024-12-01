using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IUserRepository :
        IGetRepository<UserModel>,
        IAddRepository<UserModel>,
        IUpdateRepository<UserModel>
    {
        bool IsUserInDebt(int UserID);
        Task<bool> IsUserInDebtAsync(int UserID);

        IEnumerable<BillModel> GetUnpaidBillsForUser(int UserID);
        Task<IEnumerable<BillModel>> GetUnpaidBillsForUserAsync(int UserID);

        UserModel GetByUsername(string Username);
        Task<UserModel> GetByUsernameAsync(string Username);

        List<UserModel> GetUsersByUsernames(ICollection<string> usernames);
        Task<List<UserModel>> GetUsersByUsernamesAsync(ICollection<string> usernames);
    }
}
