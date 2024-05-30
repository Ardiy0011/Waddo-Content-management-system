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
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("CreateFields", new { id = project.Id });
            }

            return View(project);
        }

        // GET: Project/CreateFields
        public ActionResult CreateFields(int id)
        {
            ViewBag.ProjectId = id;
            return View();
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

            return RedirectToAction("CreateFields", new { id = projectId });
        }
    }
}
