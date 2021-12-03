using ComBase;
using ComLibB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;
using System.IO;

/// <summary>
/// Class Name      : HC_School
/// File Name       : frmHcSchoolExamPanjeng.cs
/// Description     : 학생검진판정
/// Author          : 이상훈
/// Create Date     : 2020-02-04
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm학생판정.frm(Frm학생판정)" />

namespace HC_School
{
    public partial class frmHcSchoolExamPanjeng : Form
    {
        HicSchoolNewService hicSchoolNewService = null;
        HicResultwardService hicResultwardService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuPatientSchoolSangdamService hicJepsuPatientSchoolSangdamService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicXrayResultService hicXrayResultService = null;
        HicSangdamNewService hicSangdamNewService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcSchoolCommonInput FrmHcSchoolCommonInput = null;
        frmViewResult FrmViewResult = null;
        frmHcSangStudentCounsel FrmHcSangStudentCounsel = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

        string FstrROWID;
        string FstrEdit;
        string FstrSex;
        long FnAge;
        double FnHeight;
        double FnWeight;
        string FstrPano;
        string FstrFlag;
        string FstrJepDate;
        string FstrGjJong;
        int nPan1;  //정상(경계)
        int nPan2;  //정밀검사요함
        long FnWRTNO;

        public frmHcSchoolExamPanjeng()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicSchoolNewService = new HicSchoolNewService();
            hicResultwardService = new HicResultwardService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            hicJepsuPatientSchoolSangdamService = new HicJepsuPatientSchoolSangdamService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicXrayResultService = new HicXrayResultService();
            hicSangdamNewService = new HicSangdamNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuResult.Click += new EventHandler(eBtnClick);
            this.btnSet1.Click += new EventHandler(eBtnClick);
            this.btnSet2.Click += new EventHandler(eBtnClick);
            this.btnExam_result.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.cboPan.KeyPress += new KeyPressEventHandler(eCboKeyPress);

            this.txtPanDate.DoubleClick += new EventHandler(eTxtDblClick);

            this.txtB1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB3.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB4.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB5.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB6.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtBH.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtBL.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtColor.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtEJ1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtH1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtH2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtHJ.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ0.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ3.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtLiver.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtM.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtN.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan0.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan3.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan4.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan5.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtPRes1.GotFocus += new EventHandler(eTxtGotFocus); 
            this.txtS.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSWeight.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU3.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtColor.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtE1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtEJ1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtH1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtH2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHJ.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLiver.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtM.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtN.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtNE1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtNE2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanJochi.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPRes1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtS.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSWeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtB1.LostFocus += new EventHandler(eTxtLostFocus);            
            this.txtB2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtB3.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtB4.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtB5.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtB6.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtBH.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtBL.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtColor.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtE1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtE2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtEJ1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtEJ2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtH1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtH2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtHJ.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtLiver.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtM.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtN.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtNE1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtNE2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan0.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan3.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan4.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan5.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtPRes1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtS.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtU1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtU2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtU3.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtColor)
            {
                if (e.KeyChar == 0)
                {
                    lblPan2.Text = hf.READ_YN2("2", txtColor.Text.Trim());
                }
            }
            else if (sender == txtE1)
            {
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtE2)
            {
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtEJ1)
            {
                if (e.KeyChar == 0)
                {
                    lblPanEye1.Text = hf.READ_Eye("2", txtEJ1.Text.Trim());
                }
                if (txtEJ1.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtH1)
            {
                if (e.KeyChar == 0)
                {
                    lblPanH1.Text = hf.READ_YN2("2", txtH1.Text.Trim());
                }
            }
            else if (sender == txtH2)
            {
                if (e.KeyChar == 0)
                {
                    lblPanH2.Text = hf.READ_YN2("2", txtH2.Text.Trim());
                }
            }
            else if (sender == txtHJ)
            {
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtJ0)
            {
                lblPan17.Text = hf.READ_YN_1("2", txtJ0.Text.Trim());
                if (double.Parse(txtJ0.Text) == 1)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtJ2)
            {
                lblPan19.Text = hf.READ_YN_1("2", txtJ2.Text.Trim());
                if (double.Parse(txtJ2.Text) == 1)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtJ1)
            {
                lblPan18.Text = hf.READ_Old1("2", txtJ1.Text.Trim());
                if (double.Parse(txtJ1.Text) == 1)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtJ3)
            {
                lblPan20.Text = hf.READ_Old2("2", txtJ3.Text.Trim());
                if (double.Parse(txtJ3.Text) == 1)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtLiver)
            {
                if (e.KeyChar == 0)
                {
                    lblPan15.Text = hf.READ_PM("2", txtLiver.Text.Trim());
                }
            }
            else if (sender == txtM)
            {
                if (e.KeyChar == 0)
                {
                    lblPan21.Text = hf.READ_Nose("2", txtM.Text.Trim());
                }
                if (txtM.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtN)
            {
                if (e.KeyChar == 0)
                {
                    lblPan4.Text = hf.READ_Neck("2", txtN.Text.Trim());
                }
                if (txtN.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtName)
            {
                if (e.KeyChar == 13)
                {
                    btnSearch.Focus();
                }
            }
            else if (sender == txtNE1)
            {
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtOrgan0)
            {
                if (e.KeyChar == 0)
                {
                    lblPan6.Text = hf.READ_YN_NEW("2", txtOrgan0.Text.Trim());
                }
                if (txtOrgan0.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtOrgan1)
            {
                if (e.KeyChar == 0)
                {
                    lblPan7.Text = hf.READ_YN_NEW("2", txtOrgan1.Text.Trim());
                }
                if (txtOrgan1.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtOrgan2)
            {
                if (e.KeyChar == 0)
                {
                    lblPan8.Text = hf.READ_YN_NEW("2", txtOrgan2.Text.Trim());
                }
                if (txtOrgan2.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtOrgan3)
            {
                if (e.KeyChar == 0)
                {
                    lblPan9.Text = hf.READ_YN_NEW("2", txtOrgan3.Text.Trim());
                }
                if (txtOrgan3.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtOrgan4)
            {
                if (e.KeyChar == 0)
                {
                    lblPan10.Text = hf.READ_YN_NEW("2", txtOrgan4.Text.Trim());
                }
                if (txtOrgan4.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtOrgan5)
            {
                if (e.KeyChar == 0)
                {
                    lblPan11.Text = hf.READ_YN_NEW("2", txtOrgan5.Text.Trim());
                }
                if (txtOrgan5.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtPanJochi)
            {
                if (int.Parse(txtPanJochi.Text.Trim()) > 300)
                {
                    MessageBox.Show("조치사항은 최대 300자까지 세팅되어있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPanJochi.Focus();
                    return;
                }
            }
            else if (sender == txtPanSogen)
            {
                if (int.Parse(txtPanSogen.Text.Trim()) > 300)
                {
                    MessageBox.Show("종합소견은 최대 300자까지 세팅되어있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPanSogen.Focus();
                    return;
                }
            }
            else if (sender == txtPRes1)
            {
                if (e.KeyChar == 0)
                {
                    lblPan22.Text = hf.READ_Mush("2", txtPRes1.Text.Trim());
                }
            }
            else if (sender == txtS)
            {
                if (e.KeyChar == 0)
                {
                    lblPan5.Text = hf.READ_Skin("2", txtS.Text.Trim());
                }
                if (txtS.Text.Trim() == "1")
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtSWeight)
            {
                if (e.KeyChar == 0)
                {
                    lblPan1.Text = hf.READ_Biman("4", txtSWeight.Text.Trim());
                }
            }
            else if (sender == txtU1)
            {
                if (e.KeyChar == 0)
                {
                    lblPan12.Text = hf.READ_URO("2", txtU1.Text.Trim());
                }
            }
            else if (sender == txtU2)
            {
                if (e.KeyChar == 0)
                {
                    lblPan13.Text = hf.READ_URO("2", txtU2.Text.Trim());
                }
            }
            else if (sender == txtU3)
            {
                if (e.KeyChar == 0)
                {
                    lblPan16.Text = hf.READ_URO("2", txtU3.Text.Trim());
                }
            }
            else if (sender == txtU4)
            {
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtWeight)
            {
                int nValue = 0;
                string strChejil = "";
                string strBiman = "";

                nValue = 0;
                if (e.KeyChar == 0)
                {
                    FnHeight = double.Parse(txtHeight.Text);
                    FnWeight = double.Parse(txtWeight.Text);

                    if (txtHeight.Text.Trim() != "" && txtWeight.Text.Trim() != "")
                    {
                        //체질량
                        fn_ObesityLevel_AutoPangeng();
                        //상대체중
                        strBiman = hm.CALC_Biman_Rate(double.Parse(txtWeight.Text), double.Parse(txtHeight.Text), FstrSex);
                        if (strBiman != "")
                        {
                            txtSWeight.Text = VB.Pstr(strBiman, ".", 1);
                            lblPan1.Text = VB.Pstr(strBiman, ".", 2);
                        }
                    }

                    if (txtGrade1.Text.Trim() == "과체중" || txtGrade1.Text.Trim() == "비만" || txtGrade1.Text.Trim() == "저체중")
                    {
                        txtGrade1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    }
                    else
                    {
                        txtGrade1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                    if (double.Parse(txtSWeight.Text) > 1)
                    {
                        txtSWeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    }
                    else
                    {
                        txtSWeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                }
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtX)
            {
                if (e.KeyChar == 0)
                {
                    lblPan14.Text = hf.READ_Xray("2", txtX.Text.Trim());
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtB1 || sender == txtB2 || sender == txtB3 || sender == txtB4 || sender == txtB5 || sender == txtBH || sender == txtBL)
            {
                lblMsg.Text = "결과값 입력";
            }
            else if (sender == txtB6)
            {
                txtB6.ImeMode = ImeMode.Alpha;
            }
            else if (sender == txtColor)
            {
                lblMsg.Text = hf.READ_YN2("1", "");
            }
            else if (sender == txtEJ1)
            {
                lblMsg.Text = hf.READ_Eye("1", "");
            }
            else if (sender == txtEJ2)
            {
                lblMsg.Text = hf.READ_Eye("2", txtEJ2.Text.Trim());
            }
            else if (sender == txtH1)
            {
                lblMsg.Text = hf.READ_YN2("1", "");
            }
            else if (sender == txtH2)
            {
                lblMsg.Text = hf.READ_YN2("1", "");
            }
            else if (sender == txtHJ)
            {
                lblMsg.Text = hf.READ_Hear("1", "");
            }
            else if (sender == txtJ0 || sender == txtJ2)
            {
                lblMsg.Text = hf.READ_YN_1("1", "");
            }
            else if (sender == txtJ1)
            {
                lblMsg.Text = hf.READ_Old1("1", "");
            }
            else if (sender == txtJ3)
            {
                lblMsg.Text = hf.READ_Old2("1", "");
            }
            else if (sender == txtLiver)
            {
                lblMsg.Text = hf.READ_PM("1", "");
            }
            else if (sender == txtM)
            {
                lblMsg.Text = hf.READ_Nose("1", "");
            }
            else if (sender == txtN)
            {
                lblMsg.Text = hf.READ_Neck("1", "");
            }
            else if (sender == txtName)
            {
                txtName.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtOrgan0 || sender == txtOrgan1 || sender == txtOrgan2 || sender == txtOrgan3 || sender == txtOrgan4 || sender == txtOrgan5)
            {
                lblMsg.Text = hf.READ_YN_NEW("1", "");
            }
            else if (sender == txtPRes1)
            {
                lblMsg.Text = hf.READ_Mush("1", "");
            }
            else if (sender == txtS)
            {
                lblMsg.Text = hf.READ_Skin("1", "");
            }
            else if (sender == txtSWeight)
            {
                lblMsg.Text = hf.READ_Biman("3", "");
            }
            else if (sender == txtU1)
            {
                lblMsg.Text = hf.READ_URO("1", "");
            }
            else if (sender == txtU2)
            {
                lblMsg.Text = hf.READ_URO("1", "");
            }
            else if (sender == txtU3)
            {
                lblMsg.Text = hf.READ_URO("1", "");
            }
            else if (sender == txtX)
            {
                lblMsg.Text = hf.READ_Xray("1", "");
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtB1)
            {
                if (txtB1.Text.Trim() == "")
                {
                    return;
                }
                if (double.Parse(txtB1.Text) >= 100 && double.Parse(txtB1.Text) <= 125)
                {
                    lblPan25.Text = "정상(경계)";
                    txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (double.Parse(txtB1.Text) >= 126)
                {
                    lblPan25.Text = "정밀검사요함";
                    txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    lblPan25.Text = "정상";
                    txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtB2)
            {
                if (txtB2.Text.Trim() == "")
                {
                    return;
                }
                if (double.Parse(txtB2.Text) >= 200)
                {
                    lblPan26.Text = "정밀검사요함";
                    txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (double.Parse(txtB2.Text) >= 171 && double.Parse(txtB2.Text) <= 199)
                {
                    lblPan26.Text = "정상(경계)";
                    txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    lblPan26.Text = "정상";
                    txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtB3)
            {
                if (txtB3.Text.Trim() == "")
                {
                    return;
                }
                if (FnAge < 10)
                {
                    if (double.Parse(txtB3.Text) > 55)
                    {
                        lblPan27.Text = "정밀검사요함";
                        txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    }
                    else
                    {
                        lblPan27.Text = "정상";
                        txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                }
                else
                {
                    if (double.Parse(txtB3.Text) > 45)
                    {
                        lblPan27.Text = "정밀검사요함";
                        txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    }
                    else
                    {
                        lblPan27.Text = "정상";
                        txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                }
            }
            else if (sender == txtB4)
            {
                if (txtB4.Text.Trim() == "")
                {
                    return;
                }
                if (double.Parse(txtB4.Text) > 45)
                {
                    lblPan28.Text = "정밀검사요함";
                    txtB4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    lblPan28.Text = "정상";
                    txtB4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtB5)
            {
                if (txtB5.Text.Trim() == "")
                {
                    return;
                }
                if (FstrSex == "F")
                {
                    if (double.Parse(txtB5.Text) >= 12)
                    {
                        lblPan29.Text = "정상";
                        txtB5.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                    else
                    {
                        lblPan29.Text = "정밀검사요함";
                        txtB5.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    }
                }
            }
            else if (sender == txtB6)
            {
                txtB6.Text = txtB6.Text.ToUpper();
            }
            else if (sender == txtBH)
            {
                if (txtBH.Text.Trim() == "")
                {
                    return;
                }
                if (double.Parse(txtBH.Text) >= 130)
                {
                    txtBH.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    if (lblPan23.Text != "고혈압 위험군" && lblPan23.Text != "고혈압 의심")
                    {
                        txtBH.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                }
                fn_Result_OverChk();
            }   
            else if (sender == txtBL)
            {
                if (txtBL.Text.Trim() == "")
                {
                    return;
                }
                if (double.Parse(txtBL.Text) >= 130)
                {
                    txtBL.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    if (lblPan23.Text != "고혈압 위험군" && lblPan23.Text != "고혈압 의심")
                    {
                        txtBL.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                }
                fn_Result_OverChk();
            }
            else if (sender == txtColor)
            {
                if (txtColor.Text.Trim() == "")
                {
                    return;
                }
                if (double.Parse(txtColor.Text) > 1)
                {
                    txtColor.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtColor.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
                lblPan2.Text = hf.READ_YN2("2", txtColor.Text.Trim());
            }
            else if (sender == txtE1)
            {
                if (double.Parse(txtE1.Text) <= 0.7 && txtE1.Text.Trim() != "")
                {
                    txtE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
            }
            else if (sender == txtE2)
            {
                if (double.Parse(txtE2.Text) <= 0.7 && txtE2.Text.Trim() != "")
                {
                    txtE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
            }
            else if (sender == txtEJ1)
            {
                if (double.Parse(txtEJ1.Text) > 1)
                {
                    txtEJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtEJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtEJ2)
            {
                if (double.Parse(txtEJ2.Text) > 1)
                {
                    txtEJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtEJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtH1)
            {
                if (double.Parse(txtH1.Text) > 1)
                {
                    txtH1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtH1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
                lblPanH1.Text = hf.READ_YN2("2", txtH1.Text.Trim());
            }
            else if (sender == txtH2)
            {
                if (double.Parse(txtH2.Text) > 1)
                {
                    txtH2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtH2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
                lblPanH2.Text = hf.READ_YN2("2", txtH2.Text.Trim());
            }
            else if (sender == txtHJ)
            {
                if (double.Parse(txtHJ.Text) > 1)
                {
                    txtHJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtHJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtJ0)
            {
                if (double.Parse(txtJ0.Text) > 1)
                {
                    txtJ0.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtJ0.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtJ1)
            {
                if (double.Parse(txtJ1.Text) > 1)
                {
                    txtJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtJ2)
            {
                if (double.Parse(txtJ2.Text) > 1)
                {
                    txtJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtJ3)
            {
                if (double.Parse(txtJ3.Text) > 1)
                {
                    txtJ3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtJ3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtLiver)
            {
                if (double.Parse(txtLiver.Text) > 1)
                {
                    txtLiver.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtLiver.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtM)
            {
                if (double.Parse(txtM.Text) > 1)
                {
                    txtM.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtM.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtN)
            {
                if (double.Parse(txtN.Text) > 1)
                {
                    txtN.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtN.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtNE2)
            {
                if (double.Parse(txtNE2.Text) <= 0.7 && txtNE2.Text.Trim() != "")
                {
                    txtNE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
            }
            else if (sender == txtOrgan0)
            {
                if (double.Parse(txtOrgan0.Text) > 1)
                {
                    txtOrgan0.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtOrgan0.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtOrgan1)
            {
                if (double.Parse(txtOrgan1.Text) > 1)
                {
                    txtOrgan1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtOrgan1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtOrgan2)
            {
                if (double.Parse(txtOrgan2.Text) > 1)
                {
                    txtOrgan2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtOrgan2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtOrgan3)
            {
                if (double.Parse(txtOrgan3.Text) > 1)
                {
                    txtOrgan3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtOrgan3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtOrgan4)
            {
                if (double.Parse(txtOrgan4.Text) > 1)
                {
                    txtOrgan4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtOrgan4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtOrgan5)
            {
                if (double.Parse(txtOrgan5.Text) > 1)
                {
                    txtOrgan5.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtOrgan5.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtPRes1)
            {
                if (double.Parse(txtPRes1.Text) > 1)
                {
                    txtPRes1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtPRes1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
                lblMsg.Text = hf.READ_Mush("2", txtPRes1.Text.Trim());
            }
            else if (sender == txtS)
            {
                if (double.Parse(txtS.Text) > 1)
                {
                    txtS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtU1)
            {
                if (double.Parse(txtU1.Text) > 1)
                {
                    txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
                lblPan12.Text = hf.READ_URO("2", txtU1.Text.Trim());
            }
            else if (sender == txtU2)
            {
                if (double.Parse(txtU2.Text) > 1)
                {
                    txtU2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtU2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
                lblPan13.Text = hf.READ_URO("2", txtU2.Text.Trim());
            }
            else if (sender == txtU3)
            {
                if (double.Parse(txtU3.Text) > 1)
                {
                    txtU3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtU3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            else if (sender == txtX)
            {
                if (double.Parse(txtX.Text) > 2)
                {
                    txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            txtName.Text = "";
            FstrEdit = "";

            for (int i = 4; i <= 12; i++)
            {
                SSList_Sheet1.Columns.Get(i).Visible = false;
            }
            for (int i = 7; i <= 9; i++)
            {
                SSList_Sheet1.Columns.Get(i).Visible = true;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            cboRH.Items.Clear();
            cboRH.Items.Add(" ");
            cboRH.Items.Add("1.RH+");
            cboRH.Items.Add("2.RH-");
            cboRH.SelectedIndex = 1;

            cboRH.Items.Clear();
            cboRH.Items.Add(" ");
            cboRH.Items.Add("1.정상");
            cboRH.Items.Add("2.정상(경계)");
            cboRH.Items.Add("3.정밀검사요함");
            cboRH.SelectedIndex = 0;

            this.Text = "학생검진판정 (" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            sp.Spread_All_Clear(SSList);
            fn_Screen_Clear();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSet1)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("1");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);

            }
            else if (sender == btnSet2)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("2");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
            }
            else if (sender == btnExam_result)
            {
                fn_Exam_Result_Show();
                FnHeight = double.Parse(txtHeight.Text);
                FnWeight = double.Parse(txtWeight.Text);
                if (FnHeight == 0)
                {
                    MessageBox.Show("신장 결과값 없음. 확인요망.", "혈압 및 비만도 자동판정불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                fn_Bp_AutoPanjeng();
                fn_ObesityLevel_AutoPangeng();
            }
            else if (sender == btnOK)
            {
                string strTEMP = "";
                string[] strPPanA = new string[4];
                string[] strPPanB = new string[2];
                string[] strPPanC = new string[9];
                string[] strPPanD = new string[6];
                string[] strPPanE = new string[4];
                string[] strPPanF = new string[6];
                string[] strPPanG = new string[1];
                string[] strPPanH = new string[1];
                string[] strPPanJ = new string[1];
                string[] strPPanK = new string[4];
                long nDPanDrno = 0;
                long nPPanDrno = 0;
                string strPanDate ="";
                string strGuBun = "";
                string strGbPan = "";
                string[] strPPanRemark = new string[2];
                string strJumin = "";
                string strSname = "";
                string strSex = "";
                string strSchool = "";
                string strLtdCode = "";
                string strJepDate = "";
                long nClass = 0;
                long nBan = 0;
                long nBun = 0;

                int result = 0;

                for (int i = 0; i <= 3; i++)
                {
                    strPPanA[i] = "";
                    strPPanK[i] = "";
                }

                for (int i = 0; i <= 1; i++)
                {
                    strPPanRemark[i] = "";
                }

                for (int i = 0; i <= 8; i++)
                {
                    strPPanC[i] = "";
                }

                for (int i = 0; i <= 5; i++)
                {
                    strPPanD[i] = "";
                }

                for (int i = 0; i <= 1; i++)
                {
                    strPPanG[i] = "";
                    strPPanH[i] = "";
                    strPPanJ[i] = "";
                }

                FstrROWID = hicSchoolNewService.GetRowIdbyWrtNo(long.Parse(txtWrtNo.Text));

                //접수내역에서 정보를 읽음
                HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));

                if (!list.IsNullOrEmpty())
                {
                    strJumin = clsAES.DeAES(list.JUMIN2.Trim());    //주민번호
                    strSname = list.SNAME.Trim();                   //성명
                    strSex = list.SEX.Trim();                       //성별
                    nClass = list.CLASS;                            //학년
                    nBan = list.BAN;                                //반
                    nBun = list.BUN;                                //번
                    strSchool = list.GBN.Trim();                    //학교구분
                    strLtdCode = list.LTDCODE.ToString();           //학교기호
                    strJepDate = list.JEPDATE.ToString();           //접수일자
                }
                else
                {
                    MessageBox.Show("접수내역이 없습니다!!", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_ERROR_CHECK();

                //신체사항
                strPPanA[0] = txtHeight.Text.Trim();
                strPPanA[1] = txtWeight.Text.Trim();
                strPPanA[2] = txtGrade1.Text.Trim();
                strPPanA[3] = txtSWeight.Text.Trim();
                //근골결및척추
                strPPanB[0] = txtPRes1.Text.Trim();
                strPPanB[1] = txtPResEtc.Text.Trim();
                //눈귀
                strPPanC[0] = txtNE1.Text.Trim() + "^^" + txtNE2.Text.Trim() +"^^";
                strPPanC[1] = txtE1.Text.Trim() + "^^" + txtE2.Text.Trim() + "^^";
                strPPanC[2] = txtColor.Text.Trim();
                strPPanC[3] = txtEJ1.Text.Trim() + "^^" + txtEJ2.Text.Trim() + "^^" + txtEJEtc.Text.Trim() + "^^";
                strPPanC[4] = txtH1.Text.Trim() + "^^" + txtH2.Text.Trim() + "^^";
                strPPanC[5] = txtHJ.Text.Trim() + "^^" + txtHJEtc.Text.Trim() + "^^";
                strPPanC[6] = txtM.Text.Trim() + "^^" + txtMEtc.Text.Trim() + "^^";
                strPPanC[7] = txtN.Text.Trim() + "^^" + txtNEtc.Text.Trim() + "^^";
                strPPanC[8] = txtS.Text.Trim() + "^^" + txtSEtc.Text.Trim() + "^^";
                //기관능력
                strPPanD[0] = txtOrgan0.Text.Trim() + "^^" + txtOrgan0Etc.Text.Trim() + "^^";
                strPPanD[1] = txtOrgan1.Text.Trim() + "^^" + txtOrgan1Etc.Text.Trim() + "^^";
                strPPanD[2] = txtOrgan2.Text.Trim() + "^^" + txtOrgan2Etc.Text.Trim() + "^^";
                strPPanD[3] = txtOrgan3.Text.Trim() + "^^" + txtOrgan3Etc.Text.Trim() + "^^";
                strPPanD[4] = txtOrgan4.Text.Trim() + "^^" + txtOrgan4Etc.Text.Trim() + "^^";
                strPPanD[5] = txtOrgan5.Text.Trim() + "^^" + txtOrgan5Etc.Text.Trim() + "^^";
                //소변
                strPPanE[0] = txtU1.Text.Trim(); strPPanE[1] = txtU2.Text.Trim();
                strPPanE[2] = txtU3.Text.Trim(); strPPanE[3] = txtU4.Text.Trim();
                //혈액
                strPPanF[0] = txtB1.Text.Trim(); strPPanF[1] = txtB2.Text.Trim();
                strPPanF[2] = txtB3.Text.Trim(); strPPanF[3] = txtB4.Text.Trim();
                strPPanF[4] = txtB5.Text.Trim();
                strPPanF[5] = txtB6.Text.Trim() + "^^" + cboRH.Text.Trim() + "^^";
                //흉부
                strPPanG[0] = txtX.Text.Trim() + "^^" + txtXEtc.Text.Trim() + "^^";
                //간염
                strPPanH[0] = txtLiver.Text.Trim();
                //혈압
                strPPanJ[0] = txtBH.Text.Trim() + "^^" + txtBL.Text.Trim() + "^^";
                //진촬상담
                strPPanK[0] = txtJ0.Text.Trim() + "^^" + txtJEtc0.Text.Trim() + "^^";
                strPPanK[1] = txtJ1.Text.Trim() + "^^" + txtJEtc1.Text.Trim() + "^^";
                strPPanK[2] = txtJ2.Text.Trim() + "^^" + txtJEtc2.Text.Trim() + "^^";
                strPPanK[3] = txtJ3.Text.Trim() + "^^" + txtJEtc3.Text.Trim() + "^^";

                strPPanRemark[0] = txtPanSogen.Text.Trim().Replace("'", "`");
                strPPanRemark[1] = txtPanJochi.Text.Trim().Replace("'", "`");

                strGbPan = VB.Left(cboPan.Text, 1);

                nPPanDrno = long.Parse(txtPanDrNo.Text);
                //판정일
                strPanDate = txtPanDate.Text.Trim();

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_SCHOOL_NEW item = new HIC_SCHOOL_NEW();

                item.WRTNO = long.Parse(txtWrtNo.Text);
                item.JUMIN = VB.Left(strJumin, 7);
                item.JUMIN2 = clsAES.AES(strJumin);
                item.SNAME = strSname;
                item.SDATE = strJepDate;
                item.RDATE = strPanDate;
                item.SEX = strSex;
                item.GBN = strSchool;
                item.CLASS = nClass;
                item.BAN = nBan;
                item.BUN = nBun;
                item.LTDCODE = long.Parse(strLtdCode);
                item.PPANA1 = strPPanA[0];
                item.PPANA2 = strPPanA[1];
                item.PPANA3 = strPPanA[2];
                item.PPANA4 = strPPanA[3];
                item.PPANB1 = strPPanB[0];
                item.PPANB2 = strPPanB[1];
                item.PPANC1 = strPPanC[0];
                item.PPANC2 = strPPanC[1];
                item.PPANC3 = strPPanC[2];
                item.PPANC4 = strPPanC[3];
                item.PPANC5 = strPPanC[4];
                item.PPANC6 = strPPanC[5];
                item.PPANC7 = strPPanC[6];
                item.PPANC8 = strPPanC[7];
                item.PPANC9 = strPPanC[8];
                item.PPAND1 = strPPanD[0];
                item.PPAND2 = strPPanD[1];
                item.PPAND3 = strPPanD[2];
                item.PPAND4 = strPPanD[3];
                item.PPAND5 = strPPanD[4];
                item.PPAND6 = strPPanD[5];
                item.PPANE1 = strPPanE[0];
                item.PPANE2 = strPPanE[1];
                item.PPANE3 = strPPanE[2];
                item.PPANE4 = strPPanE[3];
                item.PPANF1 = strPPanF[0];
                item.PPANF2 = strPPanF[1];
                item.PPANF3 = strPPanF[2];
                item.PPANF4 = strPPanF[3];
                item.PPANF5 = strPPanF[4];
                item.PPANF6 = strPPanF[5];
                item.PPANG1 = strPPanG[0];
                item.PPANH1 = strPPanH[0];
                item.PPANJ1 = strPPanJ[0];
                item.PPANK1 = strPPanK[0];
                item.PPANK2 = strPPanK[1];
                item.PPANK3 = strPPanK[2];
                item.PPANK4 = strPPanK[3];
                item.PPANREMARK1 = strPPanRemark[0];
                item.PPANREMARK2 = strPPanRemark[1];
                item.PPANDRNO = nPPanDrno;
                item.GBPAN = strGbPan;
                item.ENTSABUN = long.Parse(clsType.User.IdNumber);

                if (FstrROWID == "")
                {
                    result = hicSchoolNewService.Insert(item);
                }
                else
                {
                    result = hicSchoolNewService.UpdatebyRowId(item);
                }

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("DB에 자료를 등록시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result = hicJepsuService.UpdatePanjengDrNoPanDatebyWrtNo(long.Parse(txtWrtNo.Text), nPPanDrno, strPanDate);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("DB에 자료를 등록시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
            }
            else if (sender == btnPacs)
            {
                FrmViewResult = new frmViewResult(FstrPano);
                FrmViewResult.ShowDialog();
            }
            else if (sender == btnSang)
            {
                FrmHcSangStudentCounsel = new frmHcSangStudentCounsel(FnWRTNO);
                FrmHcSangStudentCounsel.ShowDialog();
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                int nRow = 0;
                string strOK = "";
                string strJepDate = "";
                string strJumin = "";
                string strGubun = "";

                if (rdoGubun1.Checked == true)
                {
                    strGubun = "1";
                }
                else
                {
                    strGubun = "2";
                }

                fn_Screen_Clear();

                SSList.ActiveSheet.RowCount = 0;

                HIC_JEPSU_PATIENT_SCHOOL_SANGDAM item = new HIC_JEPSU_PATIENT_SCHOOL_SANGDAM();

                item.JEPFRDATE = dtpFrDate.Text;
                item.JEPTODATE = dtpToDate.Text;
                item.SNAME = txtName.Text.Trim();
                item.LTDCODE = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                item.SANGDAMDRNO = clsHcVariable.GnHicLicense;

                List<HIC_JEPSU_PATIENT_SCHOOL_SANGDAM> list = hicJepsuPatientSchoolSangdamService.GetItembyJepDate(item, strGubun);

                nRead = list.Count;
                SSList.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "";
                    strJumin = clsAES.DeAES(list[i].JUMIN2.Trim());
                    SSList.ActiveSheet.Cells[i, 0].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE.ToString();
                    strJepDate = list[i].JEPDATE.ToString();
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 4].Text = strJumin;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].SEX.Trim();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].LTDCODE.ToString();
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].CLASS.Trim();
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].BAN;
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].BUN;
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].AGE;
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].ROWID.Trim();
                    SSList.ActiveSheet.Cells[i, 12].Text = list[i].PTNO.Trim();

                    if (list[i].RDATE.ToString() != "" && list[i].GBPAN.Trim() != "")
                    {
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                    }

                    //====인터넷 문진Data 조회====
                    if (hicIeMunjinNewService.GetCountbyJumin(clsAES.AES(strJumin), strJepDate) > 0)
                    {
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(200, 255, 200);
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HF9E9FE"));
                    }                    
                }
            }
            else if (sender == btnXrayResult)
            {
                string strJepDate = "";
                string strSname = "";
                string[] strExCode = { "A142" };

                strSname = ssPatInfo.ActiveSheet.Cells[0, 3].Text.Trim();
                strJepDate = ssPatInfo.ActiveSheet.Cells[1, 1].Text.Trim();

                //흉부방사선 촬영검사가 없다면 빠져나감 ...
                if (hicResultService.GetCountbyWrtNo(FnWRTNO, strExCode) == 0)
                {
                    txtXResult.Text = "해당검사 없음";
                    return;
                }

                //해당 판독번호로 판독결과를 READ
                HIC_XRAY_RESULT list = hicXrayResultService.GetItembyJepDatePtNo(strJepDate, FstrPano);

                if (list.IsNullOrEmpty())
                {
                    MessageBox.Show("해당 수검자의 방사선 Data가 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (list.RESULT1.Trim() == "")
                {
                    txtXResult.Text = "\r\n\r\n\r\n";
                    txtXResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                }
                else
                {
                    switch (list.RESULT2.Trim())
                    {
                        case "A":
                            txtX.Text = "1";
                            break;
                        case "B":
                            txtX.Text = "2";
                            break;
                        case "C":
                            txtX.Text = "3";
                            break;
                        case "D-A":
                            txtX.Text = "4";
                            break;
                        case "D-B":
                            txtX.Text = "5";
                            break;
                        case "D-C":
                            txtX.Text = "6";
                            break;
                        case "E":
                            txtX.Text = "7";
                            break;
                        default:
                            break;
                    }

                    lblPan14.Text = hf.READ_Xray("2", txtX.Text.Trim());

                    txtXResult.Text = "판독분류: " + list.RESULT2 + "\r\n";
                    txtXResult.Text += "판독분류명: " + list.RESULT3 + "\r\n";
                    txtXResult.Text += "판독소견: " + list.RESULT4 + "\r\n";
                }
            }
            else if (sender == btnMenuResult)
            {
                ///TODO : 이상훈(2020.02.05) FrmResult(김민철) 개발 완료 후 주석 해제
                //FrmResult f = new FrmResult();
                //f.ShowDialog();
            }
        }

        void fn_Screen_Clear()
        {
            FstrROWID = "";
            FstrSex = "";
            FnAge = 0; FnHeight = 0; FnWeight = 0;
            FstrFlag = "";
            pnlExam.Enabled = false;

            btnOK.Enabled = false;
            if (clsHcVariable.GnHicLicense > 0)
            {
                btnOK.Enabled = true;
            }

            txtWrtNo.Text = "";
            lblMsg.Text = "";

            ssPatInfo.ActiveSheet.Cells[0, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = "";

            ssPatInfo.ActiveSheet.Cells[1, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 5].Text = "";

            //결과 및 판정
            for (int i = 0; i <= 29; i++)
            {
                Label lblPan = (Controls.Find("lblPan" + i.ToString(), true)[0] as Label);
                lblPan.Text = ""; 
            }

            txtHeight.Text = ""; txtWeight.Text = "";
            txtGrade1.Text = ""; txtSWeight.Text = "";
            txtPRes1.Text = "";
            txtPResEtc.Text = "";
            txtNE1.Text = ""; txtNE2.Text = "";
            txtE1.Text = ""; txtE2.Text = "";
            txtColor.Text = "";
            txtEJ1.Text = ""; txtEJ2.Text = ""; txtEJEtc.Text = "";
            txtH1.Text = ""; txtH2.Text = "";
            txtHJ.Text = ""; txtHJEtc.Text = "";
            txtM.Text = ""; txtMEtc.Text = "";
            txtN.Text = ""; txtNEtc.Text = "";
            txtS.Text = ""; txtSEtc.Text = "";
            for (int i = 0; i <= 5; i++)
            {
                TextBox txtOrgan = (Controls.Find("txtOrgan" + i.ToString(), true)[0] as TextBox);
                txtOrgan.Text = "";
            }
            txtOrgan1Etc.Text = ""; txtOrgan2Etc.Text = ""; txtOrgan3Etc.Text = "";
            txtOrgan4Etc.Text = ""; txtOrgan5Etc.Text = ""; txtOrgan0Etc.Text = "";
            txtU1.Text = ""; txtU2.Text = ""; txtU3.Text = ""; txtU4.Text = "";
            txtB1.Text = ""; txtB2.Text = ""; txtB3.Text = ""; txtB4.Text = "";
            txtB5.Text = ""; txtB6.Text = "";
            txtX.Text = ""; txtXEtc.Text = "";
            txtLiver.Text = "";
            txtBH.Text = ""; txtBL.Text = "";
            for (int i = 0; i <= 3; i++)
            {
                TextBox txtJ = (Controls.Find("txtJ" + i.ToString(), true)[0] as TextBox);
                TextBox txtJEtc = (Controls.Find("txtJEtc" + i.ToString(), true)[0] as TextBox);
                txtJ.Text = "";
                txtJEtc.Text = "";
            }
            txtPanSogen.Text = ""; txtPanJochi.Text = "";
            txtPanDate.Text = "";
            txtPanDrNo.Text = ""; lblDrName.Text = "";

            cboRH.SelectedIndex = 0;
            cboPan.SelectedIndex = 0;

            txtRemark.Text = "";
            txtXResult.Text = "";

            fn_Result_OverChk();
        }

        /// <summary>
        /// 판정하기전 에러부분 체크
        /// </summary>
        void fn_ERROR_CHECK()
        {
            if (txtWrtNo.Text.Trim() == "")
            {
                MessageBox.Show("해당환자를 먼저 선택하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboPan.Text.Trim() == "")
            {
                MessageBox.Show("판정란이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //흉부방사선 검사 있는 수검자만 확인함
            if (hicResultService.GetExCodebyWrtNoExCode(long.Parse(txtWrtNo.Text)) > 0)
            {
                if (txtX.Text.Trim() == "")
                {
                    MessageBox.Show("흉부방사선검사 결과란이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        void fn_Result_OverChk()
        {
            nPan1 = 0;
            nPan2 = 0;

            //신장
            if (txtHeight.Text.Trim() == "")
            {
                txtHeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //체질량지수
            if (double.Parse(txtGrade1.Text) > 1)
            {
                txtGrade1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                FstrFlag = "OK";
            }
            else
            {
                txtGrade1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //상대체중
            if (double.Parse(txtSWeight.Text) > 1)
            {
                txtSWeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                FstrFlag = "OK";
            }
            else
            {
                txtSWeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //근골격
            if (double.Parse(txtPRes1.Text) > 1)
            {
                txtPRes1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtPRes1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPan22.Text = hf.READ_Mush("2", txtPRes1.Text);

            //시력(나안/좌)
            if (double.Parse(txtNE1.Text) <= 0.7 && double.Parse(txtNE1.Text) != 0)
            {
                txtNE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtNE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //시력(나안/우)
            if (double.Parse(txtNE2.Text) <= 0.7 && double.Parse(txtNE2.Text) != 0)
            {
                txtNE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtNE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //시력(교정/좌)
            if (double.Parse(txtE1.Text) <= 0.7 && double.Parse(txtE1.Text) != 0)
            {
                txtE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //시력(교정/우)
            if (double.Parse(txtE2.Text) <= 0.7 && double.Parse(txtE2.Text) != 0)
            {
                txtE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //청력(우)
            if (double.Parse(txtH1.Text) > 1)
            {
                txtH1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtH1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPanH2.Text = hf.READ_YN2("2", txtH2.Text.Trim());

            //청력(좌)
            if (double.Parse(txtH2.Text) > 1)
            {
                txtH2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtH2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPanH1.Text = hf.READ_YN2("2", txtH1.Text.Trim());

            //색각
            if (double.Parse(txtColor.Text) > 1)
            {
                txtColor.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtColor.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPan2.Text = hf.READ_YN2("2", txtColor.Text.Trim());

            //혈압
            if (lblPan23.Text == "정상(경계)")
            {
                nPan1 += 1;
            }
            else if (lblPan23.Text == "정밀검사요함")
            {
                nPan2 += 1;
            }
            else
            {
                txtBH.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                txtBL.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //요단백검사
            if (double.Parse(txtU1.Text) > 2)
            {
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPan12.Text = hf.READ_URO("2", txtU1.Text.Trim());

            //요잠혈
            if (double.Parse(txtU2.Text) > 2)
            {
                txtU2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtU2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPan13.Text = hf.READ_URO("2", txtU2.Text.Trim());

            //혈당
            if (double.Parse(txtB1.Text) >= 100 && double.Parse(txtB1.Text) <= 125)
            {
                txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan1 += 1;
                lblPan25.Text = "정상(경계)";
            }
            else if (double.Parse(txtB1.Text) >= 126)
            {
                txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan25.Text = "정밀검사요함";
            }
            else
            {
                txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan25.Text = "정상";
            }

            //콜레스테롤
            if (double.Parse(txtB2.Text) > 170 && double.Parse(txtB2.Text) < 200)
            {
                txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan26.Text = "정상(경계)";
            }
            else if (double.Parse(txtB2.Text) >= 200)
            {
                txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan26.Text = "정밀검사요함";
            }
            else
            {
                txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan26.Text = "정상";
            }

            //AST
            if (FnAge < 10)
            {
                if (double.Parse(txtB3.Text) > 55)
                {
                    txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    nPan2 += 1;
                    lblPan27.Text = "정밀검사요함";
                }
                else
                {
                    txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    lblPan27.Text = "정상";
                }
            }
            else
            {
                if (double.Parse(txtB3.Text) > 45)
                {
                    txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    nPan2 += 1;
                    lblPan27.Text = "정밀검사요함";
                }
                else
                {
                    txtB3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    lblPan27.Text = "정상";
                }
            }

            //ALT
            if (double.Parse(txtB4.Text) > 45)
            {
                txtB4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan27.Text = "정밀검사요함";
            }
            else
            {
                txtB4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan27.Text = "정상";
            }

            //혈색소 12이상 정상
            if (FstrSex == "F") //여자만 해당
            {
                if (double.Parse(txtB5.Text) >= 12 || double.Parse(txtB5.Text) == 0)
                {
                    txtB5.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    lblPan29.Text = "정상";
                }
                else
                {
                    txtB5.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    nPan2 += 1;
                    lblPan29.Text = "정밀검사요함";
                }
            }

            //방사선
            if (double.Parse(txtX.Text) >= 4 && double.Parse(txtX.Text) <= 9)
            {
                txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //안질환
            if (double.Parse(txtEJ1.Text) > 1)
            {
                txtEJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtEJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            if (double.Parse(txtEJ2.Text) > 1)
            {
                txtEJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtEJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //귓병
            if (double.Parse(txtHJ.Text) > 1)
            {
                txtHJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtHJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //콧병
            if (double.Parse(txtM.Text) > 1)
            {
                txtM.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtM.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //목병
            if (double.Parse(txtN.Text) > 1)
            {
                txtN.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }

            //피부
            if (double.Parse(txtS.Text) > 1)
            {
                txtS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //기관
            for (int i = 0; i <= 5; i++)
            {
                TextBox txtOrgan = (Controls.Find("txtOrgan" + i.ToString(), true)[0] as TextBox);
                if (double.Parse(txtOrgan.Text) == 2)
                {
                    txtOrgan.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    nPan1 += 1;
                }
                else if (double.Parse(txtOrgan.Text) == 3)
                {
                    txtOrgan.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                    nPan2 += 1;
                }
                else
                {
                    txtOrgan.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            for (int i = 0; i <= 3; i++)
            {
                TextBox txtJ = (Controls.Find("txtJ" + i.ToString(), true)[0] as TextBox);
                if (double.Parse(txtJ.Text) > 1)
                {
                    txtJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (double.Parse(txtJ.Text) == 3)
                {
                    txtJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }

            //간염
            if (double.Parse(txtLiver.Text) > 1)
            {
                txtLiver.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                FstrFlag = "OK";
            }
            else
            {
                txtLiver.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            lblPan15.Text = hf.READ_PM("2", txtLiver.Text.Trim());

            //요당
            if (double.Parse(txtU3.Text) > 1)
            {
                txtU3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                FstrFlag = "OK";
            }
            else
            {
                txtU3.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            if (rdoGubun1.Checked == true)
            {
                if (nPan2 == 0 && nPan1 == 0)
                {
                    cboPan.SelectedIndex = 1;
                }
                else
                {
                    cboPan.SelectedIndex = 0;
                }
            }
        }

        void eCboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == cboPan || sender == cboRH)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtPanDate)
            {
                frmCalendar2 frmCalendar2X = new frmCalendar2();
                clsPublic.GstrCalDate = "";
                frmCalendar2X.ShowDialog();
                frmCalendar2X.Dispose();
                frmCalendar2X = null;

                txtPanDate.Text = clsPublic.GstrCalDate;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            string strJumin = "";
            string strSname = "";
            string strSex = "";
            string strSchool = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strSchoolName = "";
            long nClass = 0;
            long nBan = 0;
            long nBun = 0;
            long nAge = 0;
            bool boolSort = false;

            if (e.RowHeader == true && e.ColumnHeader)
            {
                if (FstrEdit == "")
                {
                    boolSort = false;
                    clsSpread.gSpdSortRow(SSList, e.Column, ref boolSort, true);
                }
                else
                {
                    boolSort = true;
                    clsSpread.gSpdSortRow(SSList, e.Column, ref boolSort, true);
                }
                return;
            }

            if (e.RowHeader)
            {
                return;
            }

            fn_Screen_Clear();

            strSchoolName = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            strSname = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            strJepDate = SSList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
            FnWRTNO = long.Parse(txtWrtNo.Text);
            strJumin = VB.Left(SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim(), 6) + "-" + VB.Right(SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim(), 7);
            strSex = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();
            FstrSex = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();

            nClass = long.Parse(SSList.ActiveSheet.Cells[e.Row, 7].Text);
            nBan = long.Parse(SSList.ActiveSheet.Cells[e.Row, 8].Text);
            nBun = long.Parse(SSList.ActiveSheet.Cells[e.Row, 9].Text);
            nAge = long.Parse(SSList.ActiveSheet.Cells[e.Row, 10].Text);
            FnAge = nAge;
            FstrROWID = SSList.ActiveSheet.Cells[e.Row, 11].Text;
            FstrPano = SSList.ActiveSheet.Cells[e.Row, 12].Text;
            FstrJepDate = strJepDate;
            FstrGjJong = "56";

            //인적사항
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = txtWrtNo.Text.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = strSname + "(" + nAge + "/" + strSex + ")";
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = strJumin;

            ssPatInfo.ActiveSheet.Cells[1, 1].Text = strJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = strSchoolName;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = nClass + "학년 " + nBan + "반 " + nBun + "번";

            //판독결과 조회
            eBtnClick(btnXrayResult, new EventArgs());

            if (rdoGubun1.Checked == true)  //신규
            {
                eBtnClick(btnExam_result, new EventArgs());
                fn_Sangdam_Display(long.Parse(txtWrtNo.Text));
                txtPanDate.Text = clsHcVariable.GnHicLicense.ToString();
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                txtPanDate.Text = clsPublic.GstrSysDate;
            }
            else
            {
                fn_Screen_Display(long.Parse(txtWrtNo.Text));
                FnHeight = double.Parse(txtHeight.Text);
                FnWeight = double.Parse(txtWeight.Text);
                FnAge = nAge;
                FstrSex = strSex;
                fn_Bp_AutoPanjeng();
                fn_ObesityLevel_AutoPangeng();
            }
            pnlExam.Enabled = true;

            //정상범위 체크
            fn_Result_OverChk();

            if (rdoGubun1.Checked == true)
            {
                if (cboPan.SelectedIndex == 1)
                {
                    txtPanJochi.Text = "♣ 학생건강검사결과 특별한 이상소견 없습니다. 계속 건강을 유지하십시오.";
                    txtPanSogen.Text = "정상소견입니다";
                    cboPan.SelectedIndex = 1;
                }
            }

            //문진뷰어
            if (chkMunjin.Checked == false)
            {
                FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView();
                FrmHcSangInternetMunjinView.StartPosition = FormStartPosition.CenterParent;
                FrmHcSangInternetMunjinView.ShowDialog(this);
            }

            if (rdoGubun2.Checked == false)
            {
                fn_Auto_Result_Macro();
            }
        }

        void fn_Exam_Result_Show()
        {
            string strMsg = "";
            string strTemp1 = "";
            string strTemp2 = "";

            //검사결과불러오기
            List<HIC_RESULT> list = hicResultService.GetExCodeResultbyOnlyWrtNo(long.Parse(txtWrtNo.Text));

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strTemp1 = list[i].EXCODE.Trim();
                    strTemp2 = list[i].RESULT.Trim();
                    if (strTemp1 == "A101")
                    {
                        txtHeight.Text = long.Parse(list[i].RESULT).ToString();
                        strMsg += " 키, ";
                    }
                    if (strTemp1 == "A102")
                    {
                        txtHeight.Text = long.Parse(list[i].RESULT).ToString();
                        strMsg += " 몸무게, ";
                    }
                    if (strTemp1 == "A117") //체질량지수
                    {
                        strTemp2 = list[i].RESULT.Trim();
                        if (long.Parse(strTemp2) < 25)
                        {
                            txtGrade1.Text = "1";
                        }
                        else if (long.Parse(strTemp2) >= 25 && long.Parse(strTemp2) < 30)
                        {
                            txtGrade1.Text = "2";
                        }
                        else if (long.Parse(strTemp2) >= 30)
                        {
                            txtGrade1.Text = "3";
                        }
                        strMsg += " 체질량지수, ";
                    }

                    if (strTemp1 == "A104") //시력(좌)
                    {
                        if (VB.Left(strTemp2, 1) == "(")
                        {
                            strTemp2 = VB.Left(strTemp2, strTemp2.Length - 1);
                            strTemp2 = VB.Right(strTemp2, strTemp2.Length - 1);
                            txtE1.Text = strTemp2;
                        }
                        else
                        {
                            txtNE1.Text = strTemp2;
                        }
                        strMsg += " 시력(좌), ";
                    }
                    if (strTemp1 == "A105") //시력(우)
                    {
                        if (VB.Left(strTemp2, 1) == "(")
                        {
                            strTemp2 = VB.Left(strTemp2, strTemp2.Length - 1);
                            strTemp2 = VB.Right(strTemp2, strTemp2.Length - 1);
                            txtE1.Text = strTemp2;
                        }
                        else
                        {
                            txtNE2.Text = strTemp2;
                        }
                        strMsg += " 시력(우), ";
                    }
                    //청력(좌)
                    if (strTemp1 == "A106")
                    {
                        if (list[i].RESULT.Trim() == "정상")
                        {
                            txtH1.Text = "1";
                        }
                        else if (list[i].RESULT.Trim() == "측정불가")
                        {
                            txtH1.Text = "3";
                        }
                        else if (list[i].RESULT.Trim() == "")
                        {
                            txtH1.Text = "2";
                        }
                        strMsg += " 청력(좌), ";
                    }

                    //청력(우)
                    if (strTemp1 == "A107")
                    {
                        if (list[i].RESULT.Trim() == "정상")
                        {
                            txtH2.Text = "1";
                        }
                        else if (list[i].RESULT.Trim() == "측정불가")
                        {
                            txtH2.Text = "2";
                        }
                        else if (list[i].RESULT.Trim() == "")
                        {
                            txtH2.Text = "3";
                        }
                        else
                        {
                            txtH2.Text = "";
                        }
                        strMsg = " 청력(우), ";
                    }
                    
                    //색각검사
                    if (strTemp1 == "TE05")
                    {
                        if (strTemp2 == "정상")
                        {
                            txtColor.Text = "1";
                        }
                        else if (strTemp2 == "측정불가")
                        {
                            txtColor.Text = "3";
                        }
                        else if (strTemp2 == "")
                        {
                            txtColor.Text = "";
                        }
                        else
                        {
                            txtColor.Text = "2";
                        }
                    }
                    if (strTemp1 == "A108") txtBH.Text = list[i].RESULT.Trim(); strMsg += " 혈압(고), ";
                    if (strTemp1 == "A109") txtBL.Text = list[i].RESULT.Trim(); strMsg += " 혈압(저), ";
                    if (strTemp1 == "A112") txtU1.Text = list[i].RESULT.Trim(); strMsg += " 요단백, ";
                    if (strTemp1 == "A113") txtU2.Text = list[i].RESULT.Trim(); strMsg += " 요잠혈, ";
                    if (strTemp1 == "A122") txtB1.Text = list[i].RESULT.Trim(); strMsg += " 혈당(식전), ";
                    if (strTemp1 == "A123") txtB2.Text = list[i].RESULT.Trim(); strMsg += " 총콜레스테롤, ";
                    if (strTemp1 == "A124") txtB3.Text = list[i].RESULT.Trim(); strMsg += " AST, ";
                    if (strTemp1 == "A125") txtB4.Text = list[i].RESULT.Trim(); strMsg += " ALT, ";
                    if (strTemp1 == "A121") txtB5.Text = list[i].RESULT.Trim(); strMsg += " 혈색소, ";

                    //혈액형
                    if (strTemp1 == "H840")
                    {
                        switch (long.Parse(list[i].RESULT))
                        {
                            case 1:
                                txtB6.Text = "A";
                                strMsg += " 혈액형검사, ";
                                break;
                            case 2:
                                txtB6.Text = "B";
                                strMsg += " 혈액형검사, ";
                                break;
                            case 3:
                                txtB6.Text = "O";
                                strMsg += " 혈액형검사, ";
                                break;
                            case 4:
                                txtB6.Text = "AB";
                                strMsg += " 혈액형검사, ";
                                break;
                            default:
                                break;
                        }
                    }

                    if (strTemp1 == "H841")
                    {
                        switch (long.Parse(list[i].RESULT))
                        {
                            case 1:
                                cboRH.Text = "1.RH+";
                                strMsg += " 혈액형RH, ";
                                break;
                            case 2:
                                cboRH.Text = "2.RH-";
                                strMsg += " 혈액형RH, ";
                                break;
                            default:
                                break;
                        }
                    }

                    if (strTemp1 == "A131")
                    {
                        txtLiver.Text = list[i].RESULT;
                        strMsg += " 간염검사, ";
                    }

                    if (strTemp1 == "A154")
                    {
                        txtXEtc.Text = list[i].RESULT;
                    }
                }
            }
        }

        /// <summary>
        /// 혈압자동판정
        /// </summary>
        void fn_Bp_AutoPanjeng()
        {
            int n1 = 0;
            int n2 = 0;
            int nStart = 0;
            string strTGrade = ""; //신장등급
            string strHGrade = ""; //수축기등급
            string strLGrade = ""; //이완기등급
            int nCurRow = 0;

            //신장 등급 산출 -------------------------------------
            if (FstrSex == "M")
            {
                for (int i = 1; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (FnAge == 6)
                    {
                        FnAge = 7;
                    }
                    if (FnAge == long.Parse(SS1.ActiveSheet.Cells[i, 0].Text))
                    {
                        nCurRow = i;
                        break;
                    }
                }
                for (int i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
                {
                    if (FnHeight < double.Parse(SS1.ActiveSheet.Cells[nCurRow, i].Text))
                    {
                        if ( i == 1)
                        {
                            i += 1;
                        }
                        strTGrade = SS1.ActiveSheet.Cells[0, i - 1].Text.Trim();
                        break;
                    }
                    if (i == SS1.ActiveSheet.ColumnCount - 1)
                    {
                        strTGrade = "97";
                    }
                }
            }
            else
            {
                for (int i = 1; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (FnAge == 6)
                    {
                        FnAge = 7;
                    }
                    if (FnAge == long.Parse(SS2.ActiveSheet.Cells[i, 0].Text))
                    {
                        nCurRow = i;
                        break;
                    }
                }
                for (int i = 1; i < SS2.ActiveSheet.ColumnCount; i++)
                {
                    if (FnHeight < double.Parse(SS2.ActiveSheet.Cells[nCurRow, i].Text))
                    {
                        if (i == 1)
                        {
                            i += 1;
                        }
                        strTGrade = SS2.ActiveSheet.Cells[0, i - 1].Text.Trim();
                        break;
                    }
                    if (i == SS2.ActiveSheet.ColumnCount - 1)
                    {
                        strTGrade = "97";
                    }
                }
            }

            //나이 범위 및 남여 구간 세팅---------------------------
            switch (FnAge)
            {
                case 7:
                    nStart = 1;
                    break;
                case 8:
                    nStart = 5;
                    break;
                case 9:
                    nStart = 9;
                    break;
                case 10:
                    nStart = 13;
                    break;
                case 11:
                    nStart = 17;
                    break;
                case 12:
                    nStart = 21;
                    break;
                case 13:
                    nStart = 25;
                    break;
                case 14:
                    nStart = 29;
                    break;
                case 15:
                    nStart = 33;
                    break;
                case 16:
                    nStart = 37;
                    break;
                case 17:
                    nStart = 41;
                    break;
                case 18:
                    nStart = 45;
                    break;
                default:
                    break;
            }

            n1 = 0;
            n2 = 0;

            if (FstrSex == "M")
            {
                n1 = 3;
                n2 = 9;
            }

            if (FstrSex == "F")
            {
                n1 = 10;
                n2 = 16;
            }

            //신장등급이 5미만이면 5급, 95초과이면 95등급으로 간주
            if (strTGrade == "97")
            {
                strTGrade = "95";
            }

            //수축기 등급 산출 -----------------------------------
            nCurRow = 0;
            for (int i = n1; i <= n2; i++)
            {
                if (SS3.ActiveSheet.Cells[nCurRow, i].Text.Trim() == strTGrade)
                {
                    for (int j = nStart; j <= nStart + 3; j++)
                    {
                        if (double.Parse(txtBH.Text.Trim()) < double.Parse(SS3.ActiveSheet.Cells[j, i].Text))
                        {
                            if (j == nStart)
                            {
                                strHGrade = SS3.ActiveSheet.Cells[j, 1].Text.Trim();
                                break;
                            }
                            else if (j != nStart)
                            {
                                strHGrade = SS3.ActiveSheet.Cells[j - 1, 1].Text.Trim();
                                break;
                            }
                        }
                        else if (double.Parse(txtBH.Text.Trim()) == double.Parse(SS3.ActiveSheet.Cells[j, 1].Text.Trim()))
                        {
                            strHGrade = SS3.ActiveSheet.Cells[j, 1].Text.Trim();
                            break;
                        }
                        if (j == (nStart + 3))
                        {
                            strHGrade = "99";
                            break;
                        }
                    }
                }
            }

            //이완기 등급 산출 -----------------------------------
            for (int i = n1; i <= n2; i++)
            {
                if (SS4.ActiveSheet.Cells[0, i].Text.Trim() == strTGrade)
                {
                    for (int j = 0; j <= nStart + 3; j++)
                    {
                        if (double.Parse(txtBL.Text.Trim()) < double.Parse(SS4.ActiveSheet.Cells[j, i].Text.Trim()))
                        {
                            if (j == nStart)
                            {
                                strLGrade = SS4.ActiveSheet.Cells[j, i].Text.Trim();
                                break;
                            }
                            else if (j != nStart)
                            {
                                strLGrade = SS4.ActiveSheet.Cells[j - 1, i].Text.Trim();
                                break;
                            }
                        }
                        else if (double.Parse(txtBL.Text.Trim()) == double.Parse(SS4.ActiveSheet.Cells[j, i].Text.Trim()))
                        {
                            strLGrade = SS4.ActiveSheet.Cells[i, 1].Text.Trim();
                            break;
                        }
                        if (j == (nStart + 3))
                        {
                            strLGrade = "99";
                            break;
                        }
                    }
                }
            }

            if (double.Parse(strHGrade) >= 90)
            {
                txtBH.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                //txtBH.BorderBackColor = &H8080FF;    
            }

            if (double.Parse(strLGrade) >= 90)
            {
                txtBL.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                //txtBL.BorderBackColor = &H8080FF;
            }

            //혈압 판정 --------------------------------------------
            lblPan23.Text = "정상";

            if (double.Parse(txtBH.Text) >= 130 || double.Parse(txtBL.Text) >= 80)
            {
                lblPan23.Text = "정상(경계)";
            }

            if (long.Parse(strHGrade) >= 90 && double.Parse(strHGrade) <= 95)
            {
                lblPan23.Text = "정상(경계)";
            }

            if (long.Parse(strHGrade) >= 90 && double.Parse(strLGrade) <= 95)
            {
                lblPan23.Text = "정상(경계)";
            }

            if (long.Parse(strHGrade) > 95 && double.Parse(strLGrade) > 95)
            {
                lblPan23.Text = "정밀검사요함";
            }
        }

        /// <summary>
        /// 비만도자동판정
        /// </summary>
        void fn_ObesityLevel_AutoPangeng()
        {
            string strPan = "";
            string strBiman = "";
            double nBMI = 0;
            double nHEIGHT = 0;

            if (double.Parse(txtHeight.Text) == 0 || double.Parse(txtWeight.Text) == 0)
            {
                MessageBox.Show("신장, 체중 값 누락. 확인요망.", "비만도 판정불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGrade1.Text = "0";
                txtSWeight.Text = "0";
                return;
            }
            else
            {
                nHEIGHT = FnHeight / 100;
                nBMI = FnWeight / (nHEIGHT * nHEIGHT);
                nBMI = Math.Round(nBMI, 2);
                if (FstrSex == "M")
                {
                    if (FnAge == 6)
                    {
                        FnAge = 7;
                    }
                    for (int i = 0; i < SS5.ActiveSheet.RowCount; i++)
                    {
                        if (long.Parse(SS5.ActiveSheet.Cells[i, 0].Text) == FnAge)
                        {
                            for (int j = 0; j < SS5.ActiveSheet.ColumnCount; j++)
                            {
                                if (nBMI < double.Parse(SS5.ActiveSheet.Cells[i, j].Text))
                                {
                                    if (j == 2)
                                    {
                                        j += 1;
                                    }
                                    strPan = SS5.ActiveSheet.Cells[0, j - 1].Text.Trim();
                                    break;
                                }
                            }
                            if (strPan == "")
                            {
                                strPan = "97";
                            }
                        }
                    }
                }
                else
                {
                    if (FnAge == 6)
                    {
                        FnAge = 7;
                    }
                    for (int i = 0; i < SS6.ActiveSheet.RowCount; i++)
                    {
                        if (long.Parse(SS6.ActiveSheet.Cells[i, 0].Text) == FnAge)
                        {
                            for (int j = 0; j < SS6.ActiveSheet.ColumnCount; j++)
                            {
                                if (nBMI < double.Parse(SS6.ActiveSheet.Cells[i, j].Text))
                                {
                                    if (j == 2)
                                    {
                                        j += 1;
                                    }
                                    strPan = SS6.ActiveSheet.Cells[0, j - 1].Text.Trim();
                                    break;
                                }
                            }
                            if (strPan == "")
                            {
                                strPan = "97";
                            }
                        }
                    }
                }
            }

            if (double.Parse(strPan) >= 85 && double.Parse(strPan) < 95)
            {
                lblPan0.Text = "과체중";
                txtGrade1.Text = "2";
            }
            else if (double.Parse(strPan) >= 95)
            {
                lblPan0.Text = "비만";
                txtGrade1.Text = "3";
            }
            else if (double.Parse(strPan) < 5)
            {
                lblPan0.Text = "저체중";
                txtGrade1.Text = "4";
            }
            else
            {
                lblPan0.Text = "정상";
                txtGrade1.Text = "1";
            }

            if (nBMI > 25)
            {
                lblPan0.Text = "비만";
            }

            lblPan24.Text = nBMI.ToString();
            strBiman = hm.CALC_Biman_Rate(double.Parse(txtWeight.Text), double.Parse(txtHeight.Text), FstrSex);
            if (strBiman != "")
            {
                txtSWeight.Text = VB.Pstr(strBiman, ".", 1);
                lblPan1.Text = VB.Pstr(strBiman, ".", 2);
            }
        }

        void fn_Sangdam_Display(long nWrtNo)
        {
            int nRead = 0;
            string strGbn = "";

            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));

            if (list.Count > 0)
            {
                txtPRes1.Text = list[0].PPANB1.Trim();
                lblPan22.Text = hf.READ_Mush("2", txtPRes1.Text.Trim());
                txtPResEtc.Text = list[0].PPANB2.Trim();
                txtEJ1.Text = VB.Pstr(list[0].PPANC4.Trim(), "^^", 1);
                lblPanEye1.Text = hf.READ_Eye("2", txtEJ1.Text.Trim());
                txtEJ2.Text = VB.Pstr(list[0].PPANC4.Trim(), "^^", 2);
                lblPanEye2.Text = hf.READ_Eye("2", txtEJ2.Text.Trim());
                txtEJEtc.Text = VB.Pstr(list[0].PPANC4.Trim(), "^^", 3);
                txtHJ.Text = VB.Pstr(list[0].PPANC6.Trim(), "^^", 1);
                lblPan3.Text = hf.READ_Hear("2", txtHJ.Text.Trim());
                txtHJEtc.Text = VB.Pstr(list[0].PPANC6.Trim(), "^^", 2);
                txtM.Text = VB.Pstr(list[0].PPANC7.Trim(), "^^", 1);
                lblPan21.Text = hf.READ_Nose("2", txtM.Text.Trim());
                txtMEtc.Text = VB.Pstr(list[0].PPANC7.Trim(), "^^", 2);
                txtN.Text = VB.Pstr(list[0].PPANC8.Trim(), "^^", 1);
                lblPan4.Text = hf.READ_Neck("2", txtN.Text.Trim());
                txtNEtc.Text = VB.Pstr(list[0].PPANC8.Trim(), "^^", 2);
                txtS.Text = VB.Pstr(list[0].PPANC9.Trim(), "^^", 1);
                lblPan5.Text = hf.READ_Skin("2", txtS.Text.Trim());
                txtSEtc.Text = VB.Pstr(list[0].PPANC9.Trim(), "^^", 2);
                //기관능력
                txtOrgan0.Text = VB.Pstr(list[0].PPAND1.Trim(), "^^", 1);
                lblPan6.Text = hf.READ_YN_NEW("2", txtOrgan0.Text.Trim());
                txtOrgan0Etc.Text = VB.Pstr(list[0].PPAND1.Trim(), "^^", 2);
                txtOrgan1.Text = VB.Pstr(list[0].PPAND2.Trim(), "^^", 1);
                lblPan7.Text = hf.READ_YN_NEW("2", txtOrgan1.Text.Trim());
                txtOrgan1Etc.Text = VB.Pstr(list[0].PPAND2.Trim(), "^^", 2);
                txtOrgan2.Text = VB.Pstr(list[0].PPAND3.Trim(), "^^", 1);
                lblPan8.Text = hf.READ_YN_NEW("2", txtOrgan2.Text.Trim());
                txtOrgan2Etc.Text = VB.Pstr(list[0].PPAND3.Trim(), "^^", 2);
                txtOrgan3.Text = VB.Pstr(list[0].PPAND4.Trim(), "^^", 1);
                lblPan9.Text = hf.READ_YN_NEW("2", txtOrgan3.Text.Trim());
                txtOrgan3Etc.Text = VB.Pstr(list[0].PPAND4.Trim(), "^^", 2);
                txtOrgan4.Text = VB.Pstr(list[0].PPAND5.Trim(), "^^", 1);
                lblPan10.Text = hf.READ_YN_NEW("2", txtOrgan4.Text.Trim());
                txtOrgan4Etc.Text = VB.Pstr(list[0].PPAND5.Trim(), "^^", 2);
                txtOrgan5.Text = VB.Pstr(list[0].PPAND6.Trim(), "^^", 1);
                lblPan11.Text = hf.READ_YN_NEW("2", txtOrgan5.Text.Trim());
                txtOrgan5Etc.Text = VB.Pstr(list[0].PPAND6.Trim(), "^^", 2);
                //진촬및상담
                txtJ0.Text = VB.Pstr(list[0].PPANK1.Trim(), "^^", 1);
                lblPan17.Text = hf.READ_YN_1("2", txtJ0.Text.Trim());
                txtJEtc0.Text = VB.Pstr(list[0].PPANK1.Trim(), "^^", 2);
                txtJ1.Text = VB.Pstr(list[0].PPANK2.Trim(), "^^", 1);
                lblPan18.Text = hf.READ_Old1("2", txtJ1.Text.Trim());
                txtJEtc1.Text = VB.Pstr(list[0].PPANK2.Trim(), "^^", 2);
                txtJ2.Text = VB.Pstr(list[0].PPANK3.Trim(), "^^", 1);
                lblPan19.Text = hf.READ_YN_1("2", txtJ2.Text.Trim());
                txtJEtc2.Text = VB.Pstr(list[0].PPANK3.Trim(), "^^", 2);
                txtJ3.Text = VB.Pstr(list[0].PPANK4.Trim(), "^^", 1);
                lblPan20.Text = hf.READ_Old2("2", txtJ3.Text.Trim());
                txtJEtc3.Text = VB.Pstr(list[0].PPANK4.Trim(), "^^", 2);
            }

            txtRemark.Text = hicSangdamNewService.GetRemarkbyWrtNo(long.Parse(txtWrtNo.Text));
        }

        void fn_Screen_Display(long nWrtNo)
        {
            int nRead = 0;
            string strGbn = "";

            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));

            nRead = list.Count;
            if (nRead > 1)
            {
                MessageBox.Show("자료가 2건 이상입니다.확인하세요", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRead > 0)
            {
                //결과및판정
                txtHeight.Text = list[0].PPANA1.Trim();
                txtWeight.Text = list[0].PPANA2.Trim();
                txtGrade1.Text = list[0].PPANA3.Trim();
                txtSWeight.Text = list[0].PPANA4.Trim();
                txtPRes1.Text = list[0].PPANB1.Trim();
                lblPan22.Text = hf.READ_Mush("2", txtPRes1.Text.Trim());
                txtPResEtc.Text = list[0].PPANB2.Trim();
                txtNE1.Text = VB.Pstr(list[0].PPANC1.Trim(), "^^", 1);
                txtNE2.Text = VB.Pstr(list[0].PPANC1.Trim(), "^^", 2);
                txtE1.Text = VB.Pstr(list[0].PPANC2.Trim(), "^^", 1);
                txtE2.Text = VB.Pstr(list[0].PPANC2.Trim(), "^^", 2);
                txtColor.Text = list[0].PPANC3.Trim();  //색각
                lblPan2.Text = hf.READ_YN2("2", txtColor.Text.Trim());
                txtEJ1.Text = VB.Pstr(list[0].PPANC4.Trim(), "^^", 1);
                lblPanEye1.Text = hf.READ_Eye("2", txtEJ1.Text.Trim());
                txtEJ2.Text = VB.Pstr(list[0].PPANC4.Trim(), "^^", 2);
                lblPanEye2.Text = hf.READ_Eye("2", txtEJ2.Text.Trim());
                txtEJEtc.Text = VB.Pstr(list[0].PPANC4.Trim(), "^^", 3);
                txtH1.Text = VB.Pstr(list[0].PPANC5.Trim(), "^^", 1);
                lblPanH1.Text = hf.READ_YN2("2", txtH1.Text.Trim());
                txtH2.Text = VB.Pstr(list[0].PPANC5.Trim(), "^^", 2);
                lblPanH2.Text = hf.READ_YN2("2", txtH2.Text.Trim());
                txtHJ.Text = VB.Pstr(list[0].PPANC6.Trim(), "^^", 1);
                lblPan3.Text = hf.READ_Hear("2", txtHJ.Text.Trim());
                txtHJEtc.Text = VB.Pstr(list[0].PPANC6.Trim(), "^^", 2);
                txtM.Text = VB.Pstr(list[0].PPANC7.Trim(), "^^", 1);
                lblPan21.Text = hf.READ_Nose("2", txtM.Text.Trim());
                txtMEtc.Text = VB.Pstr(list[0].PPANC7.Trim(), "^^", 2);
                txtN.Text = VB.Pstr(list[0].PPANC8.Trim(), "^^", 1);
                lblPan4.Text = hf.READ_Neck("2", txtN.Text.Trim());
                txtNEtc.Text = VB.Pstr(list[0].PPANC8.Trim(), "^^", 2);
                txtS.Text = VB.Pstr(list[0].PPANC9.Trim(), "^^", 1);
                lblPan5.Text = hf.READ_Skin("2", txtS.Text.Trim());
                txtSEtc.Text = VB.Pstr(list[0].PPANC9.Trim(), "^^", 2);
                //기관능력
                txtOrgan0.Text = VB.Pstr(list[0].PPAND1.Trim(), "^^", 1);
                lblPan6.Text = hf.READ_YN_NEW("2", txtOrgan0.Text.Trim());
                txtOrgan0Etc.Text = VB.Pstr(list[0].PPAND1.Trim(), "^^", 2);
                txtOrgan1.Text = VB.Pstr(list[0].PPAND2.Trim(), "^^", 1);
                lblPan7.Text = hf.READ_YN_NEW("2", txtOrgan1.Text.Trim());
                txtOrgan1Etc.Text = VB.Pstr(list[0].PPAND2.Trim(), "^^", 2);
                txtOrgan2.Text = VB.Pstr(list[0].PPAND3.Trim(), "^^", 1);
                lblPan8.Text = hf.READ_YN_NEW("2", txtOrgan2.Text.Trim());
                txtOrgan2Etc.Text = VB.Pstr(list[0].PPAND3.Trim(), "^^", 2);
                txtOrgan3.Text = VB.Pstr(list[0].PPAND4.Trim(), "^^", 1);
                lblPan9.Text = hf.READ_YN_NEW("2", txtOrgan3.Text.Trim());
                txtOrgan3Etc.Text = VB.Pstr(list[0].PPAND4.Trim(), "^^", 2);
                txtOrgan4.Text = VB.Pstr(list[0].PPAND5.Trim(), "^^", 1);
                lblPan10.Text = hf.READ_YN_NEW("2", txtOrgan4.Text.Trim());
                txtOrgan4Etc.Text = VB.Pstr(list[0].PPAND5.Trim(), "^^", 2);
                txtOrgan5.Text = VB.Pstr(list[0].PPAND6.Trim(), "^^", 1);
                lblPan11.Text = hf.READ_YN_NEW("2", txtOrgan5.Text.Trim());
                txtOrgan5Etc.Text = VB.Pstr(list[0].PPAND6.Trim(), "^^", 2);
                //소변
                txtU1.Text = list[0].PPANE1.Trim();
                lblPan12.Text = hf.READ_URO("2", txtU1.Text.Trim());
                txtU2.Text = list[0].PPANE2.Trim();
                lblPan13.Text = hf.READ_URO("2", txtU2.Text.Trim());
                txtU3.Text = list[0].PPANE3.Trim();
                lblPan16.Text = hf.READ_URO("2", txtU3.Text.Trim());
                txtU4.Text = list[0].PPANE4.Trim();
                //혈액
                txtB1.Text = list[0].PPANF1.Trim();
                lblPan25.Text = hf.READ_HYEOLDANG(txtB1.Text.Trim());
                txtB2.Text = list[0].PPANF2.Trim();
                lblPan26.Text = hf.READ_TotalCholesterol(txtB2.Text.Trim());
                txtB3.Text = list[0].PPANF3.Trim();
                lblPan27.Text = hf.READ_AST(txtB3.Text.Trim(), FnAge);
                txtB4.Text = list[0].PPANF4.Trim();
                lblPan28.Text = hf.READ_ALT(txtB4.Text.Trim());
                txtB5.Text = list[0].PPANF5.Trim();
                lblPan29.Text = hf.READ_Hemoglobin(txtB5.Text.Trim(), FstrSex);
                txtB6.Text = VB.Pstr(list[0].PPANF6.Trim(), "^^", 1);
                if (VB.Pstr(list[0].PPANF6.Trim(), "^^", 2) != "")
                {
                    cboRH.SelectedIndex = int.Parse(VB.Left(VB.Pstr(list[0].PPANF6.Trim(), "^^", 2), 1));
                }
                txtX.Text = VB.Pstr(list[0].PPANG1.Trim(), "^^", 1);
                lblPan14.Text = hf.READ_Xray("2", txtX.Text.Trim());
                txtXEtc.Text = VB.Pstr(list[0].PPANG1.Trim(), "^^", 2);
                txtLiver.Text = list[0].PPANH1.Trim();
                lblPan15.Text = hf.READ_PM("2", txtLiver.Text.Trim());
                //혈압
                txtBH.Text = VB.Pstr(list[0].PPANJ1.Trim(), "^^", 1);
                txtBL.Text = VB.Pstr(list[0].PPANJ1.Trim(), "^^", 2);
                //진촬및상담
                txtJ0.Text = VB.Pstr(list[0].PPANK1.Trim(), "^^", 1);
                lblPan17.Text = hf.READ_YN_1("2", txtJ0.Text.Trim());
                txtJEtc0.Text = VB.Pstr(list[0].PPANK1.Trim(), "^^", 2);
                txtJ1.Text = VB.Pstr(list[0].PPANK2.Trim(), "^^", 1);
                lblPan18.Text = hf.READ_Old1("2", txtJ1.Text.Trim());
                txtJEtc1.Text = VB.Pstr(list[0].PPANK2.Trim(), "^^", 2);
                txtJ2.Text = VB.Pstr(list[0].PPANK3.Trim(), "^^", 1);
                lblPan19.Text = hf.READ_YN_1("2", txtJ2.Text.Trim());
                txtJEtc2.Text = VB.Pstr(list[0].PPANK3.Trim(), "^^", 2);
                txtJ3.Text = VB.Pstr(list[0].PPANK4.Trim(), "^^", 1);
                lblPan20.Text = hf.READ_Old2("2", txtJ3.Text.Trim());
                txtJEtc3.Text = VB.Pstr(list[0].PPANK4.Trim(), "^^", 2);
                txtPanDate.Text = list[0].RDATE.ToString();
                if (txtPanDate.Text == "")
                {
                    txtPanDate.Text = clsPublic.GstrSysDate;
                }
                if (long.Parse(list[0].GBPAN.Trim()) > 0)
                {
                    cboPan.SelectedIndex = int.Parse(list[0].GBPAN.Trim());
                }
                txtPanSogen.Text = list[0].PPANREMARK1.Trim();
                txtPanJochi.Text = list[0].PPANREMARK2.Trim();
                //판정의사


                txtPanDrNo.Text = list[0].PPANDRNO.ToString();
                if (txtPanDrNo.Text == "84571")
                {
                    txtPanDrNo.Text = "86651";
                }
                if (txtPanDrNo.Text != "")
                {
                    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                }
                if (txtPanDrNo.Text != "0")
                {
                    if (clsHcVariable.GnHicLicense != list[0].PPANDRNO)
                    {
                        btnOK.Enabled = false;
                    }
                    else
                    {
                        txtPanDrNo.Text = clsHcVariable.GnHicLicense.ToString();
                        lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                    }
                }
            }
        }

        void fn_Auto_Result_Macro()
        {
            string strDATA_ALL = "";
            string strData = "";
            string strDATA_ALL_2 = "";
            string strData_2 = "";
            string strPoint_1 = "";
            string strPoint_2 = "";
            string strHeight_Ment = "";     //키(가정에서 조치사항)
            string strHeight_Ment_2 = "";   //키(종합소견)
            string strHeight_Low = "";      //키(저신장)

            strDATA_ALL = "";
            strData = "";
            strDATA_ALL_2 = "";
            strData_2 = "";
            strPoint_1 = "정상(경계)-";
            strPoint_2 = "정밀검사요함-";
            txtPanJochi.Text = "";
            txtPanSogen.Text = "";
            strHeight_Ment = "♣ 신장(키)이 표준보다 작습니다. 정상적인 발육지연의 가능성도 있으나, 이상유무에 대한 소아청소년과에서 진료상담받아 보십시요";
            strHeight_Ment_2 = "신장(정상 표준 이하)";
            strHeight_Low = "";

            //신장(키)
            if (txtHeight.Text.Trim() != "")
            {
                if (FstrSex == "M")
                {
                    if (FnAge == 7 && double.Parse(txtHeight.Text) < 114.4)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 8 && double.Parse(txtHeight.Text) < 118.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 9 && double.Parse(txtHeight.Text) < 123.2)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 10 && double.Parse(txtHeight.Text) < 127.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 11 && double.Parse(txtHeight.Text) < 132.6)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 12 && double.Parse(txtHeight.Text) < 137.8)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 13 && double.Parse(txtHeight.Text) < 143.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 14 && double.Parse(txtHeight.Text) < 150.3)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 15 && double.Parse(txtHeight.Text) < 156.4)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 16 && double.Parse(txtHeight.Text) < 160.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 17 && double.Parse(txtHeight.Text) < 162.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 18 && double.Parse(txtHeight.Text) < 162.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                }
                else if (FstrSex == "F")
                {
                    if (FnAge == 7 && double.Parse(txtHeight.Text) <= 113.2)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 8 && double.Parse(txtHeight.Text) <= 117.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 9 && double.Parse(txtHeight.Text) < 122.3)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 10 && double.Parse(txtHeight.Text) < 127.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 11 && double.Parse(txtHeight.Text) < 133.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 12 && double.Parse(txtHeight.Text) < 139.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 13 && double.Parse(txtHeight.Text) < 145)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 14 && double.Parse(txtHeight.Text) < 148.4)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 15 && double.Parse(txtHeight.Text) < 149.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 16 && double.Parse(txtHeight.Text) < 150.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 17 && double.Parse(txtHeight.Text) < 151)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 18 && double.Parse(txtHeight.Text) < 151.6)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                }
            }

            //신장이 표준이하 색깔을 칠함
            if (strHeight_Low == "1")
            {
                txtHeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }
            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //비만도
            switch (txtSWeight.Text.Trim())
            {
                case "2":
                    strData = "♣ 비만증 소견이 있습니다. 고칼로리음식을 삼가며,식이요법 및 운동요법으로 체중을 관리하십시요.";
                    strData_2 = strPoint_1 + "경도 비만";
                    break;
                case "3":
                    strData = "♣ 비만과 관련하여 소아청소년과 진료 및 상담이 필요할 수 있습니다.";
                    strData_2 = strPoint_2 + "중증도 비만";
                    break;
                case "4":
                    strData = "♣ 비만과 관련하여 소아청소년과 진료 및 상담이 필요할 수 있습니다.";
                    strData_2 = strPoint_2 + "고도 비만";
                    break;
                default:
                    break;
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //과체중&저체중
            switch (txtGrade1.Text.Trim())
            {
                case "2":
                    strData = "♣ 칼로리가 많은 간식은 삼가하시고, 식사요법과 적절한 운동을 통해 과체중을 조절하십시오.";
                    strData_2 = strPoint_1 + "과체중";
                    break;
                case "4":
                    strData = "♣ 저체중(표준이하) 소견이 있습니다. 소아청소년과에서 진료 및 상담받아 보십시요(이상 혹은 정상변이 여부)";
                    strData_2 = strPoint_2 + "저체중(몸무게 정상 표준 이하)";
                    break;
                default:
                    break;
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //저신장,저체중
            if (strHeight_Low == "1" && txtGrade1.Text.Trim() == "4")
            {
                strData = "♣ 체중 및 신장이 표준에 미달됩니다. 정상적인 발육지연의 가능성도 있으나, 이상유무에 대한 소아청소년과에서 진료상담받아 보십시요";
                strData_2 = strPoint_2 + "신장(정상 표준 이하) 저체중(몸무게 정상 표준이하)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //근골격계
            if (txtPRes1.Text.Trim() != "1" && txtPRes1.Text.Trim() != "")
            {
                strData = "♣ 근골격계이상이 의심되니 증상 관찰 및 확인진료 하시고 필요시 정형외과전문의의 관리를 받으십시오.";
                strData_2 = strPoint_2 + "근골격계- 척추측만증(의심)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //시력(나안)
            if (double.Parse(txtNE2.Text) < 0.8 && txtNE2.Text.Trim() != "" && double.Parse(txtNE1.Text) < 0.8 && txtNE1.Text.Trim() != "")
            {
                strData = "♣ 시력저하 소견이 있으므로 안과진료 바랍니다. 안경을 착용하고 있지 않다면 안경 처방이 필요할 수도 있습니다.";
                strData_2 = strPoint_2 + "눈- 시력저하 (양안)";
            }
            else if (double.Parse(txtNE2.Text) < 0.8 && txtNE2.Text.Trim() != "")
            {
                strData = "♣ 편측 시력저하소견이 있으므로 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "눈- 시력저하 (우안)";
            }
            else if (double.Parse(txtNE1.Text) < 0.8 && txtNE1.Text.Trim() != "")
            {
                strData = "♣ 편측 시력저하소견이 있으므로 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "눈- 시력저하 (좌안)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //시력(교정)
            if (double.Parse(txtE2.Text) < 0.8 && txtE2.Text.Trim() != "" && double.Parse(txtE1.Text) < 0.8 && txtE1.Text.Trim() != "")
            {
                strData = "♣ 교정시력이 낮으므로 재교정을 위해 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "교정시력저하(양안)";
            }
            else if (double.Parse(txtE2.Text) < 0.8 && txtE2.Text.Trim() != "")
            {
                strData = "♣ 편측  교정시력이 낮으므로 재교정을 위해 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "교정시력저하 (우안)";
            }
            else if (double.Parse(txtE1.Text) < 0.8 && txtE1.Text.Trim() != "")
            {
                strData = "♣ 편측  교정시력이 낮으므로 재교정을 위해 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "교정시력저하 (좌안)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //청력 검사(귀)
            if ((txtH2.Text.Trim() == "2" || txtH2.Text.Trim() == "3") && (txtH1.Text.Trim() == "2" || txtH1.Text.Trim() == "3"))
            {
                strData = "♣ 청력저하 또는 측정불가소견이 있으므로 이비인후과 진료 바랍니다.";
                strData_2 = strPoint_2 + "귀- 청력저하 (양측)";
            }
            else if (double.Parse(txtH2.Text) < 0.8 && txtH2.Text.Trim() != "")
            {
                strData = "♣ 청력저하 또는 측정불가소견이 있으므로 이비인후과 진료 바랍니다.";
                strData_2 = strPoint_2 + "귀- 청력저하 (우측)";
            }
            else if (double.Parse(txtH1.Text) < 0.8 && txtH1.Text.Trim() != "")
            {
                strData = "♣ 청력저하 또는 측정불가소견이 있으므로 이비인후과 진료 바랍니다.";
                strData_2 = strPoint_2 + "귀- 청력저하 (좌측)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //색각이상
            switch (txtColor.Text.Trim())
            {
                case "2":
                case "3":
                    strData = "♣ 색각이상 또는 측정불가소견이 의심되니 추후 안과에서 진료하시어 재 확인하시기 바랍니다.";
                    strData_2 = strPoint_2 + "눈- 색각이상";
                    break;
                default:
                    break;
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //혈압(최고, 최저)
            if (lblPan23.Text == "정밀검사요함")
            {
                strData = "♣ 혈압이 높게 측정되었으므로 소아청소년과에서 진료상담 및 정기적으로 혈압측정 받으십시오.";
                strData_2 = strPoint_2 + "고혈압(의심)";
            }
            else if(lblPan23.Text == "정상(경계)")
            {
                strData = "♣ 혈압이 기준치를 약간 상회하고 있으나, 측정시 오차가 있을 수도 있습니다. 정기적 측정바랍니다.";
                strData_2 = strPoint_1 + "고혈압<정상(경계)>";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //요단백&요잠혈
            if (double.Parse(txtU1.Text) >= 3 && double.Parse(txtU2.Text) >= 3)
            {
                strData = "♣ 소변검사상 이상소견(뇨단백 및 뇨잠혈)이 있으니 소아청소년과에서 진료상담이 필요합니다(정밀검사필요).";
                strData_2 = strPoint_2 + "신장질환(의심) - 요잠혈양성(+) 및 요단백 양성(+)";
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF")); 
                txtU2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }
            else if (double.Parse(txtU1.Text) >= 3)
            {
                strData = "♣ 소변검사상 이상소견(뇨단백)이 있으니 소아청소년과에서 진료상담 및 재확인이 필요합니다(필요시 정밀검사)";
                strData_2 = strPoint_2 + "소변검사이상 - 요단백양성 (+)";
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }
            else if (double.Parse(txtU2.Text) >= 3)
            {
                strData = "♣ 소변검사상 이상소견(뇨잠혈)이 있으니 소아청소년과에서 진료상담 및 재확인이 필요합니다(필요시 정밀검사)";
                strData_2 = strPoint_2 + "소변검사이상 - 요잠혈 양성(+)";
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //당뇨질환(혈당)
            if (double.Parse(txtB1.Text) >= 100 && double.Parse(txtB1.Text) <= 125)
            {
                strData = "♣ 공복혈당수치가 경계수치로 약간 높으므로 소아청소년과에서 공복혈당에 대한 재확인바랍니다.";
                strData_2 = strPoint_1 + "당뇨질환 - 공복혈당장애<정상(경계)>";
            }
            else if (double.Parse(txtB1.Text) >= 126)
            {
                strData = "♣ 공복혈당수치가 당뇨병으로 진단되는 수치입니다. 소아청소년과에서 진료상담 및 재확인바랍니다(정밀검사요).";
                strData_2 = strPoint_2 + "당뇨질환(의심)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //고지혈증(총콜레스테롤)
            if (double.Parse(txtB2.Text) >= 200)
            {
                strData = "♣ 공복혈당수치가 경계수치로 약간 높으므로 소아청소년과에서 공복혈당에 대한 재확인바랍니다.";
                strData_2 = strPoint_1 + "당뇨질환 - 공복혈당장애<정상(경계)>";
            }
            else if (double.Parse(txtB2.Text) >= 171 && double.Parse(txtB2.Text) <= 199)
            {
                strData = "♣ 총콜레스테롤 수치가 경계수준입니다. 생활요법 및 소아청소년과에서 정기적 검진 받으십시오.";
                strData_2 = strPoint_1 + "고지혈증 - 고콜레스테롤증<정상(경계)>";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //간장질환(AST, ALT)
            if ((FnAge < 10 && double.Parse(txtB3.Text) > 55) || (FnAge >= 10 && double.Parse(txtB3.Text) > 45) || (double.Parse(txtB4.Text) > 45))
            {
                strData = "♣ 간기능검사 수치가 높으므로 소아청소년과에서 진료상담 및 추적재검사 받으십시오(필요시 정밀검사).";
                strData_2 = strPoint_2 + "간장질환 - 간기능이상(AST, ALT)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //빈혈(혈색소) - 여성만 해당
            if (FstrSex == "F" && double.Parse(txtB5.Text) < 12 && txtB5.Text.Trim() != "")
            {
                strData = "♣ 혈액 검사상 빈혈이 의심됩니다. 소아청소년과에서 진료상담 및 추적 관리받으십시오.";
                strData_2 = strPoint_2 + "빈혈증 - 혈색소 감소(정밀검사요함)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //간염 검사
            switch (txtLiver.Text.Trim())
            {
                case "2":
                    strData = "♣ 간염항원양성입니다. 추가적인 간기능 검사 및 정기적인 소아청소년과의  관리를 받으십시오.";
                    strData_2 = strPoint_2 + "B형간염검사 - B형 간염 항원(HBsAg) 양성(+)";
                    break;
                default:
                    break;
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //흉부방사선 검사
            switch (txtX.Text.Trim())
            {
                case "4":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                    strData_2 = strPoint_2 + "흉부방사선이상 - 폐결핵의심(경증)";
                    break;
                case "5":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                    strData_2 = strPoint_2 + "흉부방사선이상 - 폐결핵의심(중등증)";
                    break;
                case "6":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                    strData_2 = strPoint_2 + "흉부방사선이상 - 폐결핵의심(중증)";
                    break;
                case "7":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                    strData_2 = strPoint_2 + "흉부방사선이상 - 폐결핵의심";
                    break;
                case "8":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                  strData_2 = strPoint_2 + "흉부방사선이상- 척추측만증의심";
                    break;
                case "9":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                    strData_2 = strPoint_2 + "흉부방사선이상 - 심비대";
                    break;
                case "10":
                    strData = "♣ 흉부방사선검사상 이상소견 발견되오니 전문의 진료요합니다";
                    strData_2 = strPoint_2 + "흉부방사선이상 - 이상음영";
                    break;
                default:
                    break;
            }

            //흉요추측만증
            if (VB.InStr(txtXResult.Text, "판독소견: 흉요추측만증") > 0)
            {
                txtX.Text = "8";
                lblPan14.Text = hf.READ_Xray("2", txtX.Text.Trim());
                txtXEtc.Text = "흉요추측만증";
                strData = "♣ 척추측만증이 의심되니 증상 관찰 및 확인진료 하시고 필요시 척추전문의의 관리를 받으십시오.";
                strData_2 = strData_2 + "근골격계- 척추측만증(의심, 흉부방사선검사)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //귓병(1.없음,2.중이염,3.외이도염,4.기타)
            switch (txtHJ.Text.Trim())
            {
                case "2":
                    strData = "♣ 귓병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                    strData_2 = strPoint_2 + "귀- 중이염(의심)";
                    break;
                case "3":
                    strData = "♣ 귓병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                  strData_2 = strPoint_2 + "귀- 외이도염(의심)";
                    break;
                case "4":
                    strData = "♣ 귓병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                    strData_2 = strPoint_2 + "귀- 이상소견(의심)";
                    break;
                default:
                    break;
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //코(콧 병)
            switch (txtM.Text.Trim())
            {
                case "2":
                    strData = "♣ 콧병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                    strData_2 = strPoint_2 + "코- 부비동염(의심)";
                    break;
                case "3":
                    strData = "♣ 콧병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                    strData_2 = strPoint_2 + "코- 비염(의심)";
                    break;
                case "4":
                    strData = "♣ 콧병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                    strData_2 = strPoint_2 + "코- 이상소견(의심)";
                    break;
                default:
                    break;
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //목(목 병)
            if (txtN.Text.Trim() != "1" && txtN.Text.Trim() != "")
            {
                strData = "♣ 목병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                strData_2 = strPoint_2 + "목- 이상소견(의심)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //피부(피부병)
            if (txtS.Text.Trim() != "1" && txtS.Text.Trim() != "")
            {
                strData = "♣ 피부병진찰시 이상소견 의심되니 피부과 관리를 받으십시오.";
                strData_2 = strPoint_2 + "피부- 이상소견(의심)";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(호흡기)
            if (txtOrgan0.Text.Trim() != "1" && txtOrgan0.Text.Trim() != "")
            {
                strData = "♣ 호흡기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "호흡기 기관능력 이상소견(   )";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(순환기)
            if (txtOrgan1.Text.Trim() != "1" && txtOrgan1.Text.Trim() != "")
            {
                strData = "♣ 순환기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "순환기 기관능력 이상소견(   )";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(비뇨기)
            if (txtOrgan2.Text.Trim() != "1" && txtOrgan2.Text.Trim() != "")
            {
                strData = "♣ 비뇨기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "비뇨기 기관능력 이상소견(   )";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(소화기)
            if (txtOrgan3.Text.Trim() != "1" && txtOrgan3.Text.Trim() != "")
            {
                strData = "♣ 소화기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "소화기 기관능력 이상소견(   )";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(신경계)
            if (txtOrgan4.Text.Trim() != "1" && txtOrgan4.Text.Trim() != "")
            {
                strData = "♣ 신경계 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "신경계 기관능력 이상소견(   )";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(기타)
            if (txtOrgan5.Text.Trim() != "1" && txtOrgan5.Text.Trim() != "")
            {
                strData = "♣ 기타 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "기타 기관능력 이상소견(   )";
            }

            if (strData != "")
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (strData_2 != "")
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //가정에서 조치사항
            if (strDATA_ALL == "")
            {
                txtPanJochi.Text = "♣ 학생건강검사결과 특별한 이상소견 없습니다. 계속 건강을 유지하십시오.";
            }
            else
            {
                txtPanJochi.Text = strDATA_ALL;
            }

            //종합소견
            if (strDATA_ALL_2 == "")
            {
                txtPanSogen.Text = "정상소견입니다.";
            }
            else
            {
                txtPanSogen.Text = strDATA_ALL_2;
            }

            //종합판정 구분
            if (txtPanSogen.Text.IndexOf("정밀검사요함") > 0)
            {
                cboPan.SelectedIndex = 3;
            }
            else if (txtPanSogen.Text.IndexOf("정상(경계)") > 0)
            {
                cboPan.SelectedIndex = 2;
            }
            else
            {
                cboPan.SelectedIndex = 1;
            }
        }

        void frmHcSchoolCommonInput_ssPanjengDblClick(string argGubun, string strCommon)
        {
            if (argGubun == "1")
            {
                txtPanSogen.Text = strCommon;
            }
            else if (argGubun == "2")
            {
                txtPanJochi.Text = strCommon;
            }
        }
    }
}
