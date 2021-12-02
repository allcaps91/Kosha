using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ComEmrBase
{
    /// <summary>
    /// 디자인 관련 함수 모음 : 속성값
    /// </summary>
    public class FormFunc
    {
        /// <summary>
        /// 컨트롤에 포함된 모든 컨트롤를 반환한다. GetAllControlsUsingRecursive와 동일
        /// </summary>
        /// <param name="containerControl"></param>
        /// <returns></returns>
        public static Control[] GetAllControls(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);

            while (queue.Count > 0)
            {
                Control.ControlCollection controls
                            = (Control.ControlCollection)queue.Dequeue();
                if (controls == null || controls.Count == 0) continue;

                foreach (Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }

            return allControls.ToArray();
        }

        /// <summary>
        /// 컨트롤 미비를 체크한다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="mForm"></param>
        /// <returns></returns>
        public static bool CheckFormMibi(string strFormNo, string strUpdateNo, Form mForm)
        {
            bool rtnVal = true;


            Color InitColor = Color.White;
            Color MibiColor = Color.Red;

            //폼별로도 하드 코팅을 해야 할 수 있음.

            //본래 색으로 돌린다
            SetMibiBackColor(mForm, "panel2", MibiColor);

            MessageBox.Show(new Form() { TopMost = true }, "미비 항목이 있습니다. 작성후 다시 저장하십시요.");
            return rtnVal;
        }

        /// <summary>
        /// 컨트롤 미비가 있을 경우 색칠한다
        /// </summary>
        /// <param name="mForm"></param>
        /// <param name="strConName"></param>
        /// <param name="pColor"></param>
        private static void SetMibiBackColor(Form mForm, string strConName, Color pColor)
        {
            Control[] tx = null;
            tx = mForm.Controls.Find(strConName, true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    if (tx[0] is Panel)
                    {
                        ((Panel)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is mtsPanel15.mPanel)
                    {
                        ((mtsPanel15.mPanel)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is GroupBox)
                    {
                        ((GroupBox)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is TextBox)
                    {
                        ((TextBox)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is CheckBox)
                    {
                        ((CheckBox)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is ComboBox)
                    {
                        ((ComboBox)tx[0]).BackColor = pColor;
                    }
                }
            }
        }

        #region //컨트롤 속성 => 문자열

        /// <summary>
        /// 이미지를 바이트로
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] imageToByteArray(Bitmap bmp)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (byte[])converter.ConvertTo(bmp, typeof(byte[]));
        }

        /// <summary>
        /// 폰트를 문자열로
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static string FontToString(Font font)
        {
            return font.FontFamily.Name + ":" + font.Size + ":" + (int)font.Style;
        }

        #endregion //컨트롤 속성 => 문자열

        #region //문자열 => 컨트롤 속성

        /// <summary>
        /// 문자열 => True/False
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool GetTrueFalse(string strValue)
        {
            bool rtnVal = false;

            if (strValue == null)
            {
                return rtnVal;
            }
            if (strValue.ToUpper() == "TRUE")
            {
                rtnVal = true;
            }
            return rtnVal;
        }

        /// <summary>
        /// 문자열 => Font
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Font StringToFont(string font)
        {
            Font loadedFont = new Font("굴림체", 9, FontStyle.Regular);
            if (font == null)
            {
                return loadedFont;
            }
            string[] parts = font.Split(':');
            if (parts.Length != 3)
            {
                return loadedFont;
            }
            loadedFont = new Font(parts[0], float.Parse(parts[1]), (FontStyle)int.Parse(parts[2]));
            return loadedFont;
        }

        /// <summary>
        /// 문자열 => DockStyle
        /// </summary>
        /// <param name="strStyle"></param>
        /// <returns></returns>
        public static DockStyle GetDockStyle(string strStyle)
        {
            DockStyle rtnVal = DockStyle.None;
            if (strStyle == null)
            {
                return rtnVal;
            }
            switch (strStyle)
            {
                case "Left":
                    rtnVal = DockStyle.Left;
                    break;
                case "Top":
                    rtnVal = DockStyle.Top;
                    break;
                case "Right":
                    rtnVal = DockStyle.Right;
                    break;
                case "Fill":
                    rtnVal = DockStyle.Fill;
                    break;
                case "Bottom":
                    rtnVal = DockStyle.Bottom;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 문자열 => BorderStyle
        /// </summary>
        /// <param name="strStyle"></param>
        /// <returns></returns>
        public static BorderStyle GetBorderStyle(string strStyle)
        {
            BorderStyle rtnVal = BorderStyle.None;
            if (strStyle == null)
            {
                return rtnVal;
            }
            switch (strStyle)
            {
                case "Fixed3D":
                    rtnVal = BorderStyle.Fixed3D;
                    break;
                case "FixedSingle":
                    rtnVal = BorderStyle.FixedSingle;
                    break;
            }
            return rtnVal;
        }

        /// <summary>
        /// 문자열 => HorizontalAlignment
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static HorizontalAlignment GetControlAlign(string strValue)
        {
            HorizontalAlignment rtnVal = System.Windows.Forms.HorizontalAlignment.Left;
            if (strValue == null)
            {
                return rtnVal;
            }
            switch (strValue)
            {
                case "Left":
                    rtnVal = System.Windows.Forms.HorizontalAlignment.Left;
                    break;
                case "Right":
                    rtnVal = System.Windows.Forms.HorizontalAlignment.Right;
                    break;
                case "Center":
                    rtnVal = System.Windows.Forms.HorizontalAlignment.Center;
                    break;
                default:
                    rtnVal = System.Windows.Forms.HorizontalAlignment.Left;
                    break;
            }
            return rtnVal;
        }

        /// <summary>
        /// 문자열 => ContentAlignment
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static ContentAlignment GetContentAlign(string strValue)
        {
            ContentAlignment rtnVal = ContentAlignment.MiddleLeft;
            if (strValue == null)
            {
                return rtnVal;
            }
            switch (strValue)
            {
                case "TopLeft":
                    rtnVal = ContentAlignment.TopLeft;
                    break;
                case "TopCenter":
                    rtnVal = ContentAlignment.TopCenter;
                    break;
                case "TopRight":
                    rtnVal = ContentAlignment.TopRight;
                    break;
                case "MiddleLeft":
                    rtnVal = ContentAlignment.MiddleLeft;
                    break;
                case "MiddleCenter":
                    rtnVal = ContentAlignment.MiddleCenter;
                    break;
                case "MiddleRight":
                    rtnVal = ContentAlignment.MiddleRight;
                    break;
                case "BottomLeft":
                    rtnVal = ContentAlignment.BottomLeft;
                    break;
                case "BottomCenter":
                    rtnVal = ContentAlignment.BottomCenter;
                    break;
                case "BottomRight":
                    rtnVal = ContentAlignment.BottomRight;
                    break;
                default:
                    rtnVal = ContentAlignment.MiddleLeft;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 문자열 => ScrollBars
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static ScrollBars GetScrollBars(string strValue)
        {
            ScrollBars rtnVal = ScrollBars.None;
            if (strValue == null)
            {
                return rtnVal;
            }
            switch (strValue)
            {
                case "None":
                    rtnVal = ScrollBars.None;
                    break;
                case "Horizontal":
                    rtnVal = ScrollBars.Horizontal;
                    break;
                case "Vertical":
                    rtnVal = ScrollBars.Vertical;
                    break;
                case "Both":
                    rtnVal = ScrollBars.Both;
                    break;
                default:
                    rtnVal = ScrollBars.None;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 문자열 => PictureBoxSizeMode
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static PictureBoxSizeMode GetPictureBoxSizeMode(string strValue)
        {
            PictureBoxSizeMode rtnVal = PictureBoxSizeMode.Normal;
            if (strValue == null)
            {
                return rtnVal;
            }
            switch (strValue)
            {
                case "Normal":
                    rtnVal = PictureBoxSizeMode.Normal;
                    break;
                case "StretchImage":
                    rtnVal = PictureBoxSizeMode.StretchImage;
                    break;
                case "AutoSize":
                    rtnVal = PictureBoxSizeMode.AutoSize;
                    break;
                case "CenterImage":
                    rtnVal = PictureBoxSizeMode.CenterImage;
                    break;
                case "Zoom":
                    rtnVal = PictureBoxSizeMode.Zoom;
                    break;
                default:
                    rtnVal = PictureBoxSizeMode.Normal;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 문자열 => Appearance
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static Appearance GetAppearance(string strValue)
        {
            Appearance rtnVal = Appearance.Normal;
            if (strValue == null)
            {
                return rtnVal;
            }
            switch (strValue)
            {
                case "Normal":
                    rtnVal = Appearance.Normal;
                    break;
                case "Button":
                    rtnVal = Appearance.Button;
                    break;
                default:
                    rtnVal = Appearance.Normal;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 문자열 => FlatStyle
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static FlatStyle GetFlatStyle(string strValue)
        {
            FlatStyle rtnVal = FlatStyle.Standard;
            if (strValue == null)
            {
                return rtnVal;
            }
            switch (strValue)
            {
                case "Flat":
                    rtnVal = FlatStyle.Flat;
                    break;
                //case "Popup":
                //    rtnVal = FlatStyle.Popup;
                //    break;
                //case "System":
                //    rtnVal = FlatStyle.System;
                //    break;
                case "Standard":
                    rtnVal = FlatStyle.Standard;
                    break;
                default:
                    rtnVal = FlatStyle.Standard;
                    break;
            }

            return rtnVal;
        }

        #endregion //문자열 => 컨트롤 속성

    }
}
