using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Application.Exceptions;
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
            List<PaymentModel> result = await _paymentRepository.GetPaymentsOfUserAsync(UserID.ToString()) ?? [];
            return result.Count > 0 ? result.Select(_mapper.Map<PaymentDTO>).ToList() : [];
        }

        /// <summary>
        /// Changes a payment's status to paid.
        /// </summary>
        /// <param name="paymentID">ID of the payment to be paid.</param>
        /// <returns>a <see cref="PaymentDTO"/> object showing the new state of the payment.</returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<PaymentDTO> PayAsync(int paymentID)
        {
            // Fetch the payment from Database.
            PaymentModel payment = await _paymentRepository.GetByIDAsync(paymentID) ?? throw new NotFoundException($"Payment With ID {paymentID} was not found.");

            // Change status and update the object.
            payment.IsPaid = true;
            int rowsAffected = _paymentRepository.Update(payment);

            if (rowsAffected == 0)
                throw new InvalidOperationException($"Payment {payment.Id} Was Not Updated.");

            // return an object based on the new state
            return _mapper.Map<PaymentDTO>(payment);
        }
    }
}