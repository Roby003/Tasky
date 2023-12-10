using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasky.Data;
using Tasky.Models;

namespace Tasky.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TasksController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        [HttpPost]
        public IActionResult UpdateStatus(int taskId, string newStatus)
        {
            var task = db.Tasks.Find(taskId);

            // Assuming TaskStatus is the property in your Task model that represents the status
            task.Status = newStatus;

            db.SaveChanges();

            // Redirect to the project details or wherever you want to go
            return Json(new { success = true, message = "Task status updated successfully" });
        }
    }
}
