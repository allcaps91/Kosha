using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsRadioButtonExt
    {
        public static void SetValue(this RadioButton radioButton, object data)
        {
            if (radioButton.Parent == null || data == null || string.IsNullOrWhiteSpace(data.ToString()))
            {
                return;
            }

            foreach (Control control in radioButton.Parent.Controls)
            {
                if (control is RadioButton)
                {
                    
                    if (control.Tag is RadioButtonOption)
                    {
                        if((control.Tag as RadioButtonOption).CheckValue.Equals(data))
                        {
                            (control as RadioButton).Checked = true;

                            return;
                        }
                        else if(data.GetType().IsEnum)
                        {
                            if ((control.Tag as RadioButtonOption).CheckValue.Equals(data.ToString()))
                            {
                                (control as RadioButton).Checked = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static string GetValue(this RadioButton radioButton)
        {
            if(radioButton.Parent == null)
            {
                return string.Empty;
            }

            string dataField = string.Empty;
            if (radioButton.Tag != null)
            {
                if(radioButton.Tag is IControlOption)
                {
                    dataField = (radioButton.Tag as IControlOption).DataField;
                }
            }

            foreach(Control control in radioButton.Parent.Controls)
            {
                if(control is RadioButton)
                {
                    string field = string.Empty;
                    if (control.Tag is IControlOption)
                    {
                        field = (radioButton.Tag as IControlOption).DataField;
                    }

                    if(string.IsNullOrWhiteSpace(dataField))
                    {
                        if((control as RadioButton).Checked)
                        {
                            return (control.Tag as RadioButtonOption).CheckValue;
                        }
                    }
                    else
                    {
                        if (dataField.Equals(field) && (control as RadioButton).Checked)
                        {
                            return (control.Tag as RadioButtonOption).CheckValue;
                        }
                    }
                }
            }

            return string.Empty;
        }

        public static void Initialize(this RadioButton radioButton)
        {
            if (radioButton.DataBindings != null)
            {
                radioButton.DataBindings.Clear();
            }
            radioButton.Checked = false;
        }

        public static void SetOptions(this RadioButton radioButton, RadioButtonOption option)
        {
            string dataField = string.Empty;
            if (radioButton.Tag != null)
            {
                if (radioButton.Tag is string)
                {
                    dataField = radioButton.Tag.ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(dataField))
            {
                option.DataField = dataField;
            }

            radioButton.Tag = option;
        }

        public static string GetDataField(this RadioButton radioButton)
        {
            if (radioButton.Tag is RadioButton)
            {
                return (radioButton.Tag as RadioButtonOption).DataField;
            }

            return string.Empty;
        }

        public static RadioButtonOption GetOption(this RadioButton radioButton)
        {
            if (radioButton.Tag is RadioButtonOption)
            {
                return radioButton.Tag as RadioButtonOption;
            }

            return null;
        }
    }

    public class RadioButtonOption : IControlOption
    {
        public string DataField { get; set; }
        public string CheckValue;
        public string UnCheckValue;
    }
}
