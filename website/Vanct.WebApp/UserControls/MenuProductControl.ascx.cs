using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.UserControls
{
    public partial class MenuProductControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadTypeGroup();
            LoadGiaiPhap();
            LoadTinTuc();
            LoadKhachHang();
            LoadSupportOnline();
        }

        #region Type

        private int _lastTypeIndex;

        protected void LoadTypeByGroupId(int groupId, Repeater subRepeater)
        {
            var list = VanctContext.ProductTypeDao
                .Gets(i => i.ProductTypeGroupId == groupId && i.IsActive)
                .OrderByDescending(i => i.Position)
                .ToList();
            _lastTypeIndex = list.Count - 1;
            subRepeater.DataSource = list;
            subRepeater.DataBind();
        }

        #endregion

        #region Type Group

        private int _lastTypeGroupIndex;

        protected void LoadTypeGroup()
        {
            List<ProductTypeGroup> list = VanctContext.ProductTypeGroupDao.Gets(i => i.IsActive)
                .OrderByDescending(i => i.Position)
                .ToList();
            _lastTypeGroupIndex = list.Count - 1;
            repeater.DataSource = list;
            repeater.DataBind();
        }

        protected void RepeaterItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) return;
            var data = (ProductTypeGroup)item.DataItem;
            var subRepeater = (Repeater)item.FindControl("subRepeater");
            LoadTypeByGroupId(data.Id, subRepeater);
        }

        #endregion

        #region PostLink type=giai-phap

        protected void LoadGiaiPhap()
        {
            using (var db = new VanctEntities())
            {
                repeaterGiaiPhap.DataSource = (from i in db.PostLinks
                    where i.IsActive && i.PostLinkType.Equals("giai-phap")
                    orderby i.Position descending
                    select new
                    {
                        i.Id,
                        i.Name,
                        i.ImageUrl,
                    })
                    .ToList();
                repeaterGiaiPhap.DataBind();
            }
        }
        
        #endregion

        #region PostLink type=tin-tuc

        protected void LoadTinTuc()
        {
            using (var db = new VanctEntities())
            {
                repeaterTinTuc.DataSource = (from i in db.PostLinks
                    where i.IsActive && i.PostLinkType.Equals("tin-tuc")
                    orderby i.Position descending
                    select new
                    {
                        i.Id,
                        i.Name,
                        i.ImageUrl,
                    })
                    .Take(10)
                    .ToList();
                repeaterTinTuc.DataBind();
            }
        }

        #endregion

        #region PostLink type=khach-hang

        protected void LoadKhachHang()
        {
            using (var db = new VanctEntities())
            {
                logoRepeater.DataSource = (from i in db.PostLinks
                    where i.IsActive && i.PostLinkType.Equals("khach-hang")
                    orderby i.Position descending
                    select new
                    {
                        i.Id,
                        i.Name,
                        i.ImageUrl,
                        Note = i.SmallOverviewContent
                    })
                    .ToList();
                logoRepeater.DataBind();
            }
        }

        #endregion

        #region Support online

        protected void LoadSupportOnline()
        {
            repeaterSupportOnline.DataSource = VanctContext.SupportOnlineDao
                .Gets()
                .ToList();
            repeaterSupportOnline.DataBind();
        }

        #endregion
    }
}