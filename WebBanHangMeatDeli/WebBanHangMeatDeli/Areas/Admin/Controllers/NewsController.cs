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
    public class NewsController : Controller
    {
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        // GET: Admin/News
        public ActionResult Index( int? page, string SeachString )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var tb_News = db.tb_News.Include(t => t.tb_Menu);
            var pageSize = 7;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(SeachString))
            {
                tb_News = tb_News.Where(x => x.Title.ToUpper().Contains(SeachString));
                if (tb_News.Count() > 0)
                {
                    pageSize = tb_News.Count();
                }
                else
                {
                    TempData["warning"] = "Không thấy tin tức bạn muốn tìm!";
                    return View(tb_News.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
                }
            }
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(tb_News.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "Id,Title,Alias,Menu_Id,Description,Details,SeoTitle,Image,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] tb_News tb_News, HttpPostedFileBase upLoadFile )
        {
            if (ModelState.IsValid)
            {
                tb_News.CreatedDate = DateTime.Now.ToString();
                tb_News.ModifiedDate = DateTime.Now.ToString();
                tb_News.Alias = Models.Convert.convert.FilterChar(tb_News.Title);
                db.tb_News.Add(tb_News);
                db.SaveChanges();
                addImage(tb_News, upLoadFile);
                db.SaveChanges();
                TempData["message"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title", tb_News.Menu_Id);
            return View(tb_News);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_News tb_News = db.tb_News.Find(id);
            if (tb_News == null)
            {
                return HttpNotFound();
            }
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title", tb_News.Menu_Id);
            return View(tb_News);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind(Include = "Id,Title,Alias,Menu_Id,Description,Details,SeoTitle,Image,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] tb_News tb_News, HttpPostedFileBase upLoadFile )
        {
            if (ModelState.IsValid)
            {
                tb_News.ModifiedDate = DateTime.Now.ToString();
                tb_News.Alias = Models.Convert.convert.FilterChar(tb_News.Title);
                addImage(tb_News, upLoadFile);
                db.Entry(tb_News).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Sửa thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.Menu_Id = new SelectList(db.tb_Menu, "Id", "Title", tb_News.Menu_Id);
            return View(tb_News);
        }

        // GET: Admin/News/Delete/5
        [HttpPost]
        public ActionResult Delete( int id )
        {
            tb_News tb_News = db.tb_News.Find(id);
            if (tb_News != null)
            {
                deleteFile(tb_News.Image);
                db.tb_News.Remove(tb_News);
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
        public void deleteFile( string fileImage )
        {
            if (fileImage != null && fileImage.Length > 0)
            {
                var filePath = Server.MapPath("~/Upload/News/");
                DirectoryInfo folderInfo = new DirectoryInfo(filePath);
                foreach (FileInfo file in folderInfo.GetFiles(fileImage))
                {
                    file.Delete();
                }
            }
        }
        public void addImage( tb_News tb_News, HttpPostedFileBase upLoadFile )
        {
            string nameImage = tb_News.Alias;
            int id = tb_News.Id;
            int index;
            string _FileNameImage = "";
            string _Path = "";
            if (upLoadFile != null && upLoadFile.ContentLength > 0)
            {
                deleteFile(tb_News.Image);
                tb_News.Image = "";
                index = upLoadFile.FileName.IndexOf(".");
                _FileNameImage = nameImage + "-" + id.ToString() + "." + upLoadFile.FileName.Substring(index + 1);
                _Path = Path.Combine(Server.MapPath("~/Upload/News"), _FileNameImage);
                upLoadFile.SaveAs(_Path);
                tb_News.Image = _FileNameImage;
            }
        }
    }
}
