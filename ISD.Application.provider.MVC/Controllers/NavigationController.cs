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
using ISD.BF;

namespace ISD.Application.provider.MVC.Controllers
{
    public class NavigationController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Menu
        public async Task<ActionResult> Index()
        {
            return View(await db.Menu.OrderBy(x => x.Sequence).ToListAsync());
        }

        // GET: Menu/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu Menu = await db.Menu.FindAsync(id);
            if (Menu == null)
            {
                return HttpNotFound();
            }
            return View(Menu);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(v_Menu Menu)
        {
            if (ModelState.IsValid)
            {
                var menu = new Menu();
                menu.ParentMenuID = 0;
                if (Menu.MenuType == ((int)ISD.Util.SystemConstants.MenuTargetType.Page))
                {
                   
                    menu.Sequence = OrderNavigation();
                    menu.LinkID = Menu.LinkID;
                    menu.MenuType = Menu.MenuType;
                    db.Menu.Add(menu);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    var link = new Link();
                    link.LinkType = Menu.MenuType;
                    link.LinkText = Menu.LinkText;

                    menu.Link = link;
                    menu.Sequence = OrderNavigation();
                    menu.LinkID = link.ID;
                    menu.MenuType = Menu.MenuType;
                    db.Menu.Add(menu);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return View(Menu);
        }

        // GET: Menu/Edit/5
        public dynamic Ordering(int id, bool increaseOrder)
        {
            string message = "";
            int fromId = 0;
            int toId = 0;
            if (id == 0)
            {
                return Json(new { status = false, Message = "Invalid MenuId" });
            }
            Menu Menu = db.Menu.Find(id);
            if (Menu == null)
            {
                return Json(new { status = false, Message = "could not find Menu" });
            }
            if (OrderNavigation(id, increaseOrder, out fromId, out toId, out message))
                return Json(new { status = true, fromId = fromId, toId = toId, Message = message });
            else
                return Json(new { status = false, fromId = fromId, toId = toId, Message = message });
        }

        // GET: Menu/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            v_Menu Menu = await db.v_Menu.Where(x=>x.ID == id).FirstOrDefaultAsync();
            if (Menu == null)
            {
                return HttpNotFound();
            }
            return View(Menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(v_Menu Menu)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    var menu = db.Menu.Where(x=>x.ID == Menu.ID).FirstOrDefault();
                    menu.ParentMenuID = 0;
                    if (Menu.MenuType == ((int)ISD.Util.SystemConstants.MenuTargetType.Page))
                    {

                        menu.Sequence = OrderNavigation();
                        menu.LinkID = Menu.LinkID;
                        menu.MenuType = Menu.MenuType;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var link =db.Link.Where(x=>x.ID == Menu.LinkID).FirstOrDefault();
                        link.LinkType = Menu.MenuType;
                        link.LinkText = Menu.LinkText;
                        link.LinkValue = Menu.LinkValue;
 
                        menu.Sequence = OrderNavigation();
                        menu.LinkID = link.ID;
                        menu.MenuType = Menu.MenuType;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }

                return View(Menu);
            }
            return View(Menu);
        }
      
        // GET: Menu/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu Menu = await db.Menu.FindAsync(id);
            if (Menu == null)
            {
                return HttpNotFound();
            }
            return View(Menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Menu Menu = await db.Menu.FindAsync(id);
            db.Menu.Remove(Menu);
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

        public int OrderNavigation()
        {
            var nav = db.Menu.ToList();
            return nav.Count + 1;
        }

        public bool OrderNavigation(int navId, bool increaseOrder, out int swapFrom, out int swapTo, out string message)
        {
            message = "Success swapping navigation";
            var navi = db.Menu.ToList();
            var from = new ISD.Data.EDM.Menu();
            if (increaseOrder)
            {
                from = navi.Where(x => x.ID == navId).FirstOrDefault();
                if (from.Sequence > 1)
                {
                    var to = navi.Where(x => x.Sequence == from.Sequence - 1).FirstOrDefault();
                    to.Sequence++;
                    from.Sequence--;
                    db.SaveChanges();
                    swapFrom = from.ID;
                    swapTo = to.ID;
                    return true;
                }
            }
            else
            {
                from = navi.Where(x => x.ID == navId).FirstOrDefault();
                if (from.Sequence < navi.Count)
                {
                    var to = navi.Where(x => x.Sequence == from.Sequence + 1).FirstOrDefault();
                    to.Sequence--;
                    from.Sequence++;
                    db.SaveChanges();
                    swapFrom = from.ID;
                    swapTo = to.ID;
                    return true;
                }
            }
            message = "Navigation cannot be reordered.";
            swapFrom = 0;
            swapTo = 0;
            return false;

        }
    }
}
