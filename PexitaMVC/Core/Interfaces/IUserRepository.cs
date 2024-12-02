using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Repositories;

namespace PexitaMVC.Core.Interfaces
{
    public interface IUserRepository :
        IGetRepository<UserModel>,
        IGetWithRelationsRepository<UserModel>,
        IUpdateRepository<UserModel>
    {
        UserModel GetByUsername(string Username);
        Task<UserModel> GetByUsernameAsync(string Username);

        List<UserModel> GetUsersByUsernames(ICollection<string> usernames);
        Task<List<UserModel>> GetUsersByUsernamesAsync(ICollection<string> usernames);
    }
}
