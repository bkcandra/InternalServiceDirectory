using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;

namespace ISD.Provider.Web.Controllers
{
    public class CliniciansController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Clinicians
        public ActionResult Index()
        {
            var clinicians = db.Clinicians.Include(c => c.ProviderProfiles);
            return View(clinicians.ToList());
        }

        // GET: Clinicians/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinicians clinicians = db.Clinicians.Find(id);
            if (clinicians == null)
            {
                return HttpNotFound();
            }
            return View(clinicians);
        }

        // GET: Clinicians/Create
        public ActionResult Create()
        {
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username");
            return View();
        }

        // POST: Clinicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProviderID,Name,LocationID,Phone,Email,Type,TimetableType")] Clinicians clinicians)
        {
            if (ModelState.IsValid)
            {
                db.Clinicians.Add(clinicians);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // GET: Clinicians/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinicians clinicians = db.Clinicians.Find(id);
            if (clinicians == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // POST: Clinicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProviderID,Name,LocationID,Phone,Email,Type,TimetableType")] Clinicians clinicians)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinicians).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // GET: Clinicians/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinicians clinicians = db.Clinicians.Find(id);
            if (clinicians == null)
            {
                return HttpNotFound();
            }
            return View(clinicians);
        }

        // POST: Clinicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinicians clinicians = db.Clinicians.Find(id);
            db.Clinicians.Remove(clinicians);
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
