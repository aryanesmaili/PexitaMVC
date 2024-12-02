namespace PexitaMVC.Core.Entites
{
    public class BillModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        private double _totalAmount;

        public double TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (BillPayments.Sum(x => x.Amount) > value)
                    throw new ArgumentException("Payment amounts cannot exceed total amount.");
                _totalAmount = Math.Round(value, 3);
            }
        }

        public bool IsCompleted => BillPayments.All(x => x.IsPaid);

        public required string UserID { get; set; }
        public required UserModel User { get; set; }
        public ICollection<PaymentModel> BillPayments { get; set; } = [];
    }

    public class PaymentModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = false;

        public required string UserId { get; set; }
        public required UserModel User { get; set; }

        public int BillID { get; set; }
        public required BillModel Bill { get; set; }
    }
}
