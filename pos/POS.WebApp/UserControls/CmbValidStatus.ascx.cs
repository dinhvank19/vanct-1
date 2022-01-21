using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using POS.Dal.Enums;
using POS.WebApp.AppCode;
using Telerik.Web.UI;

namespace POS.WebApp.UserControls
{
    public partial class CmbValidStatus : UserControl
    {
        private int _index = -1;
        private object _value;

        public CmbValidStatus()
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
            combobox.DataSource = Enum.GetValues(typeof (ValidStatus))
                .Cast<ValidStatus>()
                .Select(v => new {Id = v.ToString(), Name = v.ToString().Translate()})
                .ToList();

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
                    Text = "Chọn"
                });
        }
    }
}