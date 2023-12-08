using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Tasky.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string? Descriere { get; set; }
        public bool Status { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataFinalizare { get; set; }
        public string? Media { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project? Project { get; set; }




    }
}
