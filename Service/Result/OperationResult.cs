namespace Appointr.Service.Result
{
    public class OperationResult<T> : ProcessResult
    {
        public int RowAffected { get; set; }
        public T? Data { get; set; }
        public OperationResult() { }
        public OperationResult(bool status, string message, int rowAffected, T? data)
        {
            Status = status;
            Message = message;
            RowAffected = rowAffected;
            Data = data;
        }
        public OperationResult<T> SetResult(bool status, string message, int rowAffected, T? data)
        {
            Status = status;
            Message = message;
            RowAffected = rowAffected;
            Data = data;
            return this;
        }
    }
}
