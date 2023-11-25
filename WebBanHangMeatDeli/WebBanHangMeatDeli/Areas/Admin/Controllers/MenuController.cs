using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        // GET: Admin/Menu
        public ActionResult Index()
        {
            return View(db.tb_Menu.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        // GET: Admin/Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "Id,Title,Description,Alias,Position,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] tb_Menu tb_Menu )
        {
            if (ModelState.IsValid)
            {
                tb_Menu.CreatedDate = DateTime.Now.ToString();
                tb_Menu.ModifiedDate = DateTime.Now.ToString();
                tb_Menu.Alias = Models.Convert.convert.FilterChar(tb_Menu.Title);
                db.tb_Menu.Add(tb_Menu);
                tb_Menu.Position = db.tb_Menu.Count() + 1;
                db.SaveChanges();
                TempData["message"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            return View(tb_Menu);

        }

        public ActionResult Edit( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Menu tb_Menu = db.tb_Menu.Find(id);
            if (tb_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tb_Menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind(Include = "Id,Title,Description,Alias,Position,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] tb_Menu tb_Menu )
        {
            if (ModelState.IsValid)
            {
                tb_Menu.ModifiedDate = DateTime.Now.ToString();
                tb_Menu.Alias = Models.Convert.convert.FilterChar(tb_Menu.Title);
                db.Entry(tb_Menu).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Sửa thành công!";
                return RedirectToAction("Index");
            }
            return View(tb_Menu);
        }

        [HttpPost]
        public ActionResult Delete( int id )
        {
            tb_Menu tb_Menu = db.tb_Menu.Find(id);
            if (tb_Menu != null)
            {
                db.tb_Menu.Remove(tb_Menu);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        protected override void Dispose( bool disposing )
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
