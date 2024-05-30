using ProjectManager.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjectManager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Project
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description")] ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return Json(new { success = true, projectId = project.Id, projectName = project.Name });
            }

            return Json(new { success = false });
        }


        // GET: Project/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var project = await db.Projects.Include(p => p.Fields).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        // POST: Project/AddField
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddField(int projectId, string fieldName, string fieldType)
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
                Type = fieldType
            };

            db.Fields.Add(field);
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
