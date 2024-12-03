using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IPaymentRepository :
                     IGetRepository<PaymentModel>,
                     IUpdateRepository<PaymentModel>
    {
        /// <summary>
        /// Retrieves all payments associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose payments are to be retrieved.</param>
        /// <returns>A list of PaymentModel objects for the user, or null if no payments are found.</returns>
        List<PaymentModel>? GetPaymentsOfUser(string userId);

        /// <summary>
        /// Retrieves all payments associated with a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose payments are to be retrieved.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of PaymentModel objects for the user, or null if no payments are found.</returns>
        Task<List<PaymentModel>?> GetPaymentsOfUserAsync(string userId);
    }
}
