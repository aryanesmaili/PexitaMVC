using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IBillRepository :
        IGetRepository<BillModel>,
        IAddRepository<BillModel>,
        IUpdateRepository<BillModel>,
        IDeleteRepository<BillModel>,
        IGetWithRelationsRepository<BillModel>
    {
        /// <summary>
        /// Retrieves all bills for a given payer ID.
        /// </summary>
        /// <param name="PayerID">The ID of the payer to retrieve bills for.</param>
        /// <returns>An IEnumerable of BillModel objects representing the payer's bills.</returns>
        IEnumerable<BillModel>? GetPayersBills(int PayerID);

        /// <summary>
        /// Retrieves all bills for a given payer ID asynchronously.
        /// </summary>
        /// <param name="PayerID">The ID of the payer to retrieve bills for.</param>
        /// <returns>A Task representing the asynchronous operation, containing an IEnumerable of BillModel objects representing the payer's bills.</returns>
        Task<IEnumerable<BillModel>?> GetPayersBillsAsync(int PayerID);
    }
}
