using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub
{
    public class AppContext : IdentityDbContext<IdentityUser>
    {
        public AppContext(DbContextOptions<AppContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { set; get; }
        public DbSet<User> User { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
}
