using Microsoft.AspNetCore.Identity;

namespace Tasky.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Task>? Tasks { get; set; }
        public virtual ICollection<ApplicationUserProject>? Projects { get; set; }


    }
}
