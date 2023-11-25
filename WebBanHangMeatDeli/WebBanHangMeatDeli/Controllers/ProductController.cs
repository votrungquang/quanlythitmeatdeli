using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Controllers
{

    public class ProductController : Controller
    {
        // GET: Product
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();
        public ActionResult Index( int? page )
        {
            var pageSize = 6;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (page == null)
            {
                page = 1;
            }
            var items = db.tb_Product.ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
        }
        public ActionResult Show( int? page, string name )
        {
            var pageSize = 6;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (page == null)
            {
                page = 1;
            }
            var items = db.tb_Product.Where(x => x.tb_ProductCatagory.Name == name);
            ViewBag.CategoryName = name;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
        }
        public ActionResult Detail( string alias, int id )
        {
            var items = db.tb_Product.Find(id);
            return View(items);
        }
    }
}