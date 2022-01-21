using System;
using System.Linq;
using System.Web.UI;
using Vanct.Dal.Entities;

namespace Vanct.WebApp.UserControls
{
    public partial class ProductViewerControl : UserControl
    {
        public int ProductTypeId { set; get; }
        public int ProductTypeGroupId { set; get; }
        public int ExcludeProductId { set; get; }
        public bool ProductHome { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadProducts();
        }

        protected void LoadProducts()
        {
            using (var db = new VanctEntities())
            {
                if (ProductHome)
                {
                    repeater.DataSource = (from i in db.Products
                                           where i.IsActive && i.IsHomeShowed
                                           && (ExcludeProductId == 0 || i.Id != ExcludeProductId)
                                           orderby i.Position descending
                                           select new
                                                  {
                                                      i.Id,
                                                      i.Name,
                                                      i.PriceVnd,
                                                      i.ImageUrl,
                                                      i.IsHot,
                                                      i.IsSaleOff,
                                                      i.Note
                                                  })
                        .ToList();
                }
                else
                {
                    repeater.DataSource = (from i in db.Products
                                           where i.IsActive
                                                 && (ProductTypeId == 0 || i.ProductTypeId == ProductTypeId)
                                                 && (ProductTypeGroupId == 0 || i.ProductTypeGroupId == ProductTypeGroupId)
                                                 && (ExcludeProductId == 0 || i.Id != ExcludeProductId)
                                           orderby i.Position descending
                                           select new
                                                  {
                                                      i.Id,
                                                      i.Name,
                                                      i.PriceVnd,
                                                      i.ImageUrl,
                                                      i.IsHot,
                                                      i.IsSaleOff,
                                                      i.Note
                                                  })
                        .ToList();
                }
                repeater.DataBind();
            }

        }
    }
}