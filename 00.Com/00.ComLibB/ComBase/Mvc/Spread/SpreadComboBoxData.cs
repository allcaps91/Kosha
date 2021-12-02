using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComBase.Mvc.Spread
{
    /// <summary>
    /// 스프레드에서 사용하는 콤보박스 아이템
    /// </summary>
    public class SpreadComboBoxItem
    {
        public SpreadComboBoxItem() { }
        public SpreadComboBoxItem(string code, string codeName)
        {
            Code = code;
            CodeName = codeName;
        }
        public string Code { get; set; }
        public string CodeName { get; set; }
    }

    /// <summary>
    /// 스프레드에서 사용하는 콤보데이타
    /// </summary>
    public class SpreadComboBoxData
    {
        //코드명
        private List<string> items = new List<string>();
        //코드
        private List<string> itemData = new List<string>();

        private List<SpreadComboBoxItem> comboBoxItems = new List<SpreadComboBoxItem>();

        public SpreadComboBoxData()
        {

        }
        /// <summary>
        /// 스프레드 콤보박스 List 설정
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="displayMember"> 콤보박스 이름</param>
        /// <param name="valueMember">콤보박스 값</param>
        public void SetItems<T>(List<T> list, string displayMember, string valueMember) where T : BaseDto, new()
        {
            List<T> tList = new List<T>();
            tList = list.ToList();
            foreach (var item in tList)
            {
                object display = item.GetPropertieValue(displayMember);
                object value = item.GetPropertieValue(valueMember);
                if(value == null)
                {
                    value = "";
                }
                if (display == null)
                {
                    display = "";
                }
                items.Add(display.ToString());
                itemData.Add(value.ToString());
                comboBoxItems.Add(new SpreadComboBoxItem(value.ToString(), display.ToString()));
            }

        }
        /// <summary>
        /// 콤보 데이타 넣기
        /// </summary>
        /// <param name="code">콥보박스 값</param>
        /// <param name="codeName">콤보박스 이름</param>
        public void Put(string code, string codeName)
        {
            items.Add(codeName);
            itemData.Add(code);
            comboBoxItems.Add(new SpreadComboBoxItem(code, codeName));
        }

        public string[] GetItems()
        {
            return items.ToArray();
        }
        public string[] GetItemData()
        {
            return itemData.ToArray();
        }
        public List<SpreadComboBoxItem> GetComboBoxItems()
        {
            return this.comboBoxItems;
        }
        public BindingSource GetComboBoxDatasource()
        {
            return new BindingSource(comboBoxItems, null);
        }
        /// <summary>
        /// 스프레드 콤보박스 데이타소스 바인딩
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
