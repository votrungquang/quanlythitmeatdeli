using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login( string email, string password )
        {
            if (string.IsNullOrEmpty(email) | string.IsNullOrEmpty(password))
            {
                ViewBag.error = "Hãy nhập đầy đủ thông tin!";
                return View();
            }
            var user = db.tb_Users.SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                ViewBag.error = "Tài khoản không tồn tại!";
                return View();
            }
            if (Crypto.VerifyHashedPassword(user.Password, password))
            {
                Session["auth"] = user;
                ViewBag.username = user.Name;
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    return RedirectToRoute("ShoppingCart", new { controller = "Cart", action = "Index" });
                }
                else
                    return RedirectToRoute("default", new { controller = "Home", action = "Index" });
            }
            ViewBag.error = "Mật khẩu không chính xác!";
            return View();

        }
        public ActionResult Logout()
        {
            Session.Remove("auth");
            FormsAuthentication.SignOut();
            return RedirectToRoute("default", new { controller = "Home", action = "Index" });
        }
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Register( string name, string email, string password, string address, string phone )
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(email) | string.IsNullOrEmpty(password) | string.IsNullOrEmpty(name) | string.IsNullOrEmpty(address) | string.IsNullOrEmpty(phone))
                {
                    ViewBag.error = "Hãy nhập đầy đủ thông tin!";
                    return View();
                }
                var item = db.tb_Users.SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
                if (item != null)
                {
                    ViewBag.error = "Tài khoản đã tồn tại";
                    return View();
                }

                tb_Users tb_Users = new tb_Users
                {
                    Name = name,
                    Email = email,
                    Password = Crypto.HashPassword(password),
                    Address = address,
                    Phone = phone,
                    UserType_ID = 2,
                    IsConfirm = true
                };
                db.Configuration.ValidateOnSaveEnabled = false;
                db.tb_Users.Add(tb_Users);
                db.SaveChanges();
                ViewBag.success = true;
            }
            return RedirectToRoute("AccountLogin", new { controller = "Account", action = "Login" });
        }
    }
}