using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasky.Models;

namespace Tasky.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
/*troll*/
		}
		public DbSet<Project>? Projects { get; set; }
		public DbSet<Category>? Categories { get; set; }
	}
}