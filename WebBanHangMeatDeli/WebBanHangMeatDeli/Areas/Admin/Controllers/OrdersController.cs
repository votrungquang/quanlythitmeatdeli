using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebBanHangMeatDeli.Models;

namespace WebBanHangMeatDeli.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var tb_Orders = db.tb_Orders.Include(t => t.tb_Users);
            return View(tb_Orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Orders tb_Orders = db.tb_Orders.Find(id);
            if (tb_Orders == null)
            {
                return HttpNotFound();
            }
            return View(tb_Orders);
        }
        [HttpPost]
        public ActionResult Delete( int id )
        {
            // Lấy đối tượng Order có Id tương ứng.
            var order = db.tb_Orders.Find(id);
            if (order != null)
            {
                var orderDetails = db.tb_OrderDetails.Where(o => o.Orders_Id == id).ToList();
                foreach (var orderDetail in orderDetails)
                {
                    db.tb_OrderDetails.Remove(orderDetail);
                }
                db.tb_Orders.Remove(order);
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
