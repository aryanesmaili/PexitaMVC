using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Application.Exceptions;
using PexitaMVC.Application.Interfaces;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;

namespace PexitaMVC.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all unpaid bills for a user synchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user for whom to fetch unpaid bills.</param>
        /// <returns>An IEnumerable of BillDTO objects representing the unpaid bills for the user.</returns>
        public IEnumerable<BillDTO> GetUnpaidBillsForUser(string UserID)
        {
            // Fetch unpaid bills from the repository, returning an empty list if no bills are found
            List<BillModel> bills = (_userRepository.GetUnpaidBillsForUser(UserID) ?? []).ToList();

            // If there are any unpaid bills, map them to BillDTO, otherwise return an empty collection
            return bills.Count > 0 ? bills.Select(_mapper.Map<BillDTO>).ToList() : [];
        }

        /// <summary>
        /// Retrieves all unpaid bills for a user asynchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user for whom to fetch unpaid bills.</param>
        /// <returns>A Task that represents the asynchronous operation, containing an IEnumerable of BillDTO objects representing the unpaid bills for the user.</returns>
        public async Task<IEnumerable<BillDTO>> GetUnpaidBillsForUserAsync(string UserID)
        {
            // Fetch unpaid bills from the repository asynchronously, returning an empty list if no bills are found
            List<BillModel> bills = (await _userRepository.GetUnpaidBillsForUserAsync(UserID) ?? []).ToList();

            // If there are any unpaid bills, map them to BillDTO, otherwise return an empty collection
            return bills.Count > 0 ? bills.Select(_mapper.Map<BillDTO>).ToList() : [];
        }

        /// <summary>
        /// Determines whether a user is in debt based on their unpaid payments.
        /// </summary>
        /// <param name="UserID">The ID of the user to check for debt status.</param>
        /// <returns>True if the user has any unpaid payments, otherwise false.</returns>
        /// <exception cref="NotFoundException">Thrown if the user with the given ID is not found.</exception>
        public bool IsUserInDebt(string UserID)
        {
            // Fetch the user with related UserPayments from the repository by converting the UserID to integer
            UserModel user = _userRepository.GetWithRelations(int.Parse(UserID), u => u.UserPayments)
                ?? throw new NotFoundException($"User with ID {UserID} Not Found.");

            // Check if any of the user's payments are unpaid (IsPaid is false)
            return !user.UserPayments.All(x => x.IsPaid);
        }

        /// <summary>
        /// Determines whether a user is in debt based on their unpaid payments asynchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user to check for debt status.</param>
        /// <returns>A Task that represents the asynchronous operation, containing a boolean indicating whether the user is in debt.</returns>
        /// <exception cref="NotFoundException">Thrown if the user with the given ID is not found.</exception>
        public async Task<bool> IsUserInDebtAsync(string UserID)
        {
            // Fetch the user with related UserPayments from the repository asynchronously
            UserModel user = await _userRepository.GetWithRelationsAsync(int.Parse(UserID), u => u.UserPayments)
                ?? throw new NotFoundException($"User with ID {UserID} Not Found.");

            // Check if any of the user's payments are unpaid (IsPaid is false)
            return !user.UserPayments.All(x => x.IsPaid);
        }
    }
}
