using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Backload.Controllers;
using BCUtility;
using ISD.Application.provider.MVC.Models;
using ISD.BF;
using ISD.Data.EDM;
using ISD.EDS;
using ISD.Util;
using Microsoft.AspNet.Identity;
using System.IO;

namespace ISD.Application.provider.MVC.Controllers
{
    
    public class ServiceController : Controller
    {
        private ISDEntities db = new ISDEntities();
        [Authorize(Roles = @SystemConstants.ProviderRole)]
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
            model.ImageInfo = await db.ActivityImage.Where(x => x.ActivityID == id).FirstOrDefaultAsync();
            if (model.ImageInfo != null)
                model.Images = await db.ActivityImageDetail.Where(x => x.ActivityImageID == model.ImageInfo.ID).ToListAsync();
            model.Services = await db.v_ActivityExplorer.Where(x => x.PrimaryServiceID == id).ToListAsync();
            model.Clinicians = await db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == id).ToListAsync();
            return View(model);
        }
        [Authorize(Roles = @SystemConstants.ProviderRole)]
        public async Task<ActionResult> Detail(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbService = await db.v_ActivityExplorer.Where(x => x.Name == name && x.isPrimary == true).FirstOrDefaultAsync();
            if (dbService == null)
            {
                return HttpNotFound();
            }
            ServiceDetailModel model = new ServiceDetailModel();
            ObjectHandler.CopyPropertyValues(dbService, model);
            model.Images = await db.ActivityImageDetail.Where(x => x.ActivityID == dbService.ID).ToListAsync();
            model.Services = await db.v_ActivityExplorer.Where(x => x.PrimaryServiceID == dbService.ID).ToListAsync();
            model.Clinicians = await db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == dbService.ID).ToListAsync();
            return View(model);
        }

        [Authorize]
        // GET: Service/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username");
            ServiceDetailModel model = new ServiceDetailModel();

            foreach (var state in await db.State.ToListAsync())
            {
                model.StatesList.Add(new ListItem(state.StateName, state.ID.ToString()));
            }

            foreach (var suburb in await db.Suburb.ToListAsync())
            {
                model.SuburbList.Add(new ListItem(suburb.Name, suburb.ID.ToString()));
            }
            model.CliniciansList = await db.v_ProviderClinicians.ToListAsync();
            model.Categories = await db.v_CategoryExplorer.ToListAsync();
            return View(model);
        }

        // POST: Service/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ICollection<HttpPostedFileBase> files, ServiceDetailModel model)
        {
            var a = Request.Files.Count;
            int SizeUploaded = 0;
            //check if any file
            if (files.First() != null && files.Count != 0)
            {
                foreach (var file in files)
                {
                    SizeUploaded = SizeUploaded + Convert.ToInt32(file.ContentLength / 1024);
                    //checking file extension
                    string extension = Path.GetExtension(file.FileName);
                    if (!System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "image/\\S+"))
                    {
                        ModelState.AddModelError("Upload Error", "Invalid file type, only image files are accepted.");
                        break;
                    }
                }
                if (SizeUploaded >= SystemConstants.MaxActivityImageStorage)
                {
                    ModelState.AddModelError("Upload Error", "Maximum file size exceeded, Maximum Size per activity is" + SystemConstants.MaxActivityImageStorage + " Kb");
                }
            }
            if (ModelState.IsValid)
            {
                Activity act = db.Activity.Create();
                ActivityContactDetail actCon = db.ActivityContactDetail.Create();

                ObjectHandler.CopyPropertyValues(model, act);
                ObjectHandler.CopyPropertyValues(model, actCon);

                //Set activity
                act.CategoryID = act.SecondaryCategoryID1 =
                    act.SecondaryCategoryID2 = act.SecondaryCategoryID3 =
                        act.SecondaryCategoryID4 = 0;
                act.Status = (int)SystemConstants.ActivityStatus.Active;
                act.isApproved = false;
                act.isPrimary = true;
                act.CreatedBy = act.ModifiedBy = User.Identity.Name;
                act.CreatedDateTime = act.ModifiedDateTime = DateTime.Now;
                act.ExpiryDate = DateTime.Now.AddDays(180);
                if (model.ShortDescription == null)
                    act.ShortDescription = string.Empty;
                if (model.eligibilityDescription == null)
                    act.eligibilityDescription = string.Empty;
                if (model.Price == null)
                    act.Price = string.Empty;

                act.ActivityCode = Guid.NewGuid().ToString();
                act.ProviderID = User.Identity.GetUserId();
                actCon.Username = User.Identity.Name;
                actCon.ActivityID = act.ID;

                int i = 1;
                foreach (var cat in model.SelectedCategory)
                {
                    if (i == 1) { act.CategoryID = cat; }
                    if (i == 2) { act.SecondaryCategoryID1 = cat; }
                    if (i == 3) { act.SecondaryCategoryID2 = cat; }
                    if (i == 4) { act.SecondaryCategoryID3 = cat; }
                    if (i == 5) { act.SecondaryCategoryID4 = cat; }
                    i++;
                }

                foreach (var sc in model.SelectedClinicians)
                {
                    var ac = db.ActivityClinician.Create();
                    ac.ActivityID = act.ID;
                    ac.ClinicianID = sc;
                    ac.ModifiedBy = User.Identity.Name;
                    ac.ModifiedDatetime = DateTime.Now;
                    ac.Description = act.Name;
                    act.ActivityClinician.Add(ac);
                }

                var image = db.ActivityImage.Create();
                foreach (var file in files)
                {
                    var imageDetail = db.ActivityImageDetail.Create();

                    imageDetail.ActivityID = act.ID;
                    imageDetail.ActivityImageID = 0;
                    imageDetail.ImageStream = new BCUtility.ImageHandler().ReadFully(file.InputStream);
                    imageDetail.Filename = file.FileName;
                    imageDetail.Filesize = file.ContentLength / 1024;
                    image.ActivityImageDetail.Add(imageDetail);
                }
               
                image.StorageUsed = SizeUploaded;
                image.FreeStorage = SystemConstants.MaxActivityImageStorage - SizeUploaded;
                image.ImageAmount = files.Count;
                act.ActivityImage.Add(image);
                //Image handling

                act.ActivityContactDetail.Add(actCon);

                var Ref = db.ActivityReferenceCode.Create();
                Ref.ActivityID = act.ID;
                Ref.ActivityGUID = Guid.NewGuid();
                Ref.ReferenceID = new BusinessFunctionComponent().GenerateActRefID(act.Name);
                act.ActivityReferenceCode.Add(Ref);

                db.Activity.Add(act);
                //set act ID to images
                db.SaveChanges();

                return RedirectToAction("edit", "service", new { id = act.ID });

            }
            //failed
            foreach (var state in await db.State.ToListAsync())
            {
                model.StatesList.Add(new ListItem(state.StateName, state.ID.ToString()));
            }

            foreach (var suburb in await db.Suburb.ToListAsync())
            {
                model.SuburbList.Add(new ListItem(suburb.Name, suburb.ID.ToString()));
            }
            model.CliniciansList = await db.v_ProviderClinicians.ToListAsync();
            model.Categories = await db.v_CategoryExplorer.ToListAsync();
            return View(model);
        }
        // GET: Service/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDetailModel model = new ServiceDetailModel();
            var actExplorer = db.v_ActivityExplorer.Where(x => x.ID == id).FirstOrDefault();
            if (actExplorer == null)
            {
                return HttpNotFound();
            }

            ObjectHandler.CopyPropertyValues(actExplorer, model);


            foreach (var c in db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == id).ToList())
            {
                model.SelectedClinicians.Add(c.ClinicianID.Value);
            }


            if (model.CategoryID != 0)
                model.SelectedCategory.Add(model.CategoryID.Value);
            if (model.SecondaryCategoryID1 != 0)
                model.SelectedCategory.Add(model.SecondaryCategoryID1.Value);
            if (model.SecondaryCategoryID2 != 0)
                model.SelectedCategory.Add(model.SecondaryCategoryID2.Value);
            if (model.SecondaryCategoryID3 != 0)
                model.SelectedCategory.Add(model.SecondaryCategoryID3.Value);
            if (model.SecondaryCategoryID4 != 0)
                model.SelectedCategory.Add(model.SecondaryCategoryID4.Value);



            foreach (var state in await db.State.ToListAsync())
            {
                model.StatesList.Add(new ListItem(state.StateName, state.ID.ToString()));
            }

            foreach (var suburb in await db.Suburb.ToListAsync())
            {
                model.SuburbList.Add(new ListItem(suburb.Name, suburb.ID.ToString()));
            }
            model.CliniciansList = await db.v_ProviderClinicians.ToListAsync();
            model.Categories = await db.v_CategoryExplorer.ToListAsync();
            model.ImageInfo = await db.ActivityImage.Where(x => x.ActivityID == id).FirstOrDefaultAsync();
            if (model.ImageInfo != null)
                model.Images = await db.ActivityImageDetail.Where(x => x.ActivityImageID == model.ImageInfo.ID).ToListAsync();
            //Activity activity = await db.Activity.FindAsync(id);

            ViewBag.ProviderID = new SelectList(db.ProviderProfiles, "UserID", "Username", User.Identity.GetUserId());
            return View(model);
        }

        // POST: Service/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ServiceDetailModel model)
        {
            if (ModelState.IsValid)
            {
                Activity act = await db.Activity.Where(x => x.ID == model.ID).FirstOrDefaultAsync();
                ActivityContactDetail actCon = await db.ActivityContactDetail.Where(x => x.ActivityID == model.ID).FirstOrDefaultAsync();

                /*Edit Code Here
                we dont use this code as it will overwrite some record to null
                ObjectHandler.CopyPropertyValues(model, act);
                ObjectHandler.CopyPropertyValues(model, actCon);
                */
                act.Name = model.Name;
                actCon.Email = model.Email;
                actCon.PhoneNumber = model.PhoneNumber;
                act.Website = model.Website;
                actCon.Address = model.Address;
                actCon.SuburbID = model.SuburbID;
                actCon.StateID = model.StateID;
                actCon.PostCode = model.PostCode;
                act.FullDescription = model.FullDescription;
                act.eligibilityDescription = model.eligibilityDescription;
                act.Price = model.Price;
                act.Keywords = model.Keywords;

                //Set activity
                act.CategoryID = act.SecondaryCategoryID1 =
                    act.SecondaryCategoryID2 = act.SecondaryCategoryID3 =
                        act.SecondaryCategoryID4 = 0;
                act.Status = (int)SystemConstants.ActivityStatus.Active;
                act.ModifiedBy = User.Identity.Name;
                act.ModifiedDateTime = DateTime.Now;
                act.ExpiryDate = DateTime.Now.AddDays(180);
                if (model.FullDescription == null)
                    act.FullDescription = string.Empty;
                else
                    if (model.eligibilityDescription == null)
                        act.eligibilityDescription = string.Empty;
                if (model.Price == null)
                    act.Price = string.Empty;

                int i = 1;
                foreach (var cat in model.SelectedCategory)
                {
                    if (i == 1) { act.CategoryID = cat; }
                    if (i == 2) { act.SecondaryCategoryID1 = cat; }
                    if (i == 3) { act.SecondaryCategoryID2 = cat; }
                    if (i == 4) { act.SecondaryCategoryID3 = cat; }
                    if (i == 5) { act.SecondaryCategoryID4 = cat; }
                    i++;
                }
                //Recreate Clinician
                foreach (var osc in await db.ActivityClinician.Where(x => x.ActivityID == model.ID).ToListAsync())
                {
                    db.ActivityClinician.Remove(osc);
                }
                foreach (var sc in model.SelectedClinicians)
                {
                    var ac = db.ActivityClinician.Create();
                    ac.ActivityID = act.ID;
                    ac.ClinicianID = sc;
                    ac.ModifiedBy = User.Identity.Name;
                    ac.ModifiedDatetime = DateTime.Now;
                    ac.Description = act.Name;
                    act.ActivityClinician.Add(ac);
                }

                await db.SaveChangesAsync();

                return RedirectToAction("edit", "service", new { id = act.ID });

            }
            //failed
            foreach (var state in await db.State.ToListAsync())
            {
                model.StatesList.Add(new ListItem(state.StateName, state.ID.ToString()));
            }

            foreach (var suburb in await db.Suburb.ToListAsync())
            {
                model.SuburbList.Add(new ListItem(suburb.Name, suburb.ID.ToString()));
            }
            model.CliniciansList = await db.v_ProviderClinicians.ToListAsync();
            model.Categories = await db.v_CategoryExplorer.ToListAsync();
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditImage(ICollection<HttpPostedFileBase> files, ServiceImageDetailModel model)
        {
            int SizeUploaded = 0;
            //check if any file
            if (files.First() != null && files.Count != 0)
            {
                foreach (var file in files)
                {
                    SizeUploaded = SizeUploaded + Convert.ToInt32(file.ContentLength / 1024);
                    //checking file extension
                    string extension = Path.GetExtension(file.FileName);
                    if (!System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "image/\\S+"))
                    {
                        ModelState.AddModelError("Upload Error", "Invalid file type, only image files are accepted.");
                        break;
                    }
                }
                if (SizeUploaded >= model.FreeStorage)
                {
                    ModelState.AddModelError("Upload Error", "Maximum file size exceeded, Maximum Size per activity is" + SystemConstants.MaxActivityImageStorage + " Kb");
                }
            }
            if (ModelState.IsValid)
            {
                var image = await db.ActivityImage.Where(x => x.ID == model.ImageInfoID).FirstOrDefaultAsync();
                if (image == null)
                {
                    ModelState.AddModelError("Upload Error", "Image Information not found. Please Contact your administrator.");
                    TempData["ViewData"] = ViewData;
                    return RedirectToAction("Edit", new { id = model.ActivityID });
                }
                foreach (var file in files)
                {
                    var imageDetail = db.ActivityImageDetail.Create();

                    imageDetail.ActivityID = model.ActivityID;
                    imageDetail.ActivityImageID = 0;
                    imageDetail.ImageStream = new BCUtility.ImageHandler().ReadFully(file.InputStream);
                    imageDetail.Filename = file.FileName;
                    imageDetail.Filesize = file.ContentLength / 1024;
                    image.ActivityImageDetail.Add(imageDetail);
                }
                image.StorageUsed = SizeUploaded + model.StorageUsed;
                image.FreeStorage = SystemConstants.MaxActivityImageStorage - image.StorageUsed;
                image.ImageAmount = model.ImageAmount + files.Count;

                db.SaveChanges();

                return RedirectToAction("edit", "service", new { id = model.ActivityID });
            }
            TempData["ViewData"] = ViewData;
            return RedirectToAction("Edit", new { id = model.ActivityID });
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
