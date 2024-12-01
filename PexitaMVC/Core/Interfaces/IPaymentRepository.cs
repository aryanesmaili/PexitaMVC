using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IPaymentRepository :
                     IGetRepository<PaymentModel>,
                     IUpdateRepository<PaymentModel>
    {
        List<PaymentModel> GetPaymentsOfUser(int userId);
        Task<List<PaymentModel>> GetPaymentsOfUserAsync(int userId);
    }
}
