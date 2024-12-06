namespace PexitaMVC.Core.Entites
{
    public class BillModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        private double _totalAmount;

        public required double TotalAmount
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

        public required string OwnerID { get; set; }
        public UserModel? Owner { get; set; }
        public ICollection<PaymentModel> BillPayments { get; set; } = [];
    }

    public class PaymentModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = false;

        public required string UserId { get; set; }
        public UserModel? User { get; set; }

        public int BillID { get; set; }
        public BillModel? Bill { get; set; }
    }

    public class BillPaymentUserDBResult
    {
        public int BillID { get; set; }
        public string? Title { get; set; }
        public double TotalAmount { get; set; }
        public string? UserID { get; set; }

        public int PaymentID { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public string? PaymentUserID { get; set; }

        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? UserUserName { get; set; }
        public string? UserPhoneNumber { get; set; }
    }

    public class BillPaymentDBResult
    {
        public int Id { get; set; } // Bill ID
        public string Title { get; set; } = string.Empty; // Bill Title
        public double TotalAmount { get; set; } // Total Amount for the Bill
        public string BillUserID { get; set; } = string.Empty; // Creator of the Bill

        public int PaymentId { get; set; } // Payment ID
        public double PaymentAmount { get; set; } // Payment Amount
        public bool PaymentIsPaid { get; set; } // Is the payment paid?
        public string PaymentUserID { get; set; } = string.Empty; // Payment User
        public int PaymentBillID { get; set; } // Link to Bill
    }
}
