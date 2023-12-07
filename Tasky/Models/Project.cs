using System.ComponentModel.DataAnnotations;

namespace Tasky.Models
{
	public class Project
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="The project must have a name")]
		[StringLength(100, ErrorMessage = "The maximum length permitted is 100 characters")]
		[MinLength(5, ErrorMessage = "The minimum length permitted is 5 characters")]
		public string? Name { get; set; }
		
		public string? OrganizerId { get; set; }
		public virtual ICollection<Category>? Categories { get; set; }
		public virtual ICollection<ApplicationUser> Users { get; set; }


	}
}
