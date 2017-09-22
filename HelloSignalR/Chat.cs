using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace HelloSignalR
{
    public class Chat : Hub
    {
        public Task Send(string message)
        {
            if (message == string.Empty)
            {
                return Clients.All.InvokeAsync("Welcome");
            }

            return Clients.All.InvokeAsync("Send", message);
        }
    }
}