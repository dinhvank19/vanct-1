using System;
using System.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Pos
{
    public partial class CompanyUserList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var companyId = VanctContext.CompanyId;
            var company = VanctContext.PosCompanyDao.Get(i => i.Id == companyId);
            lblTitle.Text = company.Name;
            LoadData();
        }

        protected void BtnGenerate(object sender, EventArgs e)
        {
            var companyId = VanctContext.CompanyId;
            var record = new PosUser
            {
                CompanyId = companyId,
                CreatedDate = DateTime.Now,
                IsError = false,
                UniqueId = Guid.NewGuid().ToString()
            };
            VanctContext.PosUserDao.Insert(record);
            LoadData();
        }

        protected void LoadData()
        {
            var companyId = VanctContext.CompanyId;
            var list = VanctContext.PosUserDao.Gets(i => i.CompanyId == companyId);
            grid.DataSource = list;
            grid.DataBind();
        }
    }
}