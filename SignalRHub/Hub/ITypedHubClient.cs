using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub
{
    public interface ITypedHubClient    {
        Task BroadcastMessage(string payload);
        Task SendMessage(string sender, string message);
    }
}
