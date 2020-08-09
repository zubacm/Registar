using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TuristRegistar.Hubs
{
    public class ChatHub : Hub
    {
        public Task Send(string userid, string message)
        {
            return Clients.All.SendAsync("Send", userid, message);
        }
    }
}