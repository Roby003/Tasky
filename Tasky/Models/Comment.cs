using System.ComponentModel.DataAnnotations;

namespace Tasky.Models
{
	public class Comment
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Comment can't be empty")]
		[StringLength(100, ErrorMessage = "The maximum length permitted is 100 characters")]
		public string? Content { get; set; }
		public DateTime Date { get; set; }
		public string? UserId {  get; set; }
		public virtual ApplicationUser? User { get; set; }
		public int? TaskId { get; set; }
		public virtual Models.Task? Task { get; set; }
	}
}
