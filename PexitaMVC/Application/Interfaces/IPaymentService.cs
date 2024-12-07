using PexitaMVC.Application.DTOs;

namespace PexitaMVC.Application.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Changes a payment's status to paid.
        /// </summary>
        /// <param name="paymentID">ID of the payment to be paid.</param>
        /// <returns>a <see cref="PaymentDTO"/> object showing the new state of the payment.</returns>
        Task<PaymentDTO> PayAsync(int paymentID);

        /// <summary>
        /// Gets the List of Payments for a user.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>List of Payments the user has.</returns>
        Task<List<PaymentDTO>> GetUserPayments(int UserID);
    }
}
