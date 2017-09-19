namespace Common.ValueObjects
{
    public class ErrorMessage
    {
        public int Error { get; set; }
        public int SubError { get; set; }
        public string MessageKey { get; set; }
        public string Message { get; set; }
        public ErrorMessage[] Details { get; set; }
    }
}
