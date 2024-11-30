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

        public ICollection<UserModel> Users { get; set; } = [];
        public ICollection<PaymentModel> BillPayments { get; set; } = [];

        /// <summary>
        /// Adds a new User to a Bill
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(UserModel user)
        {
            if (!Users.Contains(user))
                Users.Add(user);
        }

        /// <summary>
        /// Adds a new payment to a bill
        /// </summary>
        /// <param name="payment"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddPayment(PaymentModel payment)
        {
            if (BillPayments.Sum(x => x.Amount) + payment.Amount > TotalAmount)
                throw new InvalidOperationException("Payment exceeds total amount.");
            BillPayments.Add(payment);
        }
    }

    public class PaymentModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }

        public required string UserId { get; set; }
        public required UserModel User { get; set; }

        public int BillID { get; set; }
        public required BillModel Bill { get; set; }
    }
}
