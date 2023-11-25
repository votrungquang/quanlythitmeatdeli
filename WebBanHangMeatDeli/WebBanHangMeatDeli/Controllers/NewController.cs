using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;
namespace WebBanHangMeatDeli.Controllers
{
    public class NewController : Controller
    {
        // GET: New
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();
        public ActionResult Index( int? page )
        {
            var pageSize = 6;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (page == null)
            {
                page = 1;
            }
            var items = db.tb_News.ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.OrderByDescending(x => x.CreatedDate).ToPagedList(pageIndex, pageSize));
        }
    }
}