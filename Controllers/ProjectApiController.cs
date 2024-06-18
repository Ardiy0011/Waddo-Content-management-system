using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{

    public class ProjectApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        // GET api/ProjectApi/GetFieldDescription/{projectName}/{fieldName}
        [HttpGet]
        [Route("api/ProjectApi/GetFieldDescription/{projectName}/{fieldName}")]
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




        // GET api/ProjectApi/GetFields/{projectName}
        [HttpGet]
        [Route("api/ProjectApi/GetFields/{projectName}")]
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
