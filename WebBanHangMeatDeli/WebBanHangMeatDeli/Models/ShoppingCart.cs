using System.Collections.Generic;
using System.Linq;

namespace WebBanHangMeatDeli.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Item { get; set; }
        public ShoppingCart()
        {
            this.Item = new List<ShoppingCartItem>();
        }
        public void AddToCart( ShoppingCartItem item, int Quantity )
        {
            var checkProduct = Item.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkProduct != null)
            {
                checkProduct.ProductQuantity += Quantity;
                checkProduct.ProductPriceTotal = checkProduct.ProductPrice * checkProduct.ProductQuantity;
            }
            else
            {
                Item.Add(item);
            }
        }
        public void RemoveItem( int id )
        {
            var checkProduct = Item.SingleOrDefault(x => x.ProductId == id);
            if (checkProduct != null)
            {
                Item.Remove(checkProduct);
            }
        }
        public void UpdateQuantity( int id, int quantity )
        {
            var checkProduct = Item.SingleOrDefault(x => x.ProductId == id);
            if (checkProduct != null)
            {
                checkProduct.ProductQuantity = quantity;
                checkProduct.ProductPriceTotal = checkProduct.ProductPrice * checkProduct.ProductQuantity;
            }
        }
        public decimal GetPriceTotal()
        {
            return Item.Sum(x => x.ProductPriceTotal);
        }
        public decimal GetQuantityTotal()
        {
            return Item.Sum(x => x.ProductQuantity);
        }
        public void ClearCart()
        {
            Item.Clear();
        }
    }

    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductCateName { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string ProductImage { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductPriceTotal { get; set; }
    }
}