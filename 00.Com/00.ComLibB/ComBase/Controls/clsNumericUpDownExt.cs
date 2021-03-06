using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsNumericUpDownExt
    {
        public static void SetValue(this NumericUpDown numericUpDown, object data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.ToString()))
            {
                numericUpDown.Initialize();
            }
            else
            {
                numericUpDown.Value = 0;
            
                decimal xxx = decimal.Parse(data.ToString());
                numericUpDown.Value =  decimal.Parse(data.ToString());
            }
        }

        public static object GetValue(this NumericUpDown numericUpDown)
        {
            if(string.IsNullOrWhiteSpace(numericUpDown.Controls[1].Text))
            {
                return null;
            }
            else
            {
                return numericUpDown.Value;
            }
        }

        public static void Initialize(this NumericUpDown numericUpDown)
        {
            if(numericUpDown.Minimum == decimal.MinValue)
            {
                numericUpDown.Controls[1].Text = string.Empty;
            }
            else
            {
                numericUpDown.Value = numericUpDown.Minimum;
            }
        }

        public static void SetOptions(this NumericUpDown numericUpDown, NumericUpDownOption option)
        {
            string dataField = string.Empty;
            if (numericUpDown.Tag is string)
            {
                dataField = numericUpDown.Tag.ToString();
            }

            if (!string.IsNullOrWhiteSpace(dataField))
            {
                option.DataField = dataField;
            }
            if (dataField.IsNullOrEmpty())
            {
                dataField = option.DataField;
            }

            numericUpDown.Maximum = option.Max;
            numericUpDown.Minimum = option.Min;
            numericUpDown.Tag = option;
            numericUpDown.TextAlign = option.TextAlign;
        }

        public static string GetDataField(this NumericUpDown numericUpDown)
        {
            if (numericUpDown.Tag is NumericUpDownOption)
            {
                return (numericUpDown.Tag as DateTimePickerOption).DataField;
            }

            return string.Empty;
        }

        public static void HideUpDownButton(this NumericUpDown numericUpDown)
        {
            numericUpDown.Controls.RemoveAt(0);
        }

        public static NumericUpDownOption GetOption(this NumericUpDown numericUpDown)
        {
            if (numericUpDown.Tag is NumericUpDownOption)
            {
                return numericUpDown.Tag as NumericUpDownOption;
            }

            return null;
        }
    }

    public class NumericUpDownOption : IControlOption
    {
        public string DataField { get; set; }
        public decimal Min = decimal.MinValue;
        public decimal Max = decimal.MaxValue;
        public HorizontalAlignment TextAlign = HorizontalAlignment.Center;
    }
}
