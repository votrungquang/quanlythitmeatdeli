//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBanHangMeatDeli.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_Product()
        {
            this.tb_OrderDetails = new HashSet<tb_OrderDetails>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public Nullable<decimal> PriceSale { get; set; }
        public int Quantity { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> ProductCate_Id { get; set; }
        public bool IsSale { get; set; }
        public bool IsActive { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_OrderDetails> tb_OrderDetails { get; set; }
        public virtual tb_ProductCatagory tb_ProductCatagory { get; set; }
    }
}
