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
            foreach (var c in await db.Clinicians.Where(x => x.ProviderID == pid).ToListAsync())
            {
                model.CliniciansList.Add(new ListItem(c.SavedName, c.ID.ToString()));
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> CopyClinician(CliniciansModels clinicians)
        {
            if (clinicians.CopyClinician == 0 && clinicians.CopyClinician == -1)
            {
                ModelState.AddModelError("Invalid Clinician ID", "Cannot read clinician information, please select a clinician to copy from.");
                return View(clinicians.actionReferrer, clinicians);
            }
            ModelState.Clear();
            var srcClinician = await db.Clinicians.FindAsync(clinicians.CopyClinician);
            if (srcClinician ==null)
            {
                ModelState.AddModelError("Invalid Clinician ID", "Cannot read clinician information, please select a clinician to copy from.");
                return View(clinicians.actionReferrer, clinicians);
            }
            //start copy
            ObjectHandler.CopyPropertyValues(srcClinician, clinicians);
            clinicians.SavedName = "Copy_of_" + clinicians.ClinicianName;
            clinicians.ClinicianName = srcClinician.Name;
            clinicians.ClinicianEmail = srcClinician.Email;
            clinicians.ClinicianType = srcClinician.Type.Value;

            var locs = srcClinician.Location.Split('|');
            foreach(var loc in locs)
            {
                clinicians.SelectedLocation.Add(Convert.ToInt32(loc));
            }
            

            //end copy

            clinicians.Timetables = await db.ClinicianTimetable.Where(x => x.ClinicianID == clinicians.CopyClinician).ToListAsync();
            
            clinicians.ID = 0;
            clinicians.ProviderID = User.Identity.GetUserId();
            //clinicians.Timetable.StartDatetime =                clinicians.Timetable.EndDatetime = clinicians.Timetable.ExpiryDate = DateTime.Now;

            var pid = User.Identity.GetUserId();
            foreach (var c in await db.Clinicians.Where(x => x.ProviderID == pid).ToListAsync())
            {
                clinicians.CliniciansList.Add(new ListItem(c.SavedName, c.ID.ToString()));
            }
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians.actionReferrer, clinicians);
        }


        // POST: Clinician/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CliniciansModels clinicians)
        {
            clinicians.ID = 0;
            clinicians.ProviderID = User.Identity.GetUserId();

            //fixing posted data
            if (string.IsNullOrEmpty(clinicians.SavedName))
            {
                clinicians.SavedName = clinicians.Name + "_" + DateTime.Now;
                clinicians.SavedName = clinicians.SavedName.Replace(" ", "_");
            }
            //fix timetables as we manually rendered timetables and unchecked checked will post null
            foreach (var tt in clinicians.Timetables)
            {
                if (tt.OnMonday == null)
                    tt.OnMonday = false;
                if (tt.OnTuesday == null)
                    tt.OnTuesday = false;
                if (tt.OnWednesday == null)
                    tt.OnWednesday = false;
                if (tt.OnThursday == null)
                    tt.OnThursday = false;
                if (tt.OnFriday == null)
                    tt.OnFriday = false;
                if (tt.OnSaturday == null)
                    tt.OnSaturday = false;
                if (tt.OnSunday == null)
                    tt.OnSunday = false;
            }

            if (clinicians.TimetableType == 1)
                if (clinicians.Timetables.Count() == 0)
                    ModelState.AddModelError("Invalid Timetable", "Please add a timetable for this clinician, select 'No' if this clinician does not have any timetable.");
            if (clinicians.SelectedLocation.Count == 0)
                ModelState.AddModelError("Invalid Location", "Please select a location for this service .");

            if (ModelState.IsValid)
            {
                Clinicians dbClinicians = db.Clinicians.Create();
                ObjectHandler.CopyPropertyValues(clinicians, dbClinicians);
                dbClinicians.Name = clinicians.ClinicianName;
                dbClinicians.Email = clinicians.ClinicianEmail;
                dbClinicians.Type = clinicians.ClinicianType;
                dbClinicians.Location = "";
                foreach (var loc in clinicians.SelectedLocation)
                {
                    dbClinicians.Location += loc + "|";
                }
                dbClinicians.Location = dbClinicians.Location.Remove(dbClinicians.Location.Length - 1, 1);

                if (clinicians.TimetableType == 1)
                {
                    foreach (var tt in clinicians.Timetables)
                    {
                        ClinicianTimetable dbClinicianTimetable = db.ClinicianTimetable.Create();


                        ObjectHandler.CopyPropertyValues(tt, dbClinicianTimetable);
                        dbClinicianTimetable.StartDatetime = dbClinicianTimetable.EndDatetime = dbClinicianTimetable.ExpiryDate = DateTime.Now;
                        dbClinicianTimetable.ID = dbClinicians.ID;
                        dbClinicianTimetable.LocationID = tt.LocationID;
                        dbClinicians.ClinicianTimetable.Add(dbClinicianTimetable);

                    }
                }
                db.Clinicians.Add(dbClinicians);

                await db.SaveChangesAsync();
                return RedirectToAction("", "Clinicians");
            }

            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", clinicians.ProviderID);
            return View(clinicians);
        }

        // GET: Clinician/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var clinicians = new CliniciansModels();
            var srcClinician = await db.Clinicians.Where(x => x.ID == id).FirstOrDefaultAsync();
            ObjectHandler.CopyPropertyValues(srcClinician, clinicians);
            clinicians.ClinicianName = srcClinician.Name;
            clinicians.ClinicianEmail = srcClinician.Email;
            clinicians.ClinicianType = srcClinician.Type.Value;
            var locs = srcClinician.Location.Split('|');
            foreach (var loc in locs)
            {
                clinicians.SelectedLocation.Add(Convert.ToInt32(loc));
            }
            clinicians.Timetables = await db.ClinicianTimetable.Where(x => x.ClinicianID == id).ToListAsync();

            clinicians.ID = 0;
            clinicians.ProviderID = User.Identity.GetUserId();
            //clinicians.Timetable.StartDatetime =                clinicians.Timetable.EndDatetime = clinicians.Timetable.ExpiryDate = DateTime.Now;

            var pid = User.Identity.GetUserId();
            foreach (var c in await db.Clinicians.Where(x => x.ProviderID == pid).ToListAsync())
            {
                clinicians.CliniciansList.Add(new ListItem(c.SavedName, c.ID.ToString()));
            }
            return View(clinicians);
        }

        // POST: Clinician/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CliniciansModels clinicians)
        {
            try
            {
                foreach (var tt in clinicians.Timetables)
                {
                    if (tt.OnMonday == null)
                        tt.OnMonday = false;
                    if (tt.OnTuesday == null)
                        tt.OnTuesday = false;
                    if (tt.OnWednesday == null)
                        tt.OnWednesday = false;
                    if (tt.OnThursday == null)
                        tt.OnThursday = false;
                    if (tt.OnFriday == null)
                        tt.OnFriday = false;
                    if (tt.OnSaturday == null)
                        tt.OnSaturday = false;
                    if (tt.OnSunday == null)
                        tt.OnSunday = false;
                }
                if (clinicians.TimetableType == 1)
                    if (clinicians.Timetables.Count() == 0)
                        ModelState.AddModelError("Invalid Timetable", "Please add a timetable for this clinician, select 'No' if this clinician does not have any timetable.");
                if (clinicians.SelectedLocation.Count == 0)
                    ModelState.AddModelError("Invalid Location", "Please select a location for this service .");

                clinicians.ProviderID = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    Clinicians dbClinicians = await db.Clinicians.Where(x => x.ID == clinicians.ID).FirstOrDefaultAsync();
                    
                    ObjectHandler.CopyPropertyValues(clinicians, dbClinicians);
                    dbClinicians.Name = clinicians.ClinicianName;
                    dbClinicians.Email = clinicians.ClinicianEmail;
                    dbClinicians.Type = clinicians.ClinicianType;
                    dbClinicians.Location = "";
                    foreach (var loc in clinicians.SelectedLocation)
                    {
                        dbClinicians.Location += loc + "|";
                    }
                    dbClinicians.Location = dbClinicians.Location.Remove(dbClinicians.Location.Length - 1, 1);

                    var oldTTs = await db.ClinicianTimetable.Where(x => x.ClinicianID == dbClinicians.ID).ToListAsync();
                    foreach (var oldTT in oldTTs)
                    {
                        db.ClinicianTimetable.Remove(oldTT);
                    }


                    if (clinicians.TimetableType == 1)
                    {
                        foreach (var tt in clinicians.Timetables)
                        {
                            ClinicianTimetable dbClinicianTimetable = db.ClinicianTimetable.Create();

                            ObjectHandler.CopyPropertyValues(tt, dbClinicianTimetable);
                            dbClinicianTimetable.StartDatetime = dbClinicianTimetable.EndDatetime = dbClinicianTimetable.ExpiryDate = DateTime.Now;
                            dbClinicianTimetable.ID = dbClinicians.ID;
                            dbClinicianTimetable.LocationID = tt.LocationID;
                            dbClinicians.ClinicianTimetable.Add(dbClinicianTimetable);

                        }
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
                return View(clinicians);
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
