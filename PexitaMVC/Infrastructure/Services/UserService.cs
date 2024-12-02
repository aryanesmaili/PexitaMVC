using PexitaMVC.Application.Interfaces;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;

namespace PexitaMVC.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<BillModel> GetUnpaidBillsForUser(int UserID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BillModel>> GetUnpaidBillsForUserAsync(int UserID)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInDebt(int UserID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInDebtAsync(int UserID)
        {
            throw new NotImplementedException();
        }
    }
}
