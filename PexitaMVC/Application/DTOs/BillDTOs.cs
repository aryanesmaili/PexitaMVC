﻿namespace PexitaMVC.Application.DTOs
{
    public class BaseBillDTO
    {
        public required string Title { get; set; }
        public double TotalAmount { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class BillCreateDTO : BaseBillDTO
    {
        public required Dictionary<string, double> Usernames { get; set; }
    }

    public class BillDTO : BaseBillDTO
    {
        public int ID { get; set; }
        public required List<SubUserDTO> Users { get; set; }
        public required List<SubPaymentDTO> Payments { get; set; }
    }

    public class SubBillDTO : BaseBillDTO
    {
        public int ID { get; set; }
    }
}
