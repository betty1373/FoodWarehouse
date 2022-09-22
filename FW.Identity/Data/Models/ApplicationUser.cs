using Microsoft.AspNetCore.Identity;
using System;

namespace FW.Identity.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string ForTestIgnore { get; set; }
        public string LastName { get; set; }
    }
}
