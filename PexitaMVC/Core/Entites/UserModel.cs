namespace PexitaMVC.Core.Entites
{
    public class UserModel
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; } = string.Empty;
        public ICollection<PayingSessionModel> PayingSessions { get; set; } = [];
    }
}
