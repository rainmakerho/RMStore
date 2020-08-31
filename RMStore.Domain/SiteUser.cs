using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMStore.Domain
{
    public class SiteUsers
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
