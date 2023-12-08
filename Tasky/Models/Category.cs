using System.ComponentModel.DataAnnotations;

namespace Tasky.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="The name of the category is mandatory")]
		[StringLength(100, ErrorMessage = "The maxium length permitted is 100 characters")]
		[MinLength(5, ErrorMessage = "The minimum length permitted is 5 characters")]
		public string Name { get; set; }
		public int? ProjectId {get; set; }
		public virtual Project? Project { get; set; }
		public virtual ICollection<Task>? Tasks { get; set; }
		
	}
}
