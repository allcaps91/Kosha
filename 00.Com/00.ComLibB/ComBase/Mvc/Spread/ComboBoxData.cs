using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComBase.Mvc.Spread
{
    public class ComboBoxItem
    {
        public ComboBoxItem() { }
        public ComboBoxItem(string code, string codeName)
        {
            Code = code;
            CodeName = codeName;
        }
        public string Code { get; set; }
        public string CodeName { get; set; }
    }

    public class ComboBoxData
    {
        //코드명
        private List<string> items = new List<string>();
        //코드
        private List<string> itemData = new List<string>();

        private List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem>();

        public void Put(string code, string codeName)
        {
            items.Add(codeName);
            itemData.Add(code);
            comboBoxItems.Add(new ComboBoxItem(code, codeName));
        }

        public string[] GetItems()
        {
            return items.ToArray();
        }
        public string[] GetItemData()
        {
            return itemData.ToArray();
        }
        public List<ComboBoxItem> GetComboBoxItems()
        {
            return this.comboBoxItems;
        }
        public BindingSource GetComboBoxDatasource()
        {
            return new BindingSource(comboBoxItems, null);
        }
        /// <summary>
        /// 콤보박스 데이타소스 바인딩
        /// </summary>
        /// <param name="comboBox"></param>
        public void SetComboBox(ComboBox comboBox)
        {
            comboBox.DataSource = GetComboBoxDatasource();
            comboBox.DisplayMember = "CodeName";
            comboBox.ValueMember = "Code";
        }
            
    }
}
