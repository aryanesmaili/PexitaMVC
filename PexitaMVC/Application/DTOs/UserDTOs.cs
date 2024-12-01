namespace PexitaMVC.Application.DTOs
{
    public class BaseUserDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UserDTO : BaseUserDTO
    {
        public List<SubBillDTO>? Bills { get; set; }
        public List<SubPaymentDTO>? Payments { get; set; }
    }

    public class SubUserDTO : BaseBillDTO { }

    public class UpdateUserDTO : BaseUserDTO { }
}
