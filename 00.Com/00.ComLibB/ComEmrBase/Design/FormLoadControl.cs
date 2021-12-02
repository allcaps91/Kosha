using System;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComEmrBase.Properties;

namespace ComEmrBase
{
    /// <summary>
    /// 디자인 관련 함수 모음 : 서식지 로드시
    /// </summary>
    public class FormLoadControl
    {
        #region //폼생성

        /// <summary>
        /// 서식지 로드
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="strParent"></param>
        public static void LoadControl(Form pForm, FormXml[] pFormXml, string strParent)
        {
            int i = 0;
            Control cParent = null;
            Control[] tx = null;

            tx = pForm.Controls.Find(strParent, true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    if (tx[0] is mtsPanel15.mPanel) cParent = (mtsPanel15.mPanel)tx[0];
                    if (tx[0] is Panel) cParent = (Panel)tx[0];
                    if (tx[0] is GroupBox) cParent = (GroupBox)tx[0];
                }
            }
            else
            {
                return;
            }

            int panHeight = 0;
            int panTop = 0;
            int intItemHeight = 0;

            #region //지우지 마세요 : 없어도 되지만
            if (cParent is mtsPanel15.mPanel)
            {
                panTop = ((mtsPanel15.mPanel)cParent).Top;
                panHeight = ((mtsPanel15.mPanel)cParent).Height;
            }
            else
            {
                if (cParent is Panel)
                {
                    panTop = ((Panel)cParent).Top;
                    panHeight = ((Panel)cParent).Height;
                }
                if (cParent is GroupBox)
                {
                    panTop = ((GroupBox)cParent).Top;
                    panHeight = ((GroupBox)cParent).Height;
                }
            }
            #endregion //지우지 마세요 : 없어도 되지만

            //***시작***//
            panTop = 0;

            for (i = 0; i < pFormXml.Length; i++)
            {
                if (pFormXml[i].strCONTROLPARENT == strParent)
                {
                    loadDesign(pForm, pFormXml, i, ref intItemHeight, ref panTop);
                }
            }

            if (intItemHeight == 0)
            {
                if (cParent is mtsPanel15.mPanel)
                {
                    ((mtsPanel15.mPanel)cParent).Visible = false;
                    ((mtsPanel15.mPanel)cParent).Height = 0;

                }
                else
                {
                    if (cParent is Panel)
                    {
                        ((Panel)cParent).Visible = false;
                        ((Panel)cParent).Height = 0;
                    }
                    if (cParent is GroupBox)
                    {
                        ((GroupBox)cParent).Visible = false;
                        ((GroupBox)cParent).Height = 0;
                    }
                }
            }
            else
            {
                if (cParent is mtsPanel15.mPanel)
                {
                    ((mtsPanel15.mPanel)cParent).Height = intItemHeight;
                }
                else
                {
                    if (cParent is Panel)
                    {
                        ((Panel)cParent).Height = intItemHeight;
                    }
                    if (cParent is GroupBox)
                    {
                        ((GroupBox)cParent).Height = intItemHeight;
                    }
                }
            }
        }

        /// <summary>
        /// 서식지 로드
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="strParent"></param>
        public static void LoadControl(Control pForm, FormXml[] pFormXml, string strParent)
        {
            int i = 0;
            Control cParent = null;
            Control[] tx = null;

            tx = pForm.Controls.Find(strParent, true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    if (tx[0] is mtsPanel15.mPanel) cParent = (mtsPanel15.mPanel)tx[0];
                    if (tx[0] is Panel) cParent = (Panel)tx[0];
                    if (tx[0] is GroupBox) cParent = (GroupBox)tx[0];
                }
            }
            else
            {
                return;
            }

            int panHeight = 0;
            int panTop = 0;
            int intItemHeight = 0;

            #region //지우지 마세요 : 없어도 되지만
            if (cParent is mtsPanel15.mPanel)
            {
                panTop = ((mtsPanel15.mPanel)cParent).Top;
                panHeight = ((mtsPanel15.mPanel)cParent).Height;
            }
            else
            {
                if (cParent is Panel)
                {
                    panTop = ((Panel)cParent).Top;
                    panHeight = ((Panel)cParent).Height;
                }
                if (cParent is GroupBox)
                {
                    panTop = ((GroupBox)cParent).Top;
                    panHeight = ((GroupBox)cParent).Height;
                }
            }
            #endregion //지우지 마세요 : 없어도 되지만

            //***시작***//
            panTop = 0;

            for (i = 0; i < pFormXml.Length; i++)
            {
                if (pFormXml[i].strCONTROLPARENT == strParent)
                {
                    loadDesign(pForm, pFormXml, i, ref intItemHeight, ref panTop);
                }
            }

            if (intItemHeight == 0)
            {
                if (cParent is mtsPanel15.mPanel)
                {
                    ((mtsPanel15.mPanel)cParent).Visible = false;
                    ((mtsPanel15.mPanel)cParent).Height = 0;
                }
                else
                {
                    if (cParent is Panel)
                    {
                        ((Panel)cParent).Visible = false;
                        ((Panel)cParent).Height = 0;
                    }
                    if (cParent is GroupBox)
                    {
                        ((GroupBox)cParent).Visible = false;
                        ((GroupBox)cParent).Height = 0;
                    }
                }
            }
            else
            {
                if (cParent is mtsPanel15.mPanel)
                {
                    ((mtsPanel15.mPanel)cParent).Height = intItemHeight;
                }
                else
                {
                    if (cParent is Panel)
                    {
                        ((Panel)cParent).Height = intItemHeight;
                    }
                    if (cParent is GroupBox)
                    {
                        ((GroupBox)cParent).Height = intItemHeight;
                    }
                }
            }
        }

        /// <summary>
        /// 컨트롤 로드
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="i"></param>
        /// <param name="intItemHeight"></param>
        /// <param name="panTop"></param>
        public static void loadDesign(Form pForm, FormXml[] pFormXml, int i, ref int intItemHeight, ref int panTop)
        {
            try
            {
                if (pFormXml[i].strCONTROTYPE == "mtsPanel15.mPanel" || pFormXml[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                {
                    mtsPanel15.mPanel ctrl = new mtsPanel15.mPanel();

                    #region 19-08-08 정승민 과장 요청 입력할수 있는 컨트롤들만 탭키 가능하게(패널 제외)
                    ctrl.TabStop = false;
                    ctrl.TabIndex = 99999;
                    #endregion

                    SetDefaultValue(pForm, pFormXml[i], ctrl);

                    ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                    ctrl.BringToFront();

                    LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                }
                else
                {
                    switch (pFormXml[i].strCONTROTYPE)
                    {
                        #region // Panel

                        case "System.Windows.Forms.Panel": //현재화면 사용가능여부 조회
                            {
                                Panel ctrl = new Panel();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();

                                LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                            }
                            break;
                        case "mtsPanel15.mPanel": //현재화면 사용가능여부 조회
                            {
                                mtsPanel15.mPanel ctrl = new mtsPanel15.mPanel();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();

                                LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                            }
                            break;
                        case "System.Windows.Forms.GroupBox": //현재화면 사용가능여부 조회
                            {
                                GroupBox ctrl = new GroupBox();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BringToFront();

                                LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                            }
                            break;
                        #endregion

                        #region // Control

                        case "System.Windows.Forms.Label": //현재화면 사용가능여부 조회
                            {
                                Label ctrl = new Label();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.TextAlign = FormFunc.GetContentAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.AutoSize = FormFunc.GetTrueFalse(pFormXml[i].strAUTOSIZE);
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();
                                
                                if (ctrl.Name.Trim().Length > 6)
                                {
                                    if (ctrl.Name.Trim().Substring(0, 6) == "TITLE_")
                                    {
                                        if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                        {
                                            if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                        }
                                    }
                                }

                            }
                            break;
                        case "System.Windows.Forms.ComboBox": //현재화면 사용가능여부 조회
                            {
                                ComboBox ctrl = new ComboBox();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);

                                ctrl.BringToFront();

                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.TextBox": //현재화면 사용가능여부 조회
                            {
                                TextBox ctrl = new TextBox();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.MaxLength = (int)(VB.Val(pFormXml[i].strMAXTEXTLENGTH) == 0 ? 32767 : VB.Val(pFormXml[i].strMAXTEXTLENGTH));
                                ctrl.ReadOnly = FormFunc.GetTrueFalse(pFormXml[i].strTEXTREADONLY);
                                ctrl.TextAlign = FormFunc.GetControlAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.Multiline = (pFormXml[i].strMULTILINE == "True" ? true : false);
                                ctrl.ScrollBars = ScrollBars.Vertical;
                                if (ctrl.Multiline == true) //높이 조절용
                                {
                                    ctrl.Tag = pFormXml[i].strAUTOHEIGH;
                                }
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.RadioButton": //현재화면 사용가능여부 조회
                            {
                                RadioButton ctrl = new RadioButton();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.TextAlign = FormFunc.GetContentAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.AutoSize = FormFunc.GetTrueFalse(pFormXml[i].strAUTOSIZE);
                                //ctrl.Checked = (pFormXml[i].strCHECKED == "True" ? true : false);
                                ctrl.Appearance = FormFunc.GetAppearance(pFormXml[i].strAPPEARANCS);
                                if (ctrl.Appearance == Appearance.Button)
                                {
                                    ctrl.FlatAppearance.BorderSize = (int)VB.Val(pFormXml[i].strFLATBORDERSIZE);
                                }
                                ctrl.FlatStyle = FormFunc.GetFlatStyle(pFormXml[i].strFLATSTYLE);
                                ctrl.Checked = (pFormXml[i].strCHECKED == "True" ? true : false);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.CheckBox": //현재화면 사용가능여부 조회
                            {
                                CheckBox ctrl = new CheckBox();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.TextAlign = FormFunc.GetContentAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.CheckAlign = FormFunc.GetContentAlign(pFormXml[i].strCHECKALIGN);
                                ctrl.AutoSize = FormFunc.GetTrueFalse(pFormXml[i].strAUTOSIZE);
                                ctrl.Appearance = FormFunc.GetAppearance(pFormXml[i].strAPPEARANCS);
                                if (ctrl.Appearance == Appearance.Button)
                                {
                                    ctrl.FlatAppearance.BorderSize = (int)VB.Val(pFormXml[i].strFLATBORDERSIZE);
                                }
                                ctrl.FlatStyle = FormFunc.GetFlatStyle(pFormXml[i].strFLATSTYLE);
                                //ctrl.Checked = (pFormXml[i].strCHECKED == "True" ? true : false);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.PictureBox": //현재화면 사용가능여부 조회
                            {
                                PictureBox ctrl = new PictureBox();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.Image = pFormXml[i].imgIMAGE;
                                ctrl.SizeMode = PictureBoxSizeMode.StretchImage;
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();

                                #region 19-10-07 이미지 비었을경우 노이미지 보이게
                                if (ctrl.Image == null)
                                {
                                    ctrl.BackColor = Color.LightGray;
                                    ctrl.Image = Resources.noimage;
                                    ctrl.Tag = null;
                                }
                                #endregion

                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.Button":
                            {
                                Button ctrl = new Button();
                                ctrl.TabStop = false;
                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        #endregion

                        #region //EtcControl
                        case "System.Windows.Forms.ListBox": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.DataGridView": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.NumericUpDown": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.TreeView": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.DateTimePicker": //현재화면 사용가능여부 조회
                            break;
                            #endregion


                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 컨트롤 로드
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="i"></param>
        /// <param name="intItemHeight"></param>
        /// <param name="panTop"></param>
        public static void loadDesign(Control pForm, FormXml[] pFormXml, int i, ref int intItemHeight, ref int panTop)
        {
            try
            {
                if (pFormXml[i].strCONTROTYPE == "mtsPanel15.mPanel" || pFormXml[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                {
                    mtsPanel15.mPanel ctrl = new mtsPanel15.mPanel();

                    SetDefaultValue(pForm, pFormXml[i], ctrl);

                    ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                    ctrl.BringToFront();

                    LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                }
                else
                {
                    switch (pFormXml[i].strCONTROTYPE)
                    {
                        #region // Panel

                        case "System.Windows.Forms.Panel": //현재화면 사용가능여부 조회
                            {
                                Panel ctrl = new Panel();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();

                                LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                            }
                            break;
                        case "mtsPanel15.mPanel": //현재화면 사용가능여부 조회
                            {
                                mtsPanel15.mPanel ctrl = new mtsPanel15.mPanel();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();

                                LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                            }
                            break;
                        case "System.Windows.Forms.GroupBox": //현재화면 사용가능여부 조회
                            {
                                GroupBox ctrl = new GroupBox();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BringToFront();

                                LoadControlSub(pForm, pFormXml, i, ctrl, ref intItemHeight);
                            }
                            break;
                        #endregion

                        #region // Control

                        case "System.Windows.Forms.Label": //현재화면 사용가능여부 조회
                            {
                                Label ctrl = new Label();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.TextAlign = FormFunc.GetContentAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.AutoSize = FormFunc.GetTrueFalse(pFormXml[i].strAUTOSIZE);
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();
                                if (ctrl.Name.Trim().Length > 6)
                                {
                                    if (ctrl.Name.Trim().Substring(0, 6) == "TITLE_")
                                    {
                                        if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                        {
                                            if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                        }
                                    }
                                }
                            }
                            break;
                        case "System.Windows.Forms.ComboBox": //현재화면 사용가능여부 조회
                            {
                                ComboBox ctrl = new ComboBox();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);

                                ctrl.BringToFront();

                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.TextBox": //현재화면 사용가능여부 조회
                            {
                                TextBox ctrl = new TextBox();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.MaxLength = (int) (VB.Val(pFormXml[i].strMAXTEXTLENGTH) == 0 ? 32767 : VB.Val(pFormXml[i].strMAXTEXTLENGTH)) ;
                                ctrl.ReadOnly = FormFunc.GetTrueFalse(pFormXml[i].strTEXTREADONLY);
                                ctrl.TextAlign = FormFunc.GetControlAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.Multiline = (pFormXml[i].strMULTILINE == "True" ? true : false);
                                if (ctrl.Multiline == true) //높이 조절용
                                {
                                    ctrl.Tag = pFormXml[i].strAUTOHEIGH;
                                }
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.RadioButton": //현재화면 사용가능여부 조회
                            {
                                RadioButton ctrl = new RadioButton();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.TextAlign = FormFunc.GetContentAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.AutoSize = FormFunc.GetTrueFalse(pFormXml[i].strAUTOSIZE);
                                //ctrl.Checked = (pFormXml[i].strCHECKED == "True" ? true : false);
                                ctrl.Appearance = FormFunc.GetAppearance(pFormXml[i].strAPPEARANCS);
                                if (ctrl.Appearance == Appearance.Button)
                                {
                                    ctrl.FlatAppearance.BorderSize = (int)VB.Val(pFormXml[i].strFLATBORDERSIZE);
                                }
                                ctrl.FlatStyle = FormFunc.GetFlatStyle(pFormXml[i].strFLATSTYLE);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.CheckBox": //현재화면 사용가능여부 조회
                            {
                                CheckBox ctrl = new CheckBox();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.TextAlign = FormFunc.GetContentAlign(pFormXml[i].strTEXTALIGN);
                                ctrl.CheckAlign = FormFunc.GetContentAlign(pFormXml[i].strCHECKALIGN);
                                ctrl.AutoSize = FormFunc.GetTrueFalse(pFormXml[i].strAUTOSIZE);
                                ctrl.Appearance = FormFunc.GetAppearance(pFormXml[i].strAPPEARANCS);
                                if (ctrl.Appearance == Appearance.Button)
                                {
                                    ctrl.FlatAppearance.BorderSize = (int)VB.Val(pFormXml[i].strFLATBORDERSIZE);
                                }
                                ctrl.FlatStyle = FormFunc.GetFlatStyle(pFormXml[i].strFLATSTYLE);
                                //ctrl.Checked = (pFormXml[i].strCHECKED == "True" ? true : false);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.PictureBox": //현재화면 사용가능여부 조회
                            {
                                PictureBox ctrl = new PictureBox();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.Image = pFormXml[i].imgIMAGE;
                                ctrl.SizeMode = PictureBoxSizeMode.Zoom;
                                ctrl.BorderStyle = FormFunc.GetBorderStyle(pFormXml[i].strBOARDSTYLE);
                                ctrl.BringToFront();

                                #region 19-10-07 이미지 비었을경우 노이미지 보이게
                                if (ctrl.Image == null)
                                {
                                    ctrl.BackColor = Color.LightGray;
                                    ctrl.Image = Resources.noimage;
                                    ctrl.Tag = null;
                                }
                                #endregion

                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        case "System.Windows.Forms.Button":
                            {
                                Button ctrl = new Button();

                                SetDefaultValue(pForm, pFormXml[i], ctrl);
                                ctrl.BringToFront();
                                if (pFormXml[i].strVISIBLED.ToUpper() == "TRUE")
                                {
                                    if (intItemHeight < ctrl.Top + ctrl.Height) intItemHeight = ctrl.Top + ctrl.Height;
                                }
                            }
                            break;
                        #endregion

                        #region //EtcControl
                        case "System.Windows.Forms.ListBox": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.DataGridView": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.NumericUpDown": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.TreeView": //현재화면 사용가능여부 조회
                            break;
                        case "System.Windows.Forms.DateTimePicker": //현재화면 사용가능여부 조회
                            break;
                            #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 공통 값 세팅
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="pctrl"></param>
        public static void SetDefaultValue(Form pForm, FormXml pFormXml, Control pctrl)
        {
            try
            {
                #region //주석1
                if (pFormXml.strCONTROLPARENT == "")
                {
                    pForm.Controls.Add(pctrl);
                    pForm.Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                }
                else
                {
                    Control[] tx = null;
                    tx = pForm.Controls.Find(pFormXml.strCONTROLPARENT, true);
                    if (tx != null)
                    {
                        if (tx.Length > 0)
                        {
                            if (tx[0] is mtsPanel15.mPanel)
                            {
                                ((mtsPanel15.mPanel)tx[0]).Controls.Add(pctrl);
                                ((mtsPanel15.mPanel)tx[0]).Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                            }
                            else
                            {
                                if (tx[0] is Panel)
                                {
                                    ((Panel)tx[0]).Controls.Add(pctrl);
                                    ((Panel)tx[0]).Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                                }
                                else if (tx[0] is GroupBox)
                                {
                                    ((GroupBox)tx[0]).Controls.Add(pctrl);
                                    ((GroupBox)tx[0]).Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                                }
                                else
                                {
                                    pForm.Controls.Add(pctrl);
                                    pForm.Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                                }
                            }
                        }
                    }
                }
                
                #endregion //주석1

                System.Drawing.Color myBackColor = new System.Drawing.Color();
                myBackColor = System.Drawing.ColorTranslator.FromHtml(pFormXml.strBACKCOLOR);
                System.Drawing.Color myForeColorColor = new System.Drawing.Color();
                myForeColorColor = System.Drawing.ColorTranslator.FromHtml(pFormXml.strFORECOLOR);
                if ((pctrl is PictureBox) == false)
                {
                    var cvt = new FontConverter();
                    Font f = FormFunc.StringToFont(pFormXml.strFONTS);
                    pctrl.Font = f;
                }
                pctrl.Name = pFormXml.strCONTROLNAME;
                pctrl.Location = new Point(System.Convert.ToInt32(pFormXml.strLOCATIONX), System.Convert.ToInt32(pFormXml.strLOCATIONY));
                pctrl.Text = pFormXml.strTEXT;

                pctrl.BackColor = myBackColor;
                pctrl.ForeColor = myForeColorColor;
                pctrl.Tag = pFormXml.strUSERFUNC;
                pctrl.Size = new System.Drawing.Size(System.Convert.ToInt32(pFormXml.strSIZEWIDTH), System.Convert.ToInt32(pFormXml.strSIZEHEIGHT));
                pctrl.Dock = FormFunc.GetDockStyle(pFormXml.strDOCK);
                pctrl.Enabled = FormFunc.GetTrueFalse(pFormXml.strENABLED);
                pctrl.Visible = FormFunc.GetTrueFalse(pFormXml.strVISIBLED);
                //pctrl.TabStop = false;
                pctrl.TabIndex = 100000 + (int)VB.Val(pFormXml.strTABORDER);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 공통 값 세팅
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="pctrl"></param>
        public static void SetDefaultValue(Control pForm, FormXml pFormXml, Control pctrl)
        {
            try
            {
                #region //주석1
                if (pFormXml.strCONTROLPARENT == "")
                {
                    pForm.Controls.Add(pctrl);
                    pForm.Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                }
                else
                {
                    Control[] tx = null;
                    tx = pForm.Controls.Find(pFormXml.strCONTROLPARENT, true);
                    if (tx != null)
                    {
                        if (tx.Length > 0)
                        {
                            if (tx[0] is mtsPanel15.mPanel)
                            {
                                ((mtsPanel15.mPanel)tx[0]).Controls.Add(pctrl);
                                ((mtsPanel15.mPanel)tx[0]).Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                            }
                            else
                            {
                                if (tx[0] is Panel)
                                {
                                    ((Panel)tx[0]).Controls.Add(pctrl);
                                    ((Panel)tx[0]).Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                                }
                                else if (tx[0] is GroupBox)
                                {
                                    ((GroupBox)tx[0]).Controls.Add(pctrl);
                                    ((GroupBox)tx[0]).Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                                }
                                else
                                {
                                    pForm.Controls.Add(pctrl);
                                    pForm.Controls.SetChildIndex(pctrl, Convert.ToInt32(pFormXml.strCHILDINDEX));
                                }
                            }
                        }
                    }
                }

                #endregion //주석1

                System.Drawing.Color myBackColor = new System.Drawing.Color();
                myBackColor = System.Drawing.ColorTranslator.FromHtml(pFormXml.strBACKCOLOR);
                System.Drawing.Color myForeColorColor = new System.Drawing.Color();
                myForeColorColor = System.Drawing.ColorTranslator.FromHtml(pFormXml.strFORECOLOR);
                if ((pctrl is PictureBox) == false)
                {
                    var cvt = new FontConverter();
                    Font f = FormFunc.StringToFont(pFormXml.strFONTS);
                    pctrl.Font = f;
                }
                pctrl.Name = pFormXml.strCONTROLNAME;
                pctrl.Location = new Point(System.Convert.ToInt32(pFormXml.strLOCATIONX), System.Convert.ToInt32(pFormXml.strLOCATIONY));
                pctrl.Text = pFormXml.strTEXT;

                pctrl.BackColor = myBackColor;
                pctrl.ForeColor = myForeColorColor;
                pctrl.Tag = pFormXml.strUSERFUNC;
                pctrl.Size = new System.Drawing.Size(System.Convert.ToInt32(pFormXml.strSIZEWIDTH), System.Convert.ToInt32(pFormXml.strSIZEHEIGHT));
                pctrl.Dock = FormFunc.GetDockStyle(pFormXml.strDOCK);
                pctrl.Enabled = FormFunc.GetTrueFalse(pFormXml.strENABLED);
                pctrl.Visible = FormFunc.GetTrueFalse(pFormXml.strVISIBLED);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 서식지 로드 : 재귀호출
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="intNow"></param>
        /// <param name="cParent"></param>
        /// <param name="intItemHeightX"></param>
        public static void LoadControlSub(Form pForm, FormXml[] pFormXml, int intNow, Control cParent, ref int intItemHeightX)
        {
            int i = 0;

            int panTop = 0;
            int panHeight = 0;
            int intItemHeight = 0;

            if (cParent is mtsPanel15.mPanel)
            {
                panTop = ((mtsPanel15.mPanel)cParent).Top;
                panHeight = ((mtsPanel15.mPanel)cParent).Height;
            }
            else
            {
                if (cParent is Panel)
                {
                    panTop = ((Panel)cParent).Top;
                    panHeight = ((Panel)cParent).Height;
                }
                if (cParent is GroupBox)
                {
                    panTop = ((GroupBox)cParent).Top;
                    panHeight = ((GroupBox)cParent).Height;
                }
            }

            //panTop = 0; intNow 이거 사용하면 디자인 이상해진다
            for (i = 0; i < pFormXml.Length; i++)
            {
                if (pFormXml[i].strCONTROLPARENT == cParent.Name)
                {
                    loadDesign(pForm, pFormXml, i, ref intItemHeight, ref panTop);
                }
            }
            
            if (intItemHeightX < intItemHeight)
            {
                intItemHeightX = intItemHeight;
            }
        }

        /// <summary>
        /// 서식지 로드 : 재귀호출
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="pFormXml"></param>
        /// <param name="intNow"></param>
        /// <param name="cParent"></param>
        /// <param name="intItemHeightX"></param>
        public static void LoadControlSub(Control pForm, FormXml[] pFormXml, int intNow, Control cParent, ref int intItemHeightX)
        {
            int i = 0;

            int panTop = 0;
            int panHeight = 0;
            int intItemHeight = 0;

            if (cParent is mtsPanel15.mPanel)
            {
                panTop = ((mtsPanel15.mPanel)cParent).Top;
                panHeight = ((mtsPanel15.mPanel)cParent).Height;
            }
            else
            {
                if (cParent is Panel)
                {
                    panTop = ((Panel)cParent).Top;
                    panHeight = ((Panel)cParent).Height;
                }
                if (cParent is GroupBox)
                {
                    panTop = ((GroupBox)cParent).Top;
                    panHeight = ((GroupBox)cParent).Height;
                }
            }

            //panTop = 0; intNow 이거 사용하면 디자인 이상해진다
            for (i = 0; i < pFormXml.Length; i++)
            {
                if (pFormXml[i].strCONTROLPARENT == cParent.Name)
                {
                    loadDesign(pForm, pFormXml, i, ref intItemHeight, ref panTop);
                }
            }

            if (intItemHeightX < intItemHeight)
            {
                intItemHeightX = intItemHeight;
            }
        }
        #endregion //폼생성
    }
}
