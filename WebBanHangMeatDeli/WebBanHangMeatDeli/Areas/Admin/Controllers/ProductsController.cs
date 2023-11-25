using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        // GET: Admin/Products
        public ActionResult Index( int? page, string SeachString )
        {
            ViewBag.ProductCate_Id = new SelectList(db.tb_ProductCatagory, "Id", "Name");
            var tb_Product = db.tb_Product.Include(t => t.tb_ProductCatagory);
            var pageSize = 7;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(SeachString))
            {
                tb_Product = tb_Product.Where(x => x.Name.ToUpper().Contains(SeachString));
                if (tb_Product.Count() > 0)
                {
                    pageSize = tb_Product.Count();
                }
                else
                {
                    TempData["warning"] = "Không thấy sản phẩm bạn muốn tìm!";
                    return View(tb_Product.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
                }
            }
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(tb_Product.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductCate_Id = new SelectList(db.tb_ProductCatagory, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "Id,Name,Description,Alias,Details,Image,Image1,Image2,Price,PriceSale,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,ProductCate_Id,IsSale,IsActive")] tb_Product tb_Product, HttpPostedFileBase imageAvt, HttpPostedFileBase image1, HttpPostedFileBase image2 )
        {
            if (ModelState.IsValid)
            {
                tb_Product.CreatedDate = DateTime.Now.ToString();
                tb_Product.ModifiedDate = DateTime.Now.ToString();
                tb_Product.Alias = Models.Convert.convert.FilterChar(tb_Product.Name);
                db.tb_Product.Add(tb_Product);
                db.SaveChanges();
                tb_Product.Image = addImage(tb_Product, imageAvt, "-", tb_Product.Image);
                tb_Product.Image1 = addImage(tb_Product, image1, "-1-", tb_Product.Image1);
                tb_Product.Image2 = addImage(tb_Product, image2, "-2-", tb_Product.Image2);
                db.SaveChanges();
                TempData["message"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.ProductCate_Id = new SelectList(db.tb_ProductCatagory, "Id", "Name", tb_Product.ProductCate_Id);
            return View(tb_Product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCate_Id = new SelectList(db.tb_ProductCatagory, "Id", "Name", tb_Product.ProductCate_Id);
            return View(tb_Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind(Include = "Id,Name,Description,Alias,Details,Image,Image1,Image2,Price,PriceSale,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,ProductCate_Id,IsSale,IsActive")] tb_Product tb_Product, HttpPostedFileBase imageAvt, HttpPostedFileBase image1, HttpPostedFileBase image2 )
        {
            if (ModelState.IsValid)
            {
                tb_Product.ModifiedDate = DateTime.Now.ToString();
                tb_Product.Alias = Models.Convert.convert.FilterChar(tb_Product.Name);
                tb_Product.Image = addImage(tb_Product, imageAvt, "-", tb_Product.Image);
                tb_Product.Image1 = addImage(tb_Product, image1, "-1-", tb_Product.Image1);
                tb_Product.Image2 = addImage(tb_Product, image2, "-2-", tb_Product.Image2);
                db.Entry(tb_Product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Sửa thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.ProductCate_Id = new SelectList(db.tb_ProductCatagory, "Id", "Name", tb_Product.ProductCate_Id);
            return View(tb_Product);
        }

        // GET: Admin/Products/Delete/5
        [HttpPost]
        public ActionResult Delete( int id )
        {
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product != null)
            {
                deleteFile(tb_Product.Image);
                deleteFile(tb_Product.Image1);
                deleteFile(tb_Product.Image2);
                db.tb_Product.Remove(tb_Product);
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
        public string addImage( tb_Product tb_Product, HttpPostedFileBase imageFile, string fileName, string attTable )
        {
            string nameImage = tb_Product.Alias;
            int id = tb_Product.Id;
            int index;
            string _FileNameImage = "";
            string _Path = "";
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                deleteFile(attTable);
                index = imageFile.FileName.IndexOf(".");
                _FileNameImage = nameImage + fileName + id.ToString() + "." + imageFile.FileName.Substring(index + 1);
                _Path = Path.Combine(Server.MapPath("~/Upload/Product"), _FileNameImage);
                imageFile.SaveAs(_Path);
                return _FileNameImage;
            }
            return attTable;
        }
        public void deleteFile( string fileImage )
        {
            if (fileImage != null && fileImage.Length > 0)
            {
                var filePath = Server.MapPath("~/Upload/Product/");
                DirectoryInfo folderInfo = new DirectoryInfo(filePath);
                foreach (FileInfo file in folderInfo.GetFiles(fileImage))
                {
                    file.Delete();
                }
            }
        }
    }
}
