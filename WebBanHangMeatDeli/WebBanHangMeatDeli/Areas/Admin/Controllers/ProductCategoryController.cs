using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var tb_ProductCatagory = db.tb_ProductCatagory.Include(t => t.tb_Menu);
            return View(tb_ProductCatagory.ToList());
        }

        // GET: Admin/ProductCategory/Details/5
        public ActionResult Details( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_ProductCatagory tb_ProductCatagory = db.tb_ProductCatagory.Find(id);
            if (tb_ProductCatagory == null)
            {
                return HttpNotFound();
            }
            return View(tb_ProductCatagory);
        }

        // GET: Admin/ProductCategory/Create
        public ActionResult Create()
        {
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title");
            return View();
        }

        // POST: Admin/ProductCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "Id,Name,Icon,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,Menu_Id")] tb_ProductCatagory tb_ProductCatagory )
        {
            if (ModelState.IsValid)
            {
                tb_ProductCatagory.CreatedDate = DateTime.Now.ToString();
                tb_ProductCatagory.ModifiedDate = DateTime.Now.ToString();
                db.tb_ProductCatagory.Add(tb_ProductCatagory);
                db.SaveChanges();
                TempData["message"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title", tb_ProductCatagory.Menu_Id);
            return View(tb_ProductCatagory);
        }

        // GET: Admin/ProductCategory/Edit/5
        public ActionResult Edit( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_ProductCatagory tb_ProductCatagory = db.tb_ProductCatagory.Find(id);
            if (tb_ProductCatagory == null)
            {
                return HttpNotFound();
            }
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title", tb_ProductCatagory.Menu_Id);
            return View(tb_ProductCatagory);
        }

        // POST: Admin/ProductCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind(Include = "Id,Name,Icon,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,Menu_Id")] tb_ProductCatagory tb_ProductCatagory )
        {
            if (ModelState.IsValid)
            {
                tb_ProductCatagory.ModifiedDate = DateTime.Now.ToString();
                db.Entry(tb_ProductCatagory).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Sửa thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title", tb_ProductCatagory.Menu_Id);
            return View(tb_ProductCatagory);
        }

        // GET: Admin/ProductCategory/Delete/5
        [HttpPost]
        public ActionResult Delete( int id )
        {
            tb_ProductCatagory tb_ProductCatagory = db.tb_ProductCatagory.Find(id);
            if (tb_ProductCatagory != null)
            {
                db.tb_ProductCatagory.Remove(tb_ProductCatagory);
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
