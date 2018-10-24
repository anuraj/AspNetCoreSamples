using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> UserMapping = new Dictionary<string, string>();
        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var name = httpContext.Request.Query["Name"];
            UserMapping.Add(Context.ConnectionId, name);
            Clients.All.SendAsync("broadcastMessage", "System", $"{name} joined the conversation");
            return base.OnConnectedAsync();
        }
        public void Send(string name, string message)
        {
            Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var name = UserMapping[Context.ConnectionId];
            Clients.All.SendAsync("broadcastMessage", "System", $"{name} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }
}