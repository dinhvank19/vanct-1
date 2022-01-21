using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Postlinks
{
    public partial class PostLinkTypes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        protected void LoadData()
        {
            using (var db = new VanctEntities())
            {
                grid.DataSource = (from i in db.PostLinkTypes
                    orderby i.Position descending
                    select new
                    {
                        i.Id,
                        i.Name,
                        i.Position,
                        HomeShowed = i.IsHomeShowed ? "Có" : "Không"
                    }).ToList();
                grid.DataBind();
            }
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            var cmd = e.CommandName;
            switch (cmd)
            {
                case "cmEdit":
                    var id = e.CommandArgument.ToString();
                    var type = VanctContext.PostLinkDao.GetTypeById(id);
                    txtName.Text = type.Name;
                    txtPosition.Value = type.Position;
                    ckIsHomeShowed.Checked = type.IsHomeShowed;
                    txtId.Value = type.Id;
                    break;
            }
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            var type = new PostLinkType
            {
                Id = txtId.Value,
                Name = txtName.Text,
                IsHomeShowed = ckIsHomeShowed.Checked,
                Position = txtPosition.Value != null ? (int) txtPosition.Value : 0
            };
            VanctContext.PostLinkDao.UpdateType(type);
            LoadData();
        }
    }
}