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

        //a controller that handles that rountes to the Project index view
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // a controller that handles routes o the create project view
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

        // a controller that handles routes to the details view
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


        // the endpoint to add filed in the details.cshtml view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddField(int projectId, string fieldName, string fieldDescription)
        {
            var project = await db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return HttpNotFound();
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



        // POST: Project/EditField
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


        // the API endpoint that retrieves a field's description by project name and field name
        [HttpGet]
        public async Task<ActionResult> GetFieldDescription(string projectName, string fieldName)
        {
            var project = await db.Projects.Include(p => p.Fields)
                                           .FirstOrDefaultAsync(p => p.Name == projectName);

            if (project == null)
            {
                return HttpNotFound();
            }


            var field = project.Fields.FirstOrDefault(f => f.Name == fieldName);
            if (field == null)
            {
                return HttpNotFound();
            }

            return Json(field.Description, JsonRequestBehavior.AllowGet);
        }

        // the endpoint to get all fields' names and descriptions for a specific project
        [HttpGet]
        public async Task<ActionResult> GetFields(string projectName )
        {
            var project = await db.Projects.Include(p => p.Fields)
                                           .FirstOrDefaultAsync(p => p.Name == projectName);

            if (project == null)
            {
                return HttpNotFound();
            }

            var fields = project.Fields.Select(f => new { f.Name, f.Description }).ToList();
            return Json(fields, JsonRequestBehavior.AllowGet);
        }



        // POST: Project/RemoveField
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveField(int fieldId)
        {
            var field = await db.Fields.FindAsync(fieldId);
            if (field == null)
            {
                return HttpNotFound();
            }

            db.Fields.Remove(field);
            await db.SaveChangesAsync();

            return Json(new { success = true });
        }

        // GET: Project/GetProjectData
        public async Task<ActionResult> GetProjectData(int id)
        {
            var project = await db.Projects.Include(p => p.Fields).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return Json(project, JsonRequestBehavior.AllowGet);
        }
    }
}
