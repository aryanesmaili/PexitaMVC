namespace PexitaMVC.Application.DTOs
{
    public class BaseUserDTO
    {
        public string ID { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UserDTO : BaseUserDTO
    {
        public List<SubBillDTO>? Bills { get; set; }
        public List<SubPaymentDTO>? Payments { get; set; }
    }

    public class SubUserDTO : BaseUserDTO { }

    public class UpdateUserDTO : BaseUserDTO { }
}
