using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models {
    public class CiclopeUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(9)]
        public int Nif { get; set; }

        [PersonalData]
        [MaxLength(9)]
        public int Telemovel { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
