using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public int Year { get; set; }
        public List<Collection> Collections { get; set; }

        public Like Like { get; set; }
        
        
    }
}
