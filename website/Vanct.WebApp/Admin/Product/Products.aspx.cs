using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using Vanct.Dal.Entities;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Product
{
    public partial class Products : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            cmbType.ValueInt32 = Session["FindProductTypeID"] != null ? (int)Session["FindProductTypeID"] : 0;
            cmbGroup.ValueInt32 = Session["FindProductTypeGroupID"] != null ? (int)Session["FindProductTypeGroupID"] : 0;
            LoadProduct();
        }

        protected void BtnFindClick(object sender, EventArgs e)
        {
            Session["FindProductTypeID"] = cmbType.ValueInt32;
            Session["FindProductTypeGroupID"] = cmbGroup.ValueInt32;
            LoadProduct();
        }

        protected void LoadProduct()
        {
            using (var db = new VanctEntities())
            {
                var typeId = Session["FindProductTypeID"] != null ? (int)Session["FindProductTypeID"] : 0;
                var groupId = Session["FindProductTypeGroupID"] != null ? (int)Session["FindProductTypeGroupID"] : 0;
                int? valueNull = null;
                gridPro.DataSource = (from i in db.Products
                                      join typeGroup in db.ProductTypeGroups on i.ProductTypeGroupId equals typeGroup.Id
                                      where (i.ProductTypeId == valueNull || typeId == 0 || i.ProductTypeId == typeId)
                                            && (groupId == 0 || i.ProductTypeGroupId == groupId)
                                      select new
                                             {
                                                 i.Id,
                                                 i.Name,
                                                 i.PriceVnd,
                                                 TypeGroupName = typeGroup.Name,
                                                 TypeName = i.ProductType.Name,
                                                 Active = i.IsActive ? "Có" : "Không",
                                                 HomeShow = i.IsHomeShowed ? "Có" : "Không",
                                                 i.Warranty,
                                                 IsSaleOff = i.IsSaleOff ? "Có" : "Không",
                                                 IsHot = i.IsHot ? "Có" : "Không",
                                             })
                    .ToList();
                gridPro.DataBind();
            }
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdEdit":
                    Response.Redirect(string.Format("ProductUpdate.aspx?Id={0}", e.CommandArgument));
                    break;
                case "cmdDelete":
                    var id = e.CommandArgument.ToString().ToInt32();
                    var product = VanctContext.ProductDao.Get(i => i.Id == id);
                    Path.Combine(AppPath.ProductImagesFolder, product.ImageUrl).DeleteFile();
                    VanctContext.ProductDao.Delete(i => i.Id == id);
                    LoadProduct();
                    break;
            }
        }
    }
}