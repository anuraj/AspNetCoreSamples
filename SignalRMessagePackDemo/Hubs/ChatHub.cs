using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRMessagePackDemo.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }  
    }
}