using System.Linq;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CategoryHeader()
        {
            var items = db.tb_Menu.OrderBy(x => x.Position).ToList();
            return PartialView("_CategoryHeader", items);
        }
        public ActionResult CategoryFooter()
        {
            var items = db.tb_Menu.OrderBy(x => x.Position).ToList();
            return PartialView("_CategoryFooter", items);
        }
    }
}