using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Backend
{
    public class MessageHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            return base.OnConnectedAsync();
        }
    }
}