namespace PexitaMVC.Application.Exceptions
{
    public class DebugError
    {
        public required string Message { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
    }
}
