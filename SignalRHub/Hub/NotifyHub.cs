using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
        public AppContext _context { get; }

        public NotifyHub(AppContext context)
        {
            _context = context;
        }


        public override async Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;

            var user = _context.User
                .Include(u => u.Connections)
                .SingleOrDefault(u => u.UserName == name);

            if (user == null)
            {
                user = new User
                {
                    UserName = name,
                    Connections = new List<Connection>()
                };
                _context.User.Add(user);
            }

            user.Connections.Add(new Connection
            {
                ConnectionID = Context.ConnectionId,
                UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"],
                Connected = true
            });
            _context.SaveChanges();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connection = _context.Connections.Find(Context.ConnectionId);
            connection.Connected = false;
            _context.SaveChanges();
            await base.OnDisconnectedAsync(exception);
        }        
    }
}
