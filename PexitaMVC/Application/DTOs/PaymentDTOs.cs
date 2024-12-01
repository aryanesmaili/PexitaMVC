﻿namespace PexitaMVC.Application.DTOs
{
    public class BasePaymentDTO
    {
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
    }

    public class PaymentDTO : BaseBillDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public required SubUserDTO User { get; set; }
        public int BillID { get; set; }
        public required SubBillDTO Bill { get; set; }
    }

    public class SubPaymentDTO : BaseBillDTO
    {
        public int ID { get; set; }
        public int BillID { get; set; }
    }
}