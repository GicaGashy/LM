using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        

        //relationship stuff

        //relationship with Team
        public int? TeamId { get; set; }
        public Team Team { get; set; }

    }
}
