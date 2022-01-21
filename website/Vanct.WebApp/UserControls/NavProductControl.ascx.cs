using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.UserControls
{
    public partial class NavProductControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadTypeGroup();
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

        protected void SubRepeaterItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) return;
            if (item.ItemIndex != _lastTypeIndex) return;
            var lblDivider = (Literal)item.FindControl("lblDivider");
            lblDivider.Visible = false;
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
            var data = (ProductTypeGroup) item.DataItem;
            var subRepeater = (Repeater) item.FindControl("subRepeater");
            LoadTypeByGroupId(data.Id, subRepeater);

            if (item.ItemIndex != _lastTypeGroupIndex) return;
            var lblDivider = (Literal) item.FindControl("lblDivider");
            lblDivider.Visible = false;
        }

        #endregion
    }
}