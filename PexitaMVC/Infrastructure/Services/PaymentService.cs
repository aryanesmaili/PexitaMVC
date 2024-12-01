using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Application.Interfaces;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;

namespace PexitaMVC.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IMapper mapper, IPaymentRepository paymentRepository)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Gets the List of Payments for a user.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>List of Payments the user has.</returns>
        public async Task<List<PaymentDTO>> GetUserPayments(int UserID)
        {
            List<PaymentModel> result = await _paymentRepository.GetPaymentsOfUserAsync(UserID);
            return result.Select(_mapper.Map<PaymentDTO>).ToList();
        }

        /// <summary>
        /// Changes a payment's status to paid.
        /// </summary>
        /// <param name="paymentID">ID of the payment to be paid.</param>
        /// <returns>a <see cref="PaymentDTO"/> object showing the new state of the payment.</returns>
        public async Task<PaymentDTO> Pay(int paymentID)
        {
            // Fetch the payment from Database.
            PaymentModel payment = await _paymentRepository.GetByIDAsync(paymentID);

            // Change status and update the object.
            payment.IsPaid = true;
            _paymentRepository.Update(payment);

            // return an object based on the new state
            return _mapper.Map<PaymentDTO>(payment);
        }
    }
}
