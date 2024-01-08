using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
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
				//aici e bugul
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.Message = TempData["message"];
                    ViewBag.Alert = TempData["messageClass"];
                }
				/*if (TempData.ContainsKey("modalName"))
					ViewBag.modalName = TempData["modalName"];*/

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
						t.UserId = task.UserId;
						t.Descriere = task.Descriere;
						t.DataFinalizare = task.DataFinalizare;
						t.DataStart = task.DataStart;
						t.Media = task.Media;
					}
					else
					{
						db.Tasks.Add(task);
					}
					db.SaveChanges();
					TempData["message"] = "task updated successfully";
					TempData["messageClass"] = "alert-success";
                    task.Users = GetAllUsers(task.ProjectId);
                    return PartialView("TaskModa", task);
                }
                else
				{
					TempData["message"] = "you can't handle a task if you are not an organizer";
					TempData["messageClass"] = "alert-danger";

                    return Redirect("/Projects/Show/" + project.Id);
                }
			}
			else
			{
				task.Users = GetAllUsers(task.ProjectId);
                return PartialView("TaskModa", task);
			}
		}
		public IActionResult AddTask(int? projectId=null,int? taskId=null)
		{
			//same method for edit and create
			var model = new Models.Task();
			if (taskId == null)
			{

				model.ProjectId = projectId;
			}
			else

			{ model = db.Tasks.Find(taskId); }

			model.Users = GetAllUsers(model.ProjectId);


            return PartialView("TaskModa", model);
		}
		[HttpPost]
		public IActionResult AddComment([FromForm] Comment comment)
		{
            var task = db.Tasks.Include("Comments").Where(m => m.Id == comment.TaskId).First();
            Project project = db.Projects.Find(task.ProjectId);
			
			comment.Date= DateTime.Now;
			comment.UserId= _userManager.GetUserId(User);
			if (ModelState.IsValid)
			{
				Comment c=db.Comments.Find(comment.Id);
				if (c != null)
				{
					c.Content = comment.Content;
                    TempData["message"] = "comment modified successfully";

                }
                else
				{
					db.Comments.Add(comment);
                    TempData["message"] = "comment created successfully";

                }
                db.SaveChanges();
                TempData["messageClass"] = "alert-success";
                return PartialView("TaskShowModal",task);
			
			}
			
			else
			{
                
                return PartialView("TaskShowModal" , task);
            }
        }
		
		public IActionResult ShowTask(int TaskId)
		{
			var task = db.Tasks.Include("Comments").Where(m=> m.Id==TaskId).First();
			return PartialView("TaskShowModal", task);
		}
		[HttpPost]
		public IActionResult DeleteComment(int id)
		{
			Comment comm = db.Comments.Find(id);
            Models.Task task = db.Tasks.Find(comm.TaskId);
            Project project = db.Projects.Find(task.ProjectId);
            if (comm == null)
				return Redirect("/Projects/Index");
			if( comm.UserId==_userManager.GetUserId(User) || User.IsInRole("Admin"))
			{ 
				
				db.Remove(comm);
				db.SaveChanges();
				TempData["message"] = "Comment removed";
				TempData["messageClass"] = "alert-success";
                return Redirect("/Projects/Show/" + project.Id);
            }
            else
			{
                TempData["message"] = "Comment removed";
                TempData["messageClass"] = "alert-success";
                return Redirect("/Projects/Show/" + project.Id);
            }
        }
		public IActionResult Index()
		{
			ApplicationUser user = db.ApplicationUsers.Include("Projects.Project").Where(m => m.Id.Equals(_userManager.GetUserId(User))).First();

            return View(user);
		}
        public IActionResult AddMember(int project_id)
        {
            ViewBag.project = project_id;
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]

        public IActionResult AddMember(int project_id,string email)
		{
			ApplicationUser user = db.ApplicationUsers.Where(m => m.Email.Equals(email)).FirstOrDefault();
			//check if the user with the certain email exists
			if (user != null) {

				//check if member is in prokect
                ApplicationUserProject projectUser = db.ApplicationUserProjects.Where(m=>m.ProjectId == project_id && m.UserId==user.Id).FirstOrDefault();
				if (projectUser != null)
				{
                    TempData["message"] = "Member already in project";
                    TempData["messageClass"] = "alert-danger";
                }
				else
				{
                    projectUser = new ApplicationUserProject();
                    projectUser.ProjectId = project_id;
                    projectUser.UserId = user.Id;
					//check if the project user model is valid
                    if (ModelState.IsValid)
                    {
                        db.ApplicationUserProjects.Add(projectUser);
                        db.SaveChanges();
                        TempData["message"] = "Member Added successfully";
                        TempData["messageClass"] = "alert-success";
                    }

                    else
                    {
                        TempData["message"] = "Error";
                        TempData["messageClass"] = "alert-danger";
                    }
                }
               
            }
            else
            {
                TempData["message"] = "Invalid Email";
                TempData["messageClass"] = "alert-danger";
               
            }
            return Redirect("/Projects/Show/" + project_id);
        }

		[NonAction]
		public IEnumerable<SelectListItem> GetAllUsers(int? projectid)
		{
			// generam o lista de tipul SelectListItem fara elemente
			var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date .Include("ApplicationUsers")
            var users = from user in db.ApplicationUserProjects.Where(m => m.ProjectId.Equals(projectid))
                        select user;

			// iteram prin categorii
			foreach (var user in users)
			{
                ApplicationUser us = db.ApplicationUsers.Where(m => m.Id.Equals(user.UserId)).FirstOrDefault();
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
				{
					Value = user.UserId.ToString(),
					
					Text = us.UserName.ToString()
				});
			}

			return selectList;
		}
	}
}
