using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class SystemRole : IdentityRole<int>
    {
        public ICollection<SystemUser> SystemUsers { get; set; }

        public SystemRole()
        {
            SystemUsers = new HashSet<SystemUser>();
        }
    }
}
