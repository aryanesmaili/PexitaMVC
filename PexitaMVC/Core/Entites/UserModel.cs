using Microsoft.AspNetCore.Identity;

namespace PexitaMVC.Core.Entites
{
    public class UserModel : IdentityUser
    {
        public required string Name { get; set; }
        public ICollection<BillModel> Bills { get; set; } = [];
        public ICollection<PaymentModel> UserPayments { get; set; } = [];
    }
}
