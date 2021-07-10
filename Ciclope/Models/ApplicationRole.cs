using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models {
    public class ApplicationRole : IdentityRole
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; }
}
}
