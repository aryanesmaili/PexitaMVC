using Microsoft.AspNetCore.Identity;

namespace PexitaMVC.Core.Entites
{
    public class UserModel : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<BillModel> Bills { get; set; } = [];
        public ICollection<PaymentModel> UserPayments { get; set; } = [];
    }
}
