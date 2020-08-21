using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TuristRegistar.Hubs
{
    public class ChatHub : Hub
    {
        public Task Send(string userid, string receiverid, string message)
        {
            return Clients.All.SendAsync("Send", userid, receiverid, message);
        }
    }
}