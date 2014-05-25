using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;
using Microsoft.AspNet.Identity;

namespace ISD.Application.provider.MVC.Controllers
{
    public class ClinicianController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Clinician
        public ActionResult Index(int id)
        {
            return View();
        }


        // GET: Clinician/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinician/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,LocationID,Phone,Email,Type,TimetableType")] Clinicians clinicians)
        {
            clinicians.ProviderID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Clinicians.Add(clinicians);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // GET: Clinician/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Clinician/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Clinician/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Clinician/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
