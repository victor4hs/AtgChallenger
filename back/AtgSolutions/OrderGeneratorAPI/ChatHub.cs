using Microsoft.AspNetCore.SignalR;

namespace OrderGeneratorAPI
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message, string status)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, status);
        }
    }
}