using PexitaMVC.Application.DTOs;

namespace PexitaMVC.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Determines whether a user is in debt based on their unpaid payments.
        /// </summary>
        /// <param name="UserID">The ID of the user to check for debt status.</param>
        /// <returns>True if the user has any unpaid payments, otherwise false.</returns>
        bool IsUserInDebt(string UserID);

        /// <summary>
        /// Determines whether a user is in debt based on their unpaid payments asynchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user to check for debt status.</param>
        /// <returns>A Task that represents the asynchronous operation, containing a boolean indicating whether the user is in debt.</returns>
        Task<bool> IsUserInDebtAsync(string UserID);

        /// <summary>
        /// Retrieves all unpaid bills for a user synchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user for whom to fetch unpaid bills.</param>
        /// <returns>An IEnumerable of BillDTO objects representing the unpaid bills for the user.</returns>
        IEnumerable<BillDTO> GetUnpaidBillsForUser(string UserID);

        /// <summary>
        /// Retrieves all unpaid bills for a user asynchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user for whom to fetch unpaid bills.</param>
        /// <returns>A Task that represents the asynchronous operation, containing an IEnumerable of BillDTO objects representing the unpaid bills for the user.</returns>
        Task<IEnumerable<BillDTO>> GetUnpaidBillsForUserAsync(string UserID);

        /// <summary>
        /// Retrieves all bills for a user synchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user for whom to fetch bills.</param>
        /// <returns>An IEnumerable of BillDTO objects representing the bills for the user.</returns>
        IEnumerable<BillDTO> GetAllBillsForUser(string UserID);

        /// <summary>
        /// Retrieves all bills for a user asynchronously.
        /// </summary>
        /// <param name="UserID">The ID of the user for whom to fetch bills.</param>
        /// <returns>A Task that represents the asynchronous operation, containing an IEnumerable of BillDTO objects representing the bills for the user.</returns>
        Task<IEnumerable<BillDTO>> GetAllBillsForUserAsync(string UserID);
    }
}
