using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options): base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { set; get; }
    }
}
