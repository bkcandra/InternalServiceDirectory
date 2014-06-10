using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BCUtility;
using ISD.Application.provider.MVC.Models;
using ISD.Data.EDM;
using Microsoft.AspNet.Identity;
using ISD.Util;
using System.Web.UI.WebControls;

namespace ISD.Application.provider.MVC.Controllers
{

    public class ClinicianController : Controller
    {
        private ISDEntities db = new ISDEntities();

        [Authorize(Roles = @SystemConstants.ProviderRole)]
        // GET: Clinician
        public ActionResult Index(int id)
        {
            return View();
        }


        // GET: Clinician/Create
        public async Task<ActionResult> Create()
        {
            var model = new CliniciansModels();
            var pid = User.Identity.GetUserId();
            foreach (var c in await db.v_ProviderClinicians.Where(x => x.ProviderID == pid).ToListAsync())
            {
                model.CliniciansList.Add(new ListItem(c.Name, c.ID.ToString()));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> CopyClinician(CliniciansModels clinicians)
        {
            if (clinicians.CopyClinician == 0 && clinicians.CopyClinician == -1)
            {
                ModelState.AddModelError("Invalid Clinician ID", "Cannot read clinician information, please select a clinician to copy from.");
                return View(clinicians);
            }
            ModelState.Clear();
            var srcClinician = await db.Clinicians.FindAsync(clinicians.CopyClinician);
            //start copy
            ObjectHandler.CopyPropertyValues(srcClinician, clinicians);
            clinicians.ClinicianName = srcClinician.Name;
            clinicians.ClinicianEmail = clinicians.Email;
            clinicians.ClinicianType = clinicians.Type.Value;
            clinicians.SavedName = "Copy_of_" + clinicians.ClinicianName;
            //end copy

            var timetable = await db.ClinicianTimetable.Where(x => x.ClinicianID == clinicians.CopyClinician).ToListAsync();
            clinicians.ID = 0;
            clinicians.ProviderID = User.Identity.GetUserId();
            clinicians.Timetable.StartDatetime =
                clinicians.Timetable.EndDatetime = clinicians.Timetable.ExpiryDate = DateTime.Now;

            var pid = User.Identity.GetUserId();
            foreach (var c in await db.v_ProviderClinicians.Where(x => x.ProviderID == pid).ToListAsync())
            {
                clinicians.CliniciansList.Add(new ListItem(c.Name, c.ID.ToString()));
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View("Create", clinicians);
        }


        // POST: Clinician/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CliniciansModels clinicians)
        {
            clinicians.ID = 0;
            clinicians.ProviderID = User.Identity.GetUserId();
            clinicians.Timetable.StartDatetime =
                clinicians.Timetable.EndDatetime = clinicians.Timetable.ExpiryDate = DateTime.Now;
            if (string.IsNullOrEmpty(clinicians.SavedName))
            {
                clinicians.SavedName = clinicians.Name + "_" + DateTime.Now;
                clinicians.SavedName = clinicians.SavedName.Replace(" ", "_");
            }
            if (ModelState.IsValid)
            {
                Clinicians dbClinicians = new Clinicians();
                ObjectHandler.CopyPropertyValues(clinicians, dbClinicians);
                dbClinicians.Name = clinicians.ClinicianName;
                dbClinicians.Email = clinicians.ClinicianEmail;
                dbClinicians.Type = clinicians.ClinicianType;

                db.Clinicians.Add(dbClinicians);
                if (clinicians.TimetableType == 1)
                {
                    ClinicianTimetable dbClinicianTimetable = new ClinicianTimetable();
                    ObjectHandler.CopyPropertyValues(clinicians.Timetable, dbClinicianTimetable);
                    dbClinicianTimetable.ID = dbClinicians.ID;
                    db.ClinicianTimetable.Add(dbClinicianTimetable);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("", "Clinicians");
            }

            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // GET: Clinician/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = new CliniciansModels();
            var dbClinician = await db.v_ProviderClinicians.Where(x => x.ID == id).FirstOrDefaultAsync();
            ObjectHandler.CopyPropertyValues(dbClinician, model);
            model.ClinicianEmail = dbClinician.Email;
            model.ClinicianName = dbClinician.Name;
            if (dbClinician.Type == 1)
                model.ClinicianType = 1;
            else
                model.ClinicianType = 2;
            if (dbClinician.TimetableType == 0)
            {
                dbClinician.OnMonday =
                    dbClinician.OnTuesday =
                        dbClinician.OnWednesday =
                            dbClinician.OnThursday =
                                dbClinician.OnFriday = dbClinician.OnSaturday = dbClinician.OnSunday = false;
            }
            else
            {
                ObjectHandler.CopyPropertyValues(dbClinician, model.Timetable);
            }
            return View(model);
        }

        // POST: Clinician/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, FormCollection collection, [Bind(Include = "ID,ClinicianName,Location,Phone,ClinicianEmail,ClinicianType,TimetableType,OnMonday,OnTuesday,OnWednesday,OnThursday,OnFriday,OnSaturday,OnSunday,RecurEvery")] CliniciansModels clinicians)
        {
            try
            {
                clinicians.ID = id;
                clinicians.ProviderID = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    Clinicians dbClinicians = await db.Clinicians.Where(x => x.ID == id).FirstOrDefaultAsync();
                    ObjectHandler.CopyPropertyValues(clinicians, dbClinicians);
                    dbClinicians.Name = clinicians.ClinicianName;
                    dbClinicians.Email = clinicians.ClinicianEmail;
                    dbClinicians.Type = clinicians.ClinicianType;
                    if (clinicians.TimetableType == 1)
                    {

                        ClinicianTimetable dbClinicianTimetable =
                            await db.ClinicianTimetable.Where(x => x.ID == id).FirstOrDefaultAsync();
                        dbClinicianTimetable.StartDatetime =
                   dbClinicianTimetable.EndDatetime = dbClinicianTimetable.ExpiryDate = DateTime.Now;

                        ObjectHandler.CopyPropertyValues(clinicians, dbClinicianTimetable);
                        dbClinicianTimetable.ID = id;
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction("", "Clinicians");
                }
                else
                {
                    ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
                    return View(clinicians);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Clinician/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var dbClinician = await db.v_ProviderClinicians.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (dbClinician.TimetableType == 0)
                dbClinician.OnMonday = dbClinician.OnTuesday = dbClinician.OnWednesday = dbClinician.OnThursday = dbClinician.OnFriday = dbClinician.OnSaturday = dbClinician.OnSunday = false;
            return View(dbClinician);
        }

        // POST: Clinician/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var ct = await db.ClinicianTimetable.Where(x => x.ClinicianID == id).FirstOrDefaultAsync();
                var c = await db.Clinicians.Where(x => x.ID == id).FirstOrDefaultAsync();
                db.ClinicianTimetable.Remove(ct);
                db.Clinicians.Remove(c);
                await db.SaveChangesAsync();
                return RedirectToAction("", "Clinicians");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Timetable()
        {

            return PartialView("_PartialServiceTimetable");
        }
    }
}
