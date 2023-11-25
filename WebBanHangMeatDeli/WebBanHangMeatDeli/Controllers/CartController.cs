using System;
using System.Linq;
using System.Web.Mvc;
using WebBanHangMeatDeli.Commons;
using WebBanHangMeatDeli.Models;
namespace WebBanHangMeatDeli.Controllers
{

    public class CartController : Controller
    {
        // GET: Cart
        private MeatDeliSaiGonEntities db = new MeatDeliSaiGonEntities();
        public class Item
        {
            public int id { get; set; }
            public int quantity { get; set; }
        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult LoadCart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView("_LoadCart", cart.Item);
            }
            return PartialView();
        }
        [HttpPost]
        public ActionResult Delete( int id )
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.RemoveItem(id);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult Update( Item[] item )
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                for (int i = 0; i < cart.Item.Count; i++)
                {
                    cart.UpdateQuantity(item[i].id, item[i].quantity);
                    Session["Cart"] = cart;
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public ActionResult Count()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { count = cart.Item.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart( int id, int quantity )
        {
            var code = new { success = false, message = "", code = -1, count = 0 };
            var checkProduct = db.tb_Product.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Name,
                    ProductCateName = checkProduct.tb_ProductCatagory.Name,
                    ProductImage = checkProduct.Image,
                    ProductQuantity = quantity,
                    Alias = checkProduct.Alias

                };
                item.ProductPrice = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.ProductPrice = (decimal)checkProduct.PriceSale;
                }
                item.ProductPriceTotal = item.ProductQuantity * item.ProductPrice;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { success = true, message = "Sản phẩm đã thêm vào giỏ hàng", code = 1, count = cart.Item.Count };

            }
            return Json(code);
        }
        //[HttpGet]
        public ActionResult Checkout()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return View(cart.Item);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment( string Name, string phone, string email, string address )
        {
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                var user = (WebBanHangMeatDeli.Models.tb_Users)Session["auth"];
                if (cart != null)
                {
                    tb_Orders order = new tb_Orders();
                    order.CreatedDate = DateTime.Now.ToString();
                    order.Address = address;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    order.NameOrder = Name;
                    order.Email = email;
                    order.CreatedBy = phone;
                    order.Phone = phone;
                    order.User_Id = user.Id;
                    order.TotalAmount = cart.Item.Sum(x => (x.ProductPrice * x.ProductQuantity));
                    order.Quantity = cart.Item.Count();
                    cart.Item.ForEach(x => order.tb_OrderDetails.Add(new tb_OrderDetails
                    {
                        Product_Id = x.ProductId,
                        Quantity = x.ProductQuantity,
                        Price = x.ProductPrice,
                        Orders_Id = order.Id
                    }));
                    db.tb_Orders.Add(order);
                    db.SaveChanges();
                    //send mail cho khachs hang
                    var strSanPham = "";
                    decimal thanhtien = 0;
                    decimal TongTien = 0;
                    foreach (var sp in cart.Item)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.ProductQuantity + "</td>";
                        strSanPham += "<td>" + Common.FormatNumber(sp.ProductPriceTotal, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.ProductPrice * sp.ProductQuantity;
                    }
                    TongTien = thanhtien + 30000;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/SendUser.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate);
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.NameOrder);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Common.FormatNumber(thanhtien, 0).ToString());
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", Common.FormatNumber(TongTien, 0).ToString());
                    Common.SendMail(email, "Đơn hàng #" + order.Code, contentCustomer);
                    cart.ClearCart();
                }
            }
            return View();
        }
    }

}