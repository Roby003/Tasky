using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Tasky.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Description is mandatory")]
        [StringLength(100, ErrorMessage = "The maximum length permitted is 100 characters")]
        public string? Descriere { get; set; }
        public string? Status { get; set; }
        [Required(ErrorMessage ="The task must have a start date")]
        public DateTime? DataStart { get; set; }
        [Required(ErrorMessage = "The task must have an end date")]

        public DateTime? DataFinalizare { get; set; }
        public string? Media { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project? Project { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; } 



    }
}
