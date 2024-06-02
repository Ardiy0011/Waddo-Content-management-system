using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    public class FieldModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FieldModels
        public ActionResult Index()
        {
            var fields = db.Fields.Include(f => f.ProjectModel);
            return View(fields.ToList());
        }

        // GET: FieldModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldModel fieldModel = db.Fields.Find(id);
            if (fieldModel == null)
            {
                return HttpNotFound();
            }
            return View(fieldModel);
        }

        // GET: FieldModels/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: FieldModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectId")] FieldModel fieldModel)
        {
            if (ModelState.IsValid)
            {
                db.Fields.Add(fieldModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", fieldModel.ProjectId);
            return View(fieldModel);
        }

        // GET: FieldModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldModel fieldModel = db.Fields.Find(id);
            if (fieldModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", fieldModel.ProjectId);
            return View(fieldModel);
        }

        // POST: FieldModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectId")] FieldModel fieldModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fieldModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", fieldModel.ProjectId);
            return View(fieldModel);
        }

        // GET: FieldModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldModel fieldModel = db.Fields.Find(id);
            if (fieldModel == null)
            {
                return HttpNotFound();
            }
            return View(fieldModel);
        }

        // POST: FieldModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FieldModel fieldModel = db.Fields.Find(id);
            db.Fields.Remove(fieldModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
