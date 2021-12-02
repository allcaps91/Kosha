using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsMaskedTextBoxExt
    {
        public static void SetValue(this MaskedTextBox maskedTextBox, object data)
        {
            maskedTextBox.Text = data.ToString();
        }

        public static string GetValue(this MaskedTextBox maskedTextBox)
        {
            return maskedTextBox.Text;
        }

        public static void Initialize(this MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.DataBindings != null)
            {
                maskedTextBox.DataBindings.Clear();
            }
            maskedTextBox.Text = string.Empty;
        }

        public static string GetDataField(this MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.Tag is MaskedTextBoxOption)
            {
                return (maskedTextBox.Tag as MaskedTextBoxOption).DataField;
            }

            return string.Empty;
        }

        public static void SetOptions(this MaskedTextBox maskedTextBox, MaskedTextBoxOption option)
        {
            string dataField = string.Empty;
            if (maskedTextBox.Tag != null)
            {
                if (maskedTextBox.Tag is string)
                {
                    dataField = maskedTextBox.Tag.ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(dataField))
            {
                option.DataField = dataField;
            }

            maskedTextBox.Tag = option;

            if (option.MaskType == MaskedTextType.휴대전화)
            {
                maskedTextBox.Mask = "000-9000-0000";
            }
            else if (option.MaskType == MaskedTextType.사업자번호)
            {
                maskedTextBox.Mask = "000-00-00000";
            }
            else if (option.MaskType == MaskedTextType.주민등록번호)
            {
                maskedTextBox.Mask = "000000-0000000";
            }
        }
    }

    public class MaskedTextBoxOption : IControlOption
    {
        public string DataField { get; set; }
        public MaskedTextType MaskType;
    }

    public enum MaskedTextType
    {
        None,
        휴대전화,
        사업자번호,
        주민등록번호
    }
}
