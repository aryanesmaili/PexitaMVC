using Microsoft.AspNetCore.Identity;

namespace PexitaMVC.Core.Entites
{
    public class UserModel : IdentityUser
    {
        public required string Name { get; set; }
        public ICollection<PayingSessionModel> PayingSessions { get; set; } = [];
    }
}
