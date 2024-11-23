namespace Appointr.Helper.Data.Toastr
{
    public class Toastr
    {
        public Toastr(string title, string message, MessageType messageType)
        {
            Title = title;
            Message = message;
            MessageType = messageType;
        }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public MessageType MessageType { get; set; }
    }
}
