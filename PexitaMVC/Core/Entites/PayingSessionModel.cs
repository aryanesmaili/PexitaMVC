namespace PexitaMVC.Core.Entites
{
    public class PayingSessionModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public double TotalAmount { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<UserModel> Users { get; set; } = [];
        public ICollection<PaymentModel> Payments { get; set; } = [];
    }

    public class PaymentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }

        public int SessionID { get; set; }
        public required PayingSessionModel Session { get; set; }
    }
}
