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
    public class UsersController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.ProviderProfiles);
            return View(await aspNetUsers.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Id = new SelectList(db.ProviderProfiles, "UserID", "Username");
            ViewBag.Id = new SelectList(db.UserProfiles, "UserID", "Username");
            ViewBag.Id = new SelectList(db.UserReference, "UserId", "ReferenceID");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,ConfirmationToken,PasswordVerificationToken,PasswordVerificationTokenExpirationDate")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.ProviderProfiles, "UserID", "Username", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.UserProfiles, "UserID", "Username", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.UserReference, "UserId", "ReferenceID", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.ProviderProfiles, "UserID", "Username", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.UserProfiles, "UserID", "Username", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.UserReference, "UserId", "ReferenceID", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,ConfirmationToken,PasswordVerificationToken,PasswordVerificationTokenExpirationDate")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUsers).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.ProviderProfiles, "UserID", "Username", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.UserProfiles, "UserID", "Username", aspNetUsers.Id);
            ViewBag.Id = new SelectList(db.UserReference, "UserId", "ReferenceID", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUsers);
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
