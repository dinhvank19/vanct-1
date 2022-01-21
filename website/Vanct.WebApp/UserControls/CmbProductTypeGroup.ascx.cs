using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using Vanct.Dal.Entities;
using Telerik.Web.UI;

namespace Vanct.WebApp.UserControls
{
    public partial class CmbProductTypeGroup : UserControl
    {
        private int _index = -1;
        private Object _value;

        public CmbProductTypeGroup()
        {
            EmptyFirstRow = false;
        }

        public string Value
        {
            get { return combobox.SelectedValue; }
            set
            {
                _value = value;
                combobox.SelectedIndex =
                    combobox.Items.IndexOf
                        (combobox.Items.FindItemByValue(value));
            }
        }

        public int ValueInt32
        {
            get { return int.Parse(combobox.SelectedValue); }
            set
            {
                _value = value;
                combobox.SelectedIndex =
                    combobox.Items.IndexOf
                        (combobox.Items.FindItemByValue(value.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public int Index
        {
            get { return combobox.SelectedIndex; }
            set
            {
                _index = value;
                combobox.SelectedIndex = value;
            }
        }

        public bool EmptyFirstRow { get; set; }
        public int Width { get; set; }
        public string ProductTypeControlName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Reload();

                if (Width > 0) combobox.Width = Width;
            }
        }

        public void Reload()
        {
            using (var db = new VanctEntities())
            {
                combobox.DataSource = (from i in db.ProductTypeGroups
                                       where i.IsActive
                                       orderby i.Position descending
                                       select new
                                              {
                                                  i.Id,
                                                  i.Name
                                              }).ToList();
                combobox.DataTextField = "Name";
                combobox.DataValueField = "Id";
                combobox.DataBind();
                if (_value != null)
                    combobox.SelectedIndex =
                        combobox.Items.IndexOf
                            (combobox.Items.FindItemByValue(_value.ToString()));
                if (_index != -1)
                    combobox.SelectedIndex = _index;

                if (EmptyFirstRow)
                    combobox.Items.Insert(0, new RadComboBoxItem
                                             {
                                                 Value = "0",
                                                 Text = "Chọn nhóm sản phẩm"
                                             });
            }
        }

        protected void ComboboxSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProductTypeControlName)) return;
            var cmbType = Parent.FindControl(ProductTypeControlName) as CmbProductType;
            if(cmbType==null) return;
            cmbType.TypeGroupId = ValueInt32;
            cmbType.Reload();
        }
    }
}