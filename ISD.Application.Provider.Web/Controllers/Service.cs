﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;

namespace ISD.Provider.Web.Controllers
{
    public class Service : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Service
        public async Task<ActionResult> Index()
        {
            var activity = db.Activity;
            return View(await activity.ToListAsync());
        }

        // GET: Service/Details/5
        public async Task<ActionResult> Details(int? id)
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
