using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Tasky.Data;
using Tasky.Models;

namespace Tasky.Controllers
{
	public class ProjectsController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> _userManager;
		public ProjectsController( ApplicationDbContext db, UserManager<ApplicationUser> userManager)
		{
			this.db = db;
			_userManager = userManager;
		}

		[Authorize(Roles = "User,Admin")]
		public IActionResult New()
		{
			Project project = new Project();

			return View(project);
		}
		[Authorize(Roles = "User,Admin")]
		[HttpPost]
		public IActionResult New(Project project) {

			project.OrganizerId = _userManager.GetUserId(User);
			if (ModelState.IsValid)
			{
				db.Projects.Add(project);
				db.SaveChanges();
				return Redirect("/Projects/Show/" + project.Id);
			}
			else return View(project);
		}

		public IActionResult Show(int id)
		{
			Project project=db.Projects.Include("Users").Include("Categories").Where(m=>m.Id== id).First();
			return View(project);
		}
		[HttpPost]
		public IActionResult Show([FromForm] Category category)
		{

			Project project = db.Projects.Find(category.ProjectId);
			if (ModelState.IsValid)
			{
				if (project.OrganizerId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
				{
					db.Categories.Add(category);
					db.SaveChanges();

					return View(project);
				}
				else
				{
					TempData["message"] = "you can't create a category if you are not an organizer";
					TempData["messageClass"]="alert-danger";

					return View(project);
				}
			}
			else
			{ return View(project); }
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
