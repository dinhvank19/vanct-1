using System;
using System.Linq;
using System.Web.UI;
using Hulk.Shared;
using Namviet.Data;
using Namviet.Entites;
using Namviet.Helpers;

namespace Namviet.Baocao
{
    public partial class Default : Page
    {
        public bool Deleteable => SessionHelper.LoginUser.Username == "admin";
        public bool ChangePasswordable => SessionHelper.LoginUser.Username == "admin";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            BindingParams();
            BindingDoanhThu();
        }

        private DateTime BeginningOfToday()
        {
            return DateTime.Today;
        }

        private DateTime BeginningOfToday(string anotherDate)
        {
            return anotherDate.ToDate();
        }

        private DateTime EndOfToday()
        {
            return DateTime.Today.AddDays(1).AddTicks(-1);
        }

        private DateTime EndOfToday(string anotherDate)
        {
            return anotherDate.ToDate().AddDays(1).AddTicks(-1);
        }

        protected void BindingDoanhThu()
        {
            using (var db = new NamvietEntities())
            {
                var fromDate = string.IsNullOrEmpty(txtFromDate.Text)
                    ? BeginningOfToday()
                    : BeginningOfToday(txtFromDate.Text);

                var toDate = string.IsNullOrEmpty(txtToDate.Text)
                    ? EndOfToday()
                    : EndOfToday(txtToDate.Text);

                var list = db.DoanhThus.Where(i => i.CreatedAt != null
                                                   && i.CreatedAt.Value.CompareTo(fromDate) >= 0
                                                   && i.CreatedAt.Value.CompareTo(toDate) < 0)
                    .Select(i => new DoanhThuRecord
                    {
                        CreatedAt = i.CreatedAt,
                        Amount = i.Amount,
                        Name = i.Name,
                        ExternalId = i.ExternalId,
                        Code = i.Code,
                        Price = i.Price,
                        Id = i.Id
                    })
                    .ToList();

                decimal total = 0;
                if (list.Any()) total = list.DefaultIfEmpty().Sum(i => i.Total);
                lblTotalReport.Text = ViewHelper.DisplayMoney(total);
                gridDoanhThu.DataSource = list;
                gridDoanhThu.DataBind();
            }
        }

        protected void BindingParams()
        {
            txtFromDate.Text = DateTime.Today.Display();
            txtToDate.Text = DateTime.Today.Display();
        }

        protected void BtnXemClick(object sender, EventArgs e)
        {
            BindingDoanhThu();
        }

        protected void BtnDeleteOnClick(object sender, EventArgs e)
        {
            using (var db = new NamvietEntities())
            {
                var id = txtDeletingRecordId.Text.ToInt32();
                var record = db.DoanhThus.Single(i => i.Id == id);
                db.DoanhThus.Remove(record);
                db.SaveChanges();
            }

            BindingDoanhThu();
        }

        protected void RadButton2Click(object sender, EventArgs e)
        {
            SessionHelper.LoginUser = null;
            Response.Redirect("~/Baocao/Login.aspx");
        }
    }
}