using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsCheckBoxExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkBox"></param>
        /// <param name="data"></param>
        /// <History>2019.07.08 김민철   data값이 Null 이면 CheckStatus 는 False </History> 
        public static void SetValue(this CheckBox checkBox, object data)
        {
            if (checkBox.Tag is IControlOption)
            {   
                if (data == null)
                {
                    checkBox.Checked = false;
                }
                else
                {
                    checkBox.Checked = (checkBox.Tag as CheckBoxOption).CheckValue.Equals(data.ToString());
                }
            }
        }

        public static string GetValue(this CheckBox checkBox)
        {
            if(checkBox.Tag is IControlOption)
            {
                return checkBox.Checked ? (checkBox.Tag as CheckBoxOption).CheckValue : (checkBox.Tag as CheckBoxOption).UnCheckValue;
            }
            return string.Empty;
        }

        public static void Initialize(this CheckBox checkBox)
        {
            if(checkBox.DataBindings != null)
            {
                checkBox.DataBindings.Clear();
            }
            checkBox.Checked = false;
        }

        public static void SetOptions(this CheckBox checkBox, CheckBoxOption option)
        {
            string dataField = string.Empty;
            if(checkBox.Tag != null)
            {
                if(checkBox.Tag is string)
                {
                    dataField = checkBox.Tag.ToString();
                }
            }

            if(!string.IsNullOrWhiteSpace(dataField))
            {
                option.DataField = dataField;
            }

            checkBox.Tag = option;
        }

        public static string GetDataField(this CheckBox checkBox)
        {
            if (checkBox.Tag is CheckBoxOption)
            {
                return (checkBox.Tag as CheckBoxOption).DataField;
            }
            return string.Empty;
        }

        public static CheckBoxOption GetOption(this CheckBox checkBox)
        {
            if (checkBox.Tag is CheckBoxOption)
            {
                return checkBox.Tag as CheckBoxOption;
            }

            return null;
        }
    }

    public class CheckBoxOption : IControlOption
    {
        public string DataField { get; set; }
        public string CheckValue;
        public string UnCheckValue;
    }
}
