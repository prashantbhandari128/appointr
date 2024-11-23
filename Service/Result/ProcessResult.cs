namespace Appointr.Service.Result
{
    public class ProcessResult
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public ProcessResult() { }

        public ProcessResult(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public ProcessResult SetResult(bool status, string message)
        {
            Status = status;
            Message = message;
            return this;
        }
    }
}
