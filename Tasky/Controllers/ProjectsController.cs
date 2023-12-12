using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
			
			string id = _userManager.GetUserId(User);
            project.OrganizerId = id;
			
			if (ModelState.IsValid)
			{
				db.Projects.Add(project);
				
                db.SaveChanges();
                ApplicationUserProject projectUser = new ApplicationUserProject();
                projectUser.ProjectId = project.Id;
                projectUser.UserId = id;
                db.ApplicationUserProjects.Add(projectUser);
				db.SaveChanges();
                return Redirect("/Projects/Show/" + project.Id);
			}
			else return View(project);
		}

		public IActionResult Show(int id)
		{
            Project project = db.Projects.Include("ApplicationUsers").Include("Tasks.Comments.User").Where(m => m.Id == id).First();


			if (project.ApplicationUsers.Any(u => u.UserId == _userManager.GetUserId(User)))
			{
				return View(project);
			}

			return RedirectToAction("Index");

        }
		/*Show folosit pentru a adauga sau a edita taskuri*/
		[HttpPost]
		public IActionResult AddTask([FromForm] Models.Task task)
		{

			Project project = db.Projects.Find(task.ProjectId);
			if (ModelState.IsValid)
			{
				if (project.OrganizerId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
				{
                    Models.Task t = db.Tasks.Find(task.Id);
                    if (t != null)
					{
						t.Descriere = task.Descriere;
						t.DataFinalizare = task.DataFinalizare;
						t.DataStart= task.DataStart;
						t.Media = task.Media;
                    }
					else
					{
                        db.Tasks.Add(task);
                    }
                    db.SaveChanges();

                    return Redirect("/Projects/Show/" + project.Id);
                }
				else
				{
					TempData["message"] = "you can't create a category if you are not an organizer";
					TempData["messageClass"]="alert-danger";

					return View("Show",project);
				}
			}
			else
			{ return View("Show",project); }
		}
		[HttpPost]
		public IActionResult AddComment([FromForm] Comment comment)
		{
			Models.Task task=db.Tasks.Find(comment.TaskId);
			Project project = db.Projects.Find(task.ProjectId);
			
			comment.Date= DateTime.Now;
			comment.UserId= _userManager.GetUserId(User);
			if (ModelState.IsValid)
			{
				Comment c=db.Comments.Find(comment.Id);
				if (c != null)
				{
					c.Content = comment.Content;

				}
				else
				{
					db.Comments.Add(comment);
				}
				db.SaveChanges();
				return Redirect("/Projects/Show/" + project.Id);
			
			}
			else
			{ return View("Show", project); }
		}
		public IActionResult Index()
		{
			ApplicationUser user = db.ApplicationUsers.Include("Projects.Project").Where(m => m.Id.Equals(_userManager.GetUserId(User))).First();

            return View(user);
		}
	}
}
