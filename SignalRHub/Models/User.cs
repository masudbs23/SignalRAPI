using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public ICollection<Connection> Connections { get; set; }
    }
}
