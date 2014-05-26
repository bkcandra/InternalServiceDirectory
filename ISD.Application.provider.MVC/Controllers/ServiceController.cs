using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BCUtility;
using ISD.Application.provider.MVC.Models;
using ISD.Data.EDM;

namespace ISD.Application.provider.MVC.Controllers
{
    public class ServiceController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Service
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbService = await db.v_ActivityExplorer.Where(x => x.ID == id && x.isPrimary == true).FirstOrDefaultAsync();
            if (dbService == null)
            {
                return HttpNotFound();
            }
            ServiceDetailModel model = new ServiceDetailModel();
            ObjectHandler.CopyPropertyValues(dbService, model);
            model.Images = await db.ActivityImageDetail.Where(x => x.ActivityID == id).ToListAsync();
            model.Services = await db.v_ActivityExplorer.Where(x => x.PrimaryServiceID == id).ToListAsync();
            model.Clinicians = await db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == id).ToListAsync();
            return View(model);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username");
            return View();
        }

        // POST: Service/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProviderID,ActivityCode,Name,ShortDescription,FullDescription,CategoryID,SecondaryCategoryID1,SecondaryCategoryID2,SecondaryCategoryID3,SecondaryCategoryID4,Price,CreatedDateTime,ModifiedDateTime,ModifiedBy,CreatedBy,ExpiryDate,ActivityType,eligibilityDescription,Status,Website,Keywords,TimetableType,isApproved,isPrimary,PrimaryServiceID")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activity.Add(activity);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", activity.ProviderID);
            return View(activity);
        }

        // GET: Service/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = await db.Activity.FindAsync(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", activity.ProviderID);
            return View(activity);
        }

        // POST: Service/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProviderID,ActivityCode,Name,ShortDescription,FullDescription,CategoryID,SecondaryCategoryID1,SecondaryCategoryID2,SecondaryCategoryID3,SecondaryCategoryID4,Price,CreatedDateTime,ModifiedDateTime,ModifiedBy,CreatedBy,ExpiryDate,ActivityType,eligibilityDescription,Status,Website,Keywords,TimetableType,isApproved,isPrimary,PrimaryServiceID")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", activity.ProviderID);
            return View(activity);
        }

        // GET: Service/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = await db.Activity.FindAsync(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Activity activity = await db.Activity.FindAsync(id);
            db.Activity.Remove(activity);
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
