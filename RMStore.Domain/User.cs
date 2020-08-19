using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RMStore.Domain
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
       
        public string Email { get; set; }
         
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
 
    }
}
