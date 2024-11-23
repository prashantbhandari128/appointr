using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Appointr.Helper.Implementation
{
    public class ToastrHelper : IToastrHelper
    {
        private List<Toastr> toastrmessages = new List<Toastr>();

        public void SendMessage(Controller controller, string title, string message, MessageType messageType)
        {
            try
            {
                Toastr toastr = new Toastr(title, message, messageType);
                List<Toastr> toastrmessage = new List<Toastr>
                {
                    toastr
                };
                controller.TempData["messages"] = JsonConvert.SerializeObject(toastrmessage);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error Sending Message: {ex.Message}");
            }
        }

        public void AddMessage(string title, string message, MessageType messageType)
        {
            try
            {
                Toastr toastr = new Toastr(title, message, messageType);
                toastrmessages.Add(toastr);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error Adding Message: {ex.Message}");
            }
        }

        public void Send(Controller controller)
        {
            try
            {
                controller.TempData["messages"] = JsonConvert.SerializeObject(toastrmessages);
                toastrmessages.Clear();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error Sending Message: {ex.Message}");
            }
        }
    }
}
