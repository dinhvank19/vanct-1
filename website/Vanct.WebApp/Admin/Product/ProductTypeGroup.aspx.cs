using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Product
{
    public partial class ProductTypeGroup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)return;
            LoadTypeGroup();
        }

        #region Product Type Group

        protected void BtnSaveUpdateTypeGroupClick(object sender, EventArgs e)
        {
            var id = txtTypeGroupId.Value.ToInt32();
            var group = VanctContext.ProductTypeGroupDao.Get(i => i.Id == id);
            if(group==null) return;
            group.IsActive = ckIsActive.Checked;
            group.Name = txtName.Text;
            group.Position = txtPosition.Value != null ? (int) txtPosition.Value : 0;
            VanctContext.ProductTypeGroupDao.Edit(group, i => i.Id == id);
            LoadTypeGroup();
        }

        protected void BtnSaveInsertTypeGroupClick(object sender, EventArgs e)
        {
            VanctContext.ProductTypeGroupDao.Create(new Dal.Entities.ProductTypeGroup
                                                  {
                                                      IsActive = ckIsActive.Checked,
                                                      Name = txtName.Text,
                                                      Position = txtPosition.Value != null ? (int) txtPosition.Value : 0
                                                  });
            LoadTypeGroup();
        }

        protected void LoadTypeGroup(int id)
        {
            var group = VanctContext.ProductTypeGroupDao.Get(i => i.Id == id);
            if (group == null) return;
            txtTypeGroupId.Value = id.ToString(CultureInfo.InvariantCulture);
            ckIsActive.Checked = group.IsActive;
            txtName.Text = group.Name;
            txtPosition.Value = group.Position;
        }

        protected void LoadTypeGroup()
        {
            CmbProductTypeGroup.Reload();
            gridTypeGroup.DataSource = VanctContext.ProductTypeGroupDao
                .Gets()
                .OrderByDescending(i => i.Position);
            gridTypeGroup.DataBind();
        }

        protected void GridTypeGroupItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdEdit":
                    int id = e.CommandArgument.ToString().ToInt32();
                    LoadTypeGroup(id);
                    break;
            }
        }

        protected void GridTypeGroupItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item;
            if (item.ItemType != GridItemType.Item && item.ItemType != GridItemType.AlternatingItem) return;

            var groupData = (Dal.Entities.ProductTypeGroup) item.DataItem;
            var gridType = (RadGrid) item.FindControl("gridType");
            LoadTypeByGroupId(groupData.Id, gridType);
        }

        #endregion

        #region Product Type

        protected void LoadTypeByGroupId(int groupId, RadGrid radGrid)
        {
            radGrid.DataSource = VanctContext.ProductTypeDao
                .Gets(i => i.ProductTypeGroupId == groupId)
                .OrderByDescending(i => i.Position);
            radGrid.DataBind();
        }

        protected void GridTypeItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdEdit":
                    int id = e.CommandArgument.ToString().ToInt32();
                    LoadType(id);
                    break;

            }
        }

        protected void LoadType(int id)
        {
            var type = VanctContext.ProductTypeDao.Get(i => i.Id == id);
            if (type == null) return;
            txtTypeId.Value = id.ToString(CultureInfo.InvariantCulture);
            txtNameType.Text = type.Name;
            txtPositionType.Value = type.Position;
            ckIsActiveType.Checked = type.IsActive;
            ckIsHomeShowed.Checked = type.IsHomeShowed;
            CmbProductTypeGroup.ValueInt32 = type.ProductTypeGroupId;
        }

        protected void BtnSaveUpdateTypeClick(object sender, EventArgs e)
        {
            var id = txtTypeId.Value.ToInt32();
            var type = VanctContext.ProductTypeDao.Get(i => i.Id == id);
            if (type == null) return;
            type.Name = txtNameType.Text;
            type.Position = txtPositionType.Value != null ? (int) txtPositionType.Value : 0;
            type.IsActive = ckIsActiveType.Checked;
            type.IsHomeShowed = ckIsHomeShowed.Checked;
            type.ProductTypeGroupId = CmbProductTypeGroup.ValueInt32;
            VanctContext.ProductTypeDao.Edit(type, i => i.Id == id);
            LoadTypeGroup();
        }

        protected void BtnSaveInsertTypeClick(object sender, EventArgs e)
        {
            VanctContext.ProductTypeDao.Create(new Dal.Entities.ProductType
                                             {
                                                 Name = txtNameType.Text,
                                                 Position = txtPositionType.Value != null ? (int) txtPositionType.Value : 0,
                                                 IsHomeShowed = ckIsHomeShowed.Checked,
                                                 IsActive = ckIsActiveType.Checked,
                                                 ProductTypeGroupId = CmbProductTypeGroup.ValueInt32
                                             });
            LoadTypeGroup();
        }
        
        #endregion
    }
}