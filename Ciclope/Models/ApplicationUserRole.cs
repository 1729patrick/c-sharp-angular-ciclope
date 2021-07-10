using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models {
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual CiclopeUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
