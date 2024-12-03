using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IUserRepository :
        IGetRepository<UserModel>,
        IGetWithRelationsRepository<UserModel>,
        IUpdateRepository<UserModel>
    {
        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="Username">The username of the user to retrieve.</param>
        /// <returns>The UserModel if found, otherwise null.</returns>
        UserModel? GetByUsername(string Username);
        /// <summary>
        /// Retrieves a user by their username asynchronously.
        /// </summary>
        /// <param name="Username">The username of the user to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the UserModel if found, otherwise null.</returns>
        Task<UserModel?> GetByUsernameAsync(string Username);

        /// <summary>
        /// Retrieves a list of users based on their usernames.
        /// </summary>
        /// <param name="usernames">A collection of usernames to filter users by.</param>
        /// <returns>A list of UserModel representing the users with the given usernames.</returns>
        List<UserModel>? GetUsersByUsernames(ICollection<string> usernames);
        /// <summary>
        /// Retrieves a list of users based on their usernames asynchronously.
        /// </summary>
        /// <param name="usernames">A collection of usernames to filter users by.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of UserModel for the given usernames.</returns>
        Task<List<UserModel>?> GetUsersByUsernamesAsync(ICollection<string> usernames);

        /// <summary>
        /// Retrieves a list of unpaid bills for a given user.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve unpaid bills.</param>
        /// <returns>A list of BillModel containing the unpaid bills for the user, or null if no unpaid bills are found.</returns>
        IEnumerable<BillModel>? GetUnpaidBillsForUser(string UserID);
        /// <summary>
        /// Retrieves a list of unpaid bills for a given user asynchronously.
        /// </summary>
        /// <param name="userId">The user ID for which to retrieve unpaid bills.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of BillModel with the unpaid bills, or null if no unpaid bills are found.</returns>
        Task<IEnumerable<BillModel>?> GetUnpaidBillsForUserAsync(string userId);
    }
}
