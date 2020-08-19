using System;
using System.Collections.Generic;
using System.Text;

namespace RMStore.Domain
{
    public class JwtSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }

        public string Secret { get; set; }
    }
}
