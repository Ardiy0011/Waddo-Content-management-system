using ProjectManager.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using System.Data.Entity;

namespace ProjectManager.Controllers
{
    public class TestController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        // GET api/test/index
        [HttpGet]
        [Route("api/test/index")]
        public IHttpActionResult Index()
        {
            return Content(System.Net.HttpStatusCode.OK, "Hello, World! This is a test endpoint.");
        }

        // GET api/test/print/{message}
        [HttpGet]
        [Route("api/test/print/{message}")]
        public IHttpActionResult Print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            Logger.Info($"Message received: {message}");
            return Content(System.Net.HttpStatusCode.OK, $"Message received: {message}");
        }

        // GET api/test/getall
        [HttpGet]
        [Route("api/test/getall")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var allData = await db.Projects.Include("Fields").ToListAsync(); // Corrected this line
                Logger.Info($"Retrieved {allData.Count} projects with their fields.");

                // Return the data in JSON format
                return Ok(allData);
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex, "Error occurred while retrieving data.");
                return InternalServerError(ex);
            }

        }


        // GET api/test/getFieldDescription/{projectName}/{fieldName}
        [HttpGet]
        [Route("api/test/getFieldDescription/{projectName}/{fieldName}")]
        public async Task<IHttpActionResult> GetFieldDescription(string projectName, string fieldName)
        {
            try
            {
                var project = await db.Projects.Include("Fields").FirstOrDefaultAsync(p => p.Name == projectName);

                if (project == null)
                {
                    Logger.Warn($"Project '{projectName}' not found.");
                    return NotFound();
                }

                var field = project.Fields.FirstOrDefault(f => f.Name == fieldName);
                if (field == null)
                {
                    Logger.Warn($"Field '{fieldName}' not found in project '{projectName}'.");
                    return NotFound();
                }

                Logger.Info($"Field '{fieldName}' in project '{projectName}' retrieved successfully.");
                return Ok(field.Description);
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex, "Error occurred while retrieving field description.");
                return InternalServerError(ex);
            }
        }


        // GET api/test/getFields/{projectName}
        [HttpGet]
        [Route("api/test/getFields/{projectName}")]
        public async Task<IHttpActionResult> GetFields(string projectName)
        {
            try
            {
                var project = await db.Projects.Include("Fields").FirstOrDefaultAsync(p => p.Name == projectName);

                if (project == null)
                {
                    Logger.Warn($"Project '{projectName}' not found.");
                    return NotFound();
                }

                Logger.Info($"Retrieved {project.Fields.Count} fields for project '{projectName}'.");

                var fields = project.Fields.Select(f => new { f.Name, f.Description }).ToList();
                return Ok(fields);
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex, "Error occurred while retrieving fields.");
                return InternalServerError(ex);
            }
        }
        }
}
