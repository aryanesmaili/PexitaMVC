namespace PexitaMVC.Application.DTOs
{
    public class BaseBillDTO
    {
        public required string Title { get; set; }
        public double TotalAmount { get; set; }
    }

    public class BillCreateDTO : BaseBillDTO
    {
        public int OwnerID { get; set; }
        public required Dictionary<string, double> Usernames { get; set; }
    }

    public class BillDTO : BaseBillDTO
    {
        public int ID { get; set; }
        public bool IsCompleted { get => Payments.All(x => x.IsPaid);}
        public required SubUserDTO Owner { get; set; }
        public required List<SubPaymentDTO> Payments { get; set; }
    }

    public class SubBillDTO : BaseBillDTO
    {
        public bool IsCompleted { get; set; }
        public int ID { get; set; }
    }

    public class UpdateBillDTO : BaseBillDTO { }

}
