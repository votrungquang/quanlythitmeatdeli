using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();

            }
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login( string user, string password )
        {
            var admin = db.tb_Users.SingleOrDefault(x => x.Name.ToLower() == user.ToLower() && x.Password == password);
            if (admin != null)
            {
                Session["user"] = admin;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Tài khoản đăng nhập không chính xác!";
                return View();
            }
        }
        public ActionResult Logout( string user, string password )
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }

    }
}