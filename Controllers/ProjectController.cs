using ProjectManager.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjectManager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = new ProjectModel
                {
                    Name = model.Name,
                    Description = model.Description
                };

                db.Projects.Add(project);
                db.SaveChanges();

                return Json(new { success = true, projectId = project.Id, projectName = project.Name });
            }

            return Json(new { success = false });
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = await db.Projects.Include(p => p.Fields).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddField(int projectId, string fieldName, string fieldDescription)
        {
            try
            {
                var project = await db.Projects.FindAsync(projectId);
                if (project == null)
                {
                    return Json(new { success = false, message = "Project not found." });
                }

                var field = new FieldModel
                {
                    ProjectId = projectId,
                    Name = fieldName,
                    Description = fieldDescription
                };

                db.Fields.Add(field);
                await db.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = "Error occurred while adding the field." });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditField(int fieldId, string fieldName, string fieldDescription)
        {
            var field = await db.Fields.FindAsync(fieldId);
            if (field == null)
            {
                return HttpNotFound();
            }

            field.Name = fieldName;
            field.Description = fieldDescription;

            db.Entry(field).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(new { success = true });
        }


        // POST: Project/RemoveField
        [HttpPost]
        [Route("Project/RemoveField")]
        public async Task<ActionResult> RemoveField(int fieldId)
        {
            try
            {
                var field = await db.Fields.FindAsync(fieldId);
                if (field == null)
                {
                    return Json(new { success = false, message = "Field not found." });
                }

                db.Fields.Remove(field);
                await db.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = "Error occurred while removing the field." });

            }
        }
    }
}
