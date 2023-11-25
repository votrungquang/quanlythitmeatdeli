using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Controllers
{
    public class HomeController : Controller
    {
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        public ActionResult Index()
        {
            var items = db.tb_ProductCatagory.ToList();
            return View(items);
        }

        public ActionResult Search( string searchProduct, int? page )
        {
            var pageSize = 15;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(searchProduct))
            {
                var product = db.tb_Product.Where(x => x.Name.ToUpper().Contains(searchProduct));
                if (product.Count() > 0)
                {
                    ViewBag.count = product.Count();
                    return View(product.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize));
                }
                else
                {
                    ViewBag.error = "Không thấy sản phẩm bạn muốn tìm!";
                    return View();
                }
            }
            return View();
        }
    }
}