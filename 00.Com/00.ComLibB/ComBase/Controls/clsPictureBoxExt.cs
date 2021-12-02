using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsPictureBoxExt
    {
        public static void SetValue(this PictureBox pictureBox, object data)
        {

        }

        public static object GetValue(this PictureBox pictureBox)
        {
            return string.Empty;
        }

        public static void Clear(this PictureBox pictureBox)
        {
            pictureBox.Image = null;
        }
    }

    public class PictureBoxOption : IControlOption
    {
        public string DataField { get; set; }
    }
}
