using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Application.Interfaces;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;

namespace PexitaMVC.Infrastructure.Services
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public BillService(IBillRepository billRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _billRepository = billRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Add a new Bill to Database.
        /// </summary>
        /// <param name="billCreateDTO">Object containing info about the new record.</param>
        /// <returns>a <see cref="BillDTO"/> Object showing the new object that was made.</returns>
        public async Task<BillDTO> AddBillAsync(BillCreateDTO billCreateDTO)
        {
            // first we create the new object based on the info provided in front-end.
            BillModel newRecord = _mapper.Map<BillModel>(billCreateDTO);

            // now we resolve the users present in this bill
            UserModel Owner = _userRepository.GetByID(billCreateDTO.OwnerID);
            newRecord.UserID = Owner.Id;
            newRecord.User = Owner;

            // then we resolve and create payment models for this bill.
            List<PaymentModel> payments = await CreatePayments(billCreateDTO.Usernames, newRecord);
            newRecord.BillPayments = payments;

            // we add the new record to database using the bill repository.
            await _billRepository.AddAsync(newRecord);

            // we create the result from this operation to be shown to user.
            return _mapper.Map<BillDTO>(newRecord);
        }

        /// <summary>
        /// Creates a list of payments to be filled later in application flow.
        /// </summary>
        /// <param name="Users">Users Participating in bill along with their pay amount.</param>
        /// <param name="bill">the bill this payment belongs to.</param>
        /// <returns></returns>
        private async Task<List<PaymentModel>> CreatePayments(Dictionary<string, double> Users, BillModel bill)
        {
            List<PaymentModel> payments = [];

            HashSet<UserModel> users = new(await _userRepository.GetUsersByUsernamesAsync(Users.Keys)); // Converted to HashSet to increase search speed and ensure uniqueness.

            foreach (var item in Users)
            {
                UserModel user = users.First(x => x.UserName == item.Key);
                payments.Add(new PaymentModel
                {
                    Bill = bill,
                    User = user,
                    UserId = user.Id,
                    Amount = item.Value,
                });
            }
            return payments;
        }

        /// <summary>
        /// Deletes a bill from Database.
        /// </summary>
        /// <param name="BillID">ID of the bill to be deleted.</param>
        /// <returns></returns>
        public async Task DeleteBillAsync(int BillID)
        {
            // we find the bill to be deleted.
            BillModel bill = await _billRepository.GetByIDAsync(BillID);

            // we delete it using the repository.
            await _billRepository.DeleteAsync(bill);
        }

    }
}
