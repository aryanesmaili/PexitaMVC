using PexitaMVC.Application.DTOs;

namespace PexitaMVC.Application.Interfaces
{
    public interface IBillService
    {
        /// <summary>
        /// Add a new Bill to Database.
        /// </summary>
        /// <param name="billCreateDTO">Object containing info about the new record.</param>
        /// <returns>a <see cref="BillDTO"/> Object showing the new object that was made.</returns>
        Task<BillDTO> AddBillAsync(BillCreateDTO billCreateDTO);


        /// <summary>
        /// Deletes a bill from Database.
        /// </summary>
        /// <param name="BillID">ID of the bill to be deleted.</param>
        /// <returns></returns>
        Task DeleteBillAsync(int BillID);
    }
}
