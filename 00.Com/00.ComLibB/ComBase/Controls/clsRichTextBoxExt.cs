using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsRichTextBoxExt
    {
        public static void SetValue(this RichTextBox richTextBox, object data)
        {
            
        }

        public static string GetValue(this RichTextBox richTextBox)
        {
            return richTextBox.Rtf;
        }

        public static void Initialize(this RichTextBox richTextBox)
        {
            if (richTextBox.DataBindings != null)
            {
                richTextBox.DataBindings.Clear();
            }
            richTextBox.Rtf = string.Empty;
            richTextBox.Text = string.Empty;
        }

        public static string GetDataField(this RichTextBox richTextBox)
        {
            if (richTextBox.Tag is RichTextBoxOption)
            {
                return (richTextBox.Tag as RichTextBoxOption).DataField;
            }

            return string.Empty;
        }
    }

    public class RichTextBoxOption : IControlOption
    {
        public string DataField { get; set; }
    }
}
