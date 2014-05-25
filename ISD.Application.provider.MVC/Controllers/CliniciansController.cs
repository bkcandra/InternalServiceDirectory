using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;

namespace ISD.Application.provider.MVC.Controllers
{
    public class CliniciansController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Clinicians
        public async Task<ActionResult> Index()
        {
            var clinicians = db.Clinicians.Include(c => c.ProviderProfiles);
            return View(await clinicians.ToListAsync());
        }

        // GET: Clinicians/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinicians clinicians = await db.Clinicians.FindAsync(id);
            if (clinicians == null)
            {
                return HttpNotFound();
            }
            return View(clinicians);
        }

        // GET: Clinicians/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinicians clinicians = await db.Clinicians.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProviderID,Name,LocationID,Phone,Email,Type,TimetableType")] Clinicians clinicians)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinicians).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // GET: Clinicians/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinicians clinicians = await db.Clinicians.FindAsync(id);
            if (clinicians == null)
            {
                return HttpNotFound();
            }
            return View(clinicians);
        }

        // POST: Clinicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Clinicians clinicians = await db.Clinicians.FindAsync(id);
            db.Clinicians.Remove(clinicians);
            await db.SaveChangesAsync();
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
