using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOfficeDomain.Model
{
    public class User : IdentityUser
    {
        public string ContactNumber { get; set; }
    }

}
