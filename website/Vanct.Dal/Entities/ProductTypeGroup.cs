//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vanct.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductTypeGroup
    {
        public ProductTypeGroup()
        {
            this.Products = new HashSet<Product>();
            this.Products1 = new HashSet<Product>();
            this.ProductTypes = new HashSet<ProductType>();
            this.ProductTypes1 = new HashSet<ProductType>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Product> Products1 { get; set; }
        public virtual ICollection<ProductType> ProductTypes { get; set; }
        public virtual ICollection<ProductType> ProductTypes1 { get; set; }
    }
}