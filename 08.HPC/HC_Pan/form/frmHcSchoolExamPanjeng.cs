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
/// Class Name      : HC_Pan
/// File Name       : frmHcSchoolExamPanjeng.cs
/// Description     : 학생검진판정
/// Author          : 이상훈
/// Create Date     : 2020-02-04
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm학생판정.frm(Frm학생판정)" />

namespace HC_Pan
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

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcSchoolCommonInput FrmHcSchoolCommonInput = null;
        frmViewResult FrmViewResult = null;
        frmHcSangStudentCounsel FrmHcSangStudentCounsel = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;
        frmHaExamResultReg FrmHaExamResultReg = null;

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
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnMenuResult.Click += new EventHandler(eBtnClick);
            this.btnSet1.Click += new EventHandler(eBtnClick);
            this.btnSet2.Click += new EventHandler(eBtnClick);
            this.btnExam_result.Click += new EventHandler(eBtnClick);
            this.btnSang.Click += new EventHandler(eBtnClick);
            this.btnXrayResult.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnExam_result.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.cboPan.KeyPress += new KeyPressEventHandler(eCboKeyPress);

            this.txtPanDate.DoubleClick += new EventHandler(eTxtDblClick);

            this.txtHeight.Click += new EventHandler(eTxtClick);
            this.txtWeight.Click += new EventHandler(eTxtClick);
            this.txtGrade1.Click += new EventHandler(eTxtClick);
            this.txtSWeight.Click += new EventHandler(eTxtClick);
            this.txtPRes1.Click += new EventHandler(eTxtClick);
            this.txtNE2.Click += new EventHandler(eTxtClick);
            this.txtNE1.Click += new EventHandler(eTxtClick);
            this.txtE2.Click += new EventHandler(eTxtClick);
            this.txtE1.Click += new EventHandler(eTxtClick);
            this.txtH2.Click += new EventHandler(eTxtClick);
            this.lblPanH2.Click += new EventHandler(eTxtClick);
            this.txtH1.Click += new EventHandler(eTxtClick);
            this.txtColor.Click += new EventHandler(eTxtClick);
            this.txtBH.Click += new EventHandler(eTxtClick);
            this.txtBL.Click += new EventHandler(eTxtClick);
            this.txtU1.Click += new EventHandler(eTxtClick);
            this.txtU2.Click += new EventHandler(eTxtClick);
            this.txtB1.Click += new EventHandler(eTxtClick);
            this.txtB2.Click += new EventHandler(eTxtClick);
            this.txtB7.Click += new EventHandler(eTxtClick);
            this.txtB8.Click += new EventHandler(eTxtClick);
            this.txtB9.Click += new EventHandler(eTxtClick);
            this.txtB3.Click += new EventHandler(eTxtClick);
            this.txtB4.Click += new EventHandler(eTxtClick);
            this.txtB5.Click += new EventHandler(eTxtClick);
            this.txtX.Click += new EventHandler(eTxtClick);
            this.txtXEtc.Click += new EventHandler(eTxtClick);
            this.txtB6.Click += new EventHandler(eTxtClick);
            this.txtLiver.Click += new EventHandler(eTxtClick);
            this.txtU3.Click += new EventHandler(eTxtClick);
            this.txtU4.Click += new EventHandler(eTxtClick);
            this.txtEJ2.Click += new EventHandler(eTxtClick);
            this.txtEJ1.Click += new EventHandler(eTxtClick);
            this.txtHJ.Click += new EventHandler(eTxtClick);
            this.txtM.Click += new EventHandler(eTxtClick);
            this.txtN.Click += new EventHandler(eTxtClick);
            this.txtS.Click += new EventHandler(eTxtClick);
            this.txtOrgan0.Click += new EventHandler(eTxtClick);
            this.txtOrgan1.Click += new EventHandler(eTxtClick);
            this.txtOrgan2.Click += new EventHandler(eTxtClick);
            this.txtOrgan3.Click += new EventHandler(eTxtClick);
            this.txtOrgan4.Click += new EventHandler(eTxtClick);
            this.txtOrgan5.Click += new EventHandler(eTxtClick);
            this.txtJ0.Click += new EventHandler(eTxtClick);
            this.txtJ1.Click += new EventHandler(eTxtClick);
            this.txtJ2.Click += new EventHandler(eTxtClick);
            this.txtJ3.Click += new EventHandler(eTxtClick);

            this.txtPResEtc.Click += new EventHandler(eTxtClick);
            this.txtEJEtc.Click += new EventHandler(eTxtClick);
            this.txtHJEtc.Click += new EventHandler(eTxtClick);
            this.txtMEtc.Click += new EventHandler(eTxtClick);
            this.txtNEtc.Click += new EventHandler(eTxtClick);
            this.txtSEtc.Click += new EventHandler(eTxtClick);
            this.txtOrgan0Etc.Click += new EventHandler(eTxtClick);
            this.txtOrgan1Etc.Click += new EventHandler(eTxtClick);
            this.txtOrgan2Etc.Click += new EventHandler(eTxtClick);
            this.txtOrgan3Etc.Click += new EventHandler(eTxtClick);
            this.txtOrgan4Etc.Click += new EventHandler(eTxtClick);
            this.txtOrgan5Etc.Click += new EventHandler(eTxtClick);
            this.txtJEtc0.Click += new EventHandler(eTxtClick);
            this.txtJEtc1.Click += new EventHandler(eTxtClick);
            this.txtJEtc2.Click += new EventHandler(eTxtClick);
            this.txtJEtc3.Click += new EventHandler(eTxtClick);

            this.txtHeight.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWeight.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtGrade1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSWeight.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtPRes1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtNE2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtNE1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtE2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtE1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtH2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtH1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtColor.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtBH.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtBL.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB7.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB8.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB9.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB3.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB4.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB5.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtX.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtXEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtB6.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtLiver.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU3.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtU4.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtEJ2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtEJ1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtHJ.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtM.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtN.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtS.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan0.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan3.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan4.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan5.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ0.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJ3.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtPResEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtEJEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtHJEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtNEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSEtc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan0Etc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan1Etc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan2Etc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan3Etc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan4Etc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtOrgan5Etc.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJEtc0.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJEtc1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJEtc2.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJEtc3.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtHeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtGrade1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSWeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPRes1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtNE2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtNE1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtE2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtE1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtH2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.lblPanH2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtH1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtColor.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBH.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBL.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB7.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB8.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB9.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtX.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtXEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtB6.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLiver.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtU4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtEJ2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtEJ1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHJ.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtM.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtN.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtS.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJ0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJ1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJ2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJ3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtPResEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtEJEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHJEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtMEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtNEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan0Etc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan1Etc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan2Etc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan3Etc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan4Etc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOrgan5Etc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);


            this.txtHeight.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtWeight.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtGrade1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtSWeight.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtPRes1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtNE2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtNE1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtE2.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtE1.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtH2.LostFocus += new EventHandler(eTxtLostFocus);
            this.lblPanH2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtH1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtColor.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtBH.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtBL.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtU1.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtU2.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB1.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB2.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB7.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB8.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB9.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB3.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB4.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtB5.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtX.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtXEtc.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtB6.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtLiver.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtU3.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtU4.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtEJ2.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtEJ1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtHJ.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtM.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtN.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtS.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtOrgan0.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtOrgan1.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtOrgan2.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtOrgan3.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtOrgan4.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtOrgan5.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtJ0.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtJ1.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtJ2.LostFocus += new EventHandler(eTxtLostFocus);    
            this.txtJ3.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            fn_TextBox_Backcolor_Set();

            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
            else if (sender == txtColor)
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
                    //SendKeys.Send("{TAB}");
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
                if (txtJ0.Text.To<double>() == 1)
                {
                    //SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtJ2)
            {
                lblPan19.Text = hf.READ_YN_1("2", txtJ2.Text.Trim());
                if (txtJ2.Text.To<double>() == 1)
                {
                    //SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtJ1)
            {
                lblPan18.Text = hf.READ_Old1("2", txtJ1.Text.Trim());
                if (txtJ1.Text.To<double>() == 1)
                {
                    //SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtJ3)
            {
                lblPan20.Text = hf.READ_Old2("2", txtJ3.Text.Trim());
                if (txtJ3.Text.To<double>() == 1)
                {
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                    //SendKeys.Send("{TAB}");
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
                if (txtPanSogen.Text.Trim().Length > 300)
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
                    //SendKeys.Send("{TAB}");
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
                    FnHeight = txtHeight.Text.To<double>();
                    FnWeight = txtWeight.Text.To<double>();

                    if (!txtHeight.Text.IsNullOrEmpty() && !txtWeight.Text.IsNullOrEmpty())
                    {
                        //체질량
                        fn_ObesityLevel_AutoPangeng();
                        //상대체중
                        strBiman = hm.CALC_Biman_Rate(txtWeight.Text.To<double>(), txtHeight.Text.To<double>(), FstrSex);
                        if (!strBiman.IsNullOrEmpty())
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
                    if (txtSWeight.Text.To<double>() > 1)
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
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void fn_SelectMark_Set(TextBox txtBox)
        {
            if (txtBox.BackColor == Color.White)
            {
                txtBox.BackColor = Color.Aqua;
            }
        }

        void fn_TextBox_Backcolor_Set()
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)   
            {
                if (ctl is TextBox)     
                {
                    if (ctl.BackColor == Color.Aqua)
                    {
                        ctl.BackColor = Color.White;
                    }
                }
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            fn_TextBox_Backcolor_Set();

            if (sender == txtB1 || sender == txtB2 || sender == txtB3 || sender == txtB4 || sender == txtB5 || sender == txtB7 || sender == txtB8 || sender == txtB9 || sender == txtBH || sender == txtBL)
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
            fn_SelectMark_Set((TextBox)sender);
        }

        //void eTxtGotFocus(object sender, EventArgs e)
        //{
        //    fn_SelectMark_Set((TextBox)sender);
        //}

        void eTxtGotFocus(object sender, EventArgs e)
        {
            fn_SelectMark_Set((TextBox)sender);

            if (sender == txtB1 || sender == txtB2 || sender == txtB3 || sender == txtB4 || sender == txtB5 || sender == txtB7 || sender == txtB8 || sender == txtB9 || sender == txtBH || sender == txtBL)
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

            fn_SelectMark_Set((TextBox)sender);
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtB1)
            {
                if (txtB1.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (txtB1.Text.To<double>() >= 100 && txtB1.Text.To<double>() <= 125)
                {
                    lblPan25.Text = "정상(경계)";
                    txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (txtB1.Text.To<double>() >= 126)
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
                if (txtB2.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (txtB2.Text.To<double>() >= 200)
                {
                    lblPan26.Text = "정밀검사요함";
                    txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (txtB2.Text.To<double>() >= 171 && txtB2.Text.To<double>() <= 199)
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
                if (txtB3.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (FnAge < 10)
                {
                    if (txtB3.Text.To<double>() > 55)
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
                    if (txtB3.Text.To<double>() > 45)
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
                if (txtB4.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (txtB4.Text.To<double>() > 45)
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
                if (txtB5.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (FstrSex == "F")
                {
                    if (txtB5.Text.To<double>() >= 12)
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
            else if (sender == txtB7)
            {
                if (txtB7.Text.IsNullOrEmpty()) return;

                if (txtB7.Text.To<int>() >= 40 && txtB7.Text.To<int>() <= 45)
                {
                    lblPan30.Text = "정상(경계)";
                    txtB7.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (txtB7.Text.To<int>() < 40)
                {
                    lblPan30.Text = "정밀검사요함";
                    txtB7.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    lblPan30.Text = "정상";
                    txtB7.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtB8)
            {
                if (txtB8.Text.IsNullOrEmpty()) return;

                if (txtB8.Text.To<int>() >= 40 && txtB8.Text.To<int>() <= 45)
                {
                    lblPan31.Text = "정상(경계)";
                    txtB8.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (txtB8.Text.To<int>() < 40)
                {
                    lblPan31.Text = "정밀검사요함";
                    txtB8.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    lblPan31.Text = "정상";
                    txtB8.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtB9)
            {
                if (txtB9.Text.IsNullOrEmpty()) return;

                if (txtB9.Text.To<int>() >= 110 && txtB9.Text.To<int>() <= 129)
                {
                    lblPan32.Text = "정상(경계)";
                    txtB9.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (txtB9.Text.To<int>() >= 130)
                {
                    lblPan32.Text = "정밀검사요함";
                    txtB9.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    lblPan32.Text = "정상";
                    txtB9.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }
            else if (sender == txtBH)
            {
                if (txtBH.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (txtBH.Text.To<double>() >= 130)
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
                if (txtBL.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (txtBL.Text.To<double>() >= 130)
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
                if (txtColor.Text.IsNullOrEmpty())
                {
                    return;
                }
                if (txtColor.Text.To<double>() > 1)
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
                if (txtE1.Text.To<double>() <= 0.7 && !txtE1.Text.IsNullOrEmpty())
                {
                    txtE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
            }
            else if (sender == txtE2)
            {
                if (txtE2.Text.To<double>() <= 0.7 && !txtE2.Text.IsNullOrEmpty())
                {
                    txtE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
            }
            else if (sender == txtEJ1)
            {
                if (txtEJ1.Text.To<double>() > 1)
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
                if (txtEJ2.Text.To<double>() > 1)
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
                if (txtH1.Text.To<double>() > 1)
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
                if (txtH2.Text.To<double>() > 1)
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
                if (txtHJ.Text.To<double>() > 1)
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
                if (txtJ0.Text.To<double>() > 1)
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
                if (txtJ1.Text.To<double>() > 1)
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
                if (txtJ2.Text.To<double>() > 1)
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
                if (txtJ3.Text.To<double>() > 1)
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
                if (txtLiver.Text.To<double>() > 1)
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
                if (txtM.Text.To<double>() > 1)
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
                if (txtN.Text.To<double>() > 1)
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
                if (txtNE2.Text.To<double>() <= 0.7 && !txtNE2.Text.IsNullOrEmpty())
                {
                    txtNE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
            }
            else if (sender == txtOrgan0)
            {
                if (txtOrgan0.Text.To<double>() > 1)
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
                if (txtOrgan1.Text.To<double>() > 1)
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
                if (txtOrgan2.Text.To<double>() > 1)
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
                if (txtOrgan3.Text.To<double>() > 1)
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
                if (txtOrgan4.Text.To<double>() > 1)
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
                if (txtOrgan5.Text.To<double>() > 1)
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
                if (txtPRes1.Text.To<double>() > 1)
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
                if (txtS.Text.To<double>() > 1)
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
                if (txtU1.Text.To<double>() > 1)
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
                if (txtU2.Text.To<double>() > 1)
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
                if (txtU3.Text.To<double>() > 1)
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
                if (txtX.Text.To<double>() > 2)
                {
                    txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else
                {
                    txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFFF"));
                }
            }
            //fn_SelectMark_Set((TextBox)sender, "Lost");
        }

        void eFormLoad(object sender, EventArgs e)
        {
            txtName.Text = "";
            FstrEdit = "";

            SS1.ActiveSheet.RowCount = 13;
            SS2.ActiveSheet.RowCount = 13;
            SS3.ActiveSheet.RowCount = 49;
            SS4.ActiveSheet.RowCount = 49;
            SS5.ActiveSheet.RowCount = 13;
            SS6.ActiveSheet.RowCount = 13;

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

            cboPan.Items.Clear();
            cboPan.Items.Add(" ");
            cboPan.Items.Add("1.정상");
            cboPan.Items.Add("2.정상(경계)");
            cboPan.Items.Add("3.정밀검사요함");
            cboPan.SelectedIndex = 0;

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
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.To<string>() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
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
                FnHeight = txtHeight.Text.To<double>();
                FnWeight = txtWeight.Text.To<double>();
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

                FstrROWID = hicSchoolNewService.GetRowIdbyWrtNo(txtWrtNo.Text.To<long>());

                //접수내역에서 정보를 읽음
                HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

                if (!list.IsNullOrEmpty())
                {
                    strJumin = clsAES.DeAES(list.JUMIN2);    //주민번호
                    strSname = list.SNAME;                   //성명
                    strSex = list.SEX;                       //성별
                    nClass = list.CLASS;                            //학년
                    nBan = list.BAN;                                //반
                    nBun = list.BUN;                                //번
                    strSchool = list.GBN;                    //학교구분
                    strLtdCode = list.LTDCODE.To<string>();         //학교기호
                    strJepDate = list.JEPDATE;                      //접수일자
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
                strPPanF[6] = txtB7.Text.Trim();
                strPPanF[7] = txtB8.Text.Trim();
                strPPanF[8] = txtB9.Text.Trim();

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

                nPPanDrno = txtPanDrNo.Text.To<long>();
                //판정일
                strPanDate = txtPanDate.Text.Trim();

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_SCHOOL_NEW item = new HIC_SCHOOL_NEW();

                item.WRTNO = txtWrtNo.Text.To<long>();
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
                item.LTDCODE = strLtdCode.To<long>();
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
                item.ENTSABUN = clsType.User.IdNumber.To<long>();

                if (FstrROWID.IsNullOrEmpty())
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

                result = hicJepsuService.UpdatePanjengDrNoPanDatebyWrtNo(txtWrtNo.Text.To<long>(), nPPanDrno, strPanDate);

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

                Cursor.Current = Cursors.WaitCursor;

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
                item.LTDCODE = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                item.SANGDAMDRNO = clsHcVariable.GnHicLicense;

                List<HIC_JEPSU_PATIENT_SCHOOL_SANGDAM> list = hicJepsuPatientSchoolSangdamService.GetItembyJepDate(item, strGubun);

                nRead = list.Count;
                SSList.ActiveSheet.RowCount = nRead;
                progressBar1.Maximum = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "";
                    strJumin = clsAES.DeAES(list[i].JUMIN2);
                    SSList.ActiveSheet.Cells[i, 0].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                    strJepDate = list[i].JEPDATE;
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO.To<string>();
                    SSList.ActiveSheet.Cells[i, 4].Text = strJumin;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].SEX;
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].LTDCODE.To<string>();
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].CLASS;
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].BAN;
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].BUN;
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].AGE;
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].ROWID;
                    SSList.ActiveSheet.Cells[i, 12].Text = list[i].PTNO;

                    if (!list[i].RDATE.To<string>().IsNullOrEmpty() && !list[i].GBPAN.IsNullOrEmpty())
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
                    progressBar1.Value = i + 1;
                }

                Cursor.Current = Cursors.Default;
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
                if (list.RESULT1.IsNullOrEmpty())
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
                FrmHaExamResultReg = new frmHaExamResultReg();
                FrmHaExamResultReg.ShowDialog(this);
            }
        }

        void fn_Screen_Clear()
        {
            FstrROWID = "";
            FstrSex = "";
            FnAge = 0; FnHeight = 0; FnWeight = 0;
            FstrFlag = "";
            pnlExam.Enabled = false;

            //2020.09.23 테스트위해 막음
            //////////////btnOK.Enabled = false;
            //////////////if (clsHcVariable.GnHicLicense > 0)
            //////////////{
            //////////////    btnOK.Enabled = true;
            //////////////}

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
                Label lblPan = (Controls.Find("lblPan" + i.To<string>(), true)[0] as Label);
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
                TextBox txtOrgan = (Controls.Find("txtOrgan" + i.To<string>(), true)[0] as TextBox);
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
                TextBox txtJ = (Controls.Find("txtJ" + i.To<string>(), true)[0] as TextBox);
                TextBox txtJEtc = (Controls.Find("txtJEtc" + i.To<string>(), true)[0] as TextBox);
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
            if (txtWrtNo.Text.IsNullOrEmpty())
            {
                MessageBox.Show("해당환자를 먼저 선택하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboPan.Text.IsNullOrEmpty())
            {
                MessageBox.Show("판정란이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //흉부방사선 검사 있는 수검자만 확인함
            if (hicResultService.GetExCodebyWrtNoExCode(txtWrtNo.Text.To<long>()) > 0)
            {
                if (txtX.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("흉부방사선검사 결과란이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void fn_Result_OverChk()
        {
            nPan1 = 0;
            nPan2 = 0;

            //신장
            if (txtHeight.Text.IsNullOrEmpty())
            {
                txtHeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //체질량지수
            if (txtGrade1.Text.To<long>() > 1)
            {
                txtGrade1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                FstrFlag = "OK";
            }
            else
            {
                txtGrade1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //상대체중
            if (txtSWeight.Text.To<double>() > 1)
            {
                txtSWeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                FstrFlag = "OK";
            }
            else
            {
                txtSWeight.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //근골격
            if (txtPRes1.Text.To<double>() > 1)
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
            if (txtNE1.Text.To<double>() <= 0.7 && txtNE1.Text.To<double>() != 0)
            {
                txtNE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtNE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //시력(나안/우)
            if (txtNE2.Text.To<double>() <= 0.7 && txtNE2.Text.To<double>() != 0)
            {
                txtNE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtNE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //시력(교정/좌)
            if (txtE1.Text.To<double>() <= 0.7 && txtE1.Text.To<double>() != 0)
            {
                txtE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtE1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //시력(교정/우)
            if (txtE2.Text.To<double>() <= 0.7 && txtE2.Text.To<double>() != 0)
            {
                txtE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtE2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //청력(우)
            if (txtH1.Text.To<double>() > 1)
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
            if (txtH2.Text.To<double>() > 1)
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
            if (txtColor.Text.To<double>() > 1)
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
            if (txtU1.Text.To<double>() > 2)
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
            if (txtU2.Text.To<double>() > 2)
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
            if (txtB1.Text.To<double>() >= 100 && txtB1.Text.To<double>() <= 125)
            {
                txtB1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan1 += 1;
                lblPan25.Text = "정상(경계)";
            }
            else if (txtB1.Text.To<double>() >= 126)
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
            if (txtB2.Text.To<double>() > 170 && txtB2.Text.To<double>() < 200)
            {
                txtB2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan1 += 1;
                lblPan26.Text = "정상(경계)";
            }
            else if (txtB2.Text.To<double>() >= 200)
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

            //HDL
            if (txtB7.Text.To<double>() >= 40 && txtB7.Text.To<double>() < 45)
            {
                txtB7.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan1 += 1;
                lblPan30.Text = "정상(경계)";
            }
            else if (txtB7.Text.To<double>() >= 40)
            {
                txtB7.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan30.Text = "정밀검사요함";
            }
            else
            {
                txtB7.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan30.Text = "정상";
            }

            //TG
            if (txtB8.Text.To<double>() >= 90 && txtB8.Text.To<double>() < 130)
            {
                txtB8.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan1 += 1;
                lblPan31.Text = "정상(경계)";
            }
            else if (txtB8.Text.To<double>() >= 130)
            {
                txtB8.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan31.Text = "정밀검사요함";
            }
            else
            {
                txtB8.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan31.Text = "정상";
            }

            //LDL
            if (txtB9.Text.To<double>() >= 110 && txtB9.Text.To<double>() < 130)
            {
                txtB9.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan1 += 1;
                lblPan32.Text = "정상(경계)";
            }
            else if (txtB9.Text.To<double>() >= 130)
            {
                txtB9.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan32.Text = "정밀검사요함";
            }
            else
            {
                txtB9.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan32.Text = "정상";
            }

            //AST
            if (FnAge < 10)
            {
                if (txtB3.Text.To<double>() > 55)
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
                if (txtB3.Text.To<double>() > 45)
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
            if (txtB4.Text.To<double>() > 45)
            {
                txtB4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
                lblPan28.Text = "정밀검사요함";
            }
            else
            {
                txtB4.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                lblPan28.Text = "정상";
            }

            //혈색소 12이상 정상
            if (FstrSex == "F") //여자만 해당
            {
                if (txtB5.Text.To<double>() >= 12 || txtB5.Text.To<double>() == 0)
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
            if (txtX.Text.To<double>() >= 4 && txtX.Text.To<double>() <= 9)
            {
                txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtX.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //안질환
            if (txtEJ1.Text.To<double>() > 1)
            {
                txtEJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtEJ1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }
            if (txtEJ2.Text.To<double>() > 1)
            {
                txtEJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtEJ2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //귓병
            if (txtHJ.Text.To<double>() > 1)
            {
                txtHJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtHJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //콧병
            if (txtM.Text.To<double>() > 1)
            {
                txtM.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                nPan2 += 1;
            }
            else
            {
                txtM.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
            }

            //목병
            if (txtN.Text.To<double>() > 1)
            {
                txtN.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }

            //피부
            if (txtS.Text.To<double>() > 1)
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
                TextBox txtOrgan = (Controls.Find("txtOrgan" + i.To<string>(), true)[0] as TextBox);
                if (txtOrgan.Text.To<double>() == 2)
                {
                    txtOrgan.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    nPan1 += 1;
                }
                else if (txtOrgan.Text.To<double>() == 3)
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
                TextBox txtJ = (Controls.Find("txtJ" + i.To<string>(), true)[0] as TextBox);
                if (txtJ.Text.To<double>() > 1)
                {
                    txtJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                }
                else if (txtJ.Text.To<double>() == 3)
                {
                    txtJ.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                }
            }

            //간염
            if (txtLiver.Text.To<double>() > 1)
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
            if (txtU3.Text.To<double>() > 1)
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
                if (FstrEdit.IsNullOrEmpty())
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
            FnWRTNO = txtWrtNo.Text.To<long>();
            strJumin = VB.Left(SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim(), 6) + "-" + VB.Right(SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim(), 7);
            strSex = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();
            FstrSex = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();

            nClass = SSList.ActiveSheet.Cells[e.Row, 7].Text.To<long>();
            nBan = SSList.ActiveSheet.Cells[e.Row, 8].Text.To<long>();
            nBun = SSList.ActiveSheet.Cells[e.Row, 9].Text.To<long>();
            nAge = SSList.ActiveSheet.Cells[e.Row, 10].Text.To<long>();
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
            ssPatInfo.ActiveSheet.Cells[1, 3].Text = strSchoolName;
            ssPatInfo.ActiveSheet.Cells[1, 5].Text = nClass + "학년 " + nBan + "반 " + nBun + "번";

            //판독결과 조회
            eBtnClick(btnXrayResult, new EventArgs());

            if (rdoGubun1.Checked == true)  //신규
            {
                eBtnClick(btnExam_result, new EventArgs());
                fn_Sangdam_Display(txtWrtNo.Text.To<long>());
                txtPanDate.Text = clsHcVariable.GnHicLicense.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                txtPanDate.Text = clsPublic.GstrSysDate;
            }
            else
            {
                fn_Screen_Display(txtWrtNo.Text.To<long>());
                FnHeight = txtHeight.Text.To<double>();
                FnWeight = txtWeight.Text.To<double>();
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
                FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPano, FstrGjJong, FstrROWID);
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
            List<HIC_RESULT> list = hicResultService.GetExCodeResultbyOnlyWrtNo(txtWrtNo.Text.To<long>());

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strTemp1 = list[i].EXCODE.Trim();
                    strTemp2 = list[i].RESULT.Trim();
                    if (strTemp1 == "A101")
                    {
                        txtHeight.Text = list[i].RESULT.To<long>().To<string>();
                        strMsg += " 키, ";
                    }
                    if (strTemp1 == "A102")
                    {
                        txtHeight.Text = list[i].RESULT.To<long>().To<string>();
                        strMsg += " 몸무게, ";
                    }
                    if (strTemp1 == "A117") //체질량지수
                    {
                        strTemp2 = list[i].RESULT.Trim();
                        if (strTemp2.To<long>() < 25)
                        {
                            txtGrade1.Text = "1";
                        }
                        else if (strTemp2.To<long>() >= 25 && strTemp2.To<long>() < 30)
                        {
                            txtGrade1.Text = "2";
                        }
                        else if (strTemp2.To<long>() >= 30)
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
                        else if (list[i].RESULT.IsNullOrEmpty())
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
                        else if (list[i].RESULT.IsNullOrEmpty())
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
                        else if (strTemp2.IsNullOrEmpty())
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

                    if (strTemp1 == "A242") txtB7.Text = list[i].RESULT.Trim(); strMsg += " HDL, ";
                    if (strTemp1 == "A241") txtB8.Text = list[i].RESULT.Trim(); strMsg += " TG, ";
                    if (strTemp1 == "C404") txtB9.Text = list[i].RESULT.Trim(); strMsg += " LDL, ";

                    ////혈액형
                    //if (strTemp1 == "H840")
                    //{
                    //    switch (list[i].RESULT.To<long>())
                    //    {
                    //        case 1:
                    //            txtB6.Text = "A";
                    //            strMsg += " 혈액형검사, ";
                    //            break;
                    //        case 2:
                    //            txtB6.Text = "B";
                    //            strMsg += " 혈액형검사, ";
                    //            break;
                    //        case 3:
                    //            txtB6.Text = "O";
                    //            strMsg += " 혈액형검사, ";
                    //            break;
                    //        case 4:
                    //            txtB6.Text = "AB";
                    //            strMsg += " 혈액형검사, ";
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}

                    //if (strTemp1 == "H841")
                    //{
                    //    switch (list[i].RESULT.To<long>())
                    //    {
                    //        case 1:
                    //            cboRH.Text = "1.RH+";
                    //            strMsg += " 혈액형RH, ";
                    //            break;
                    //        case 2:
                    //            cboRH.Text = "2.RH-";
                    //            strMsg += " 혈액형RH, ";
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}

                    //if (strTemp1 == "A131")
                    //{
                    //    txtLiver.Text = list[i].RESULT;
                    //    strMsg += " 간염검사, ";
                    //}

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
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (FnAge == 6)
                    {
                        FnAge = 7;
                    }
                    if (FnAge == SS1.ActiveSheet.Cells[i, 0].Text.To<long>())
                    {
                        nCurRow = i;
                        break;
                    }
                }
                for (int i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
                {
                    if (FnHeight < SS1.ActiveSheet.Cells[nCurRow, i].Text.To<double>())
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
                    if (FnAge == SS2.ActiveSheet.Cells[i, 0].Text.To<long>())
                    {
                        nCurRow = i;
                        break;
                    }
                }
                for (int i = 1; i < SS2.ActiveSheet.ColumnCount; i++)
                {
                    if (FnHeight < SS2.ActiveSheet.Cells[nCurRow, i].Text.To<long>())
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
                if (SS3.ActiveSheet.Cells[nCurRow, i - 1].Text.Trim() == strTGrade)
                {
                    for (int j = nStart; j <= nStart + 3; j++)
                    {
                        if (txtBH.Text.To<double>() < SS3.ActiveSheet.Cells[j, i - 1].Text.To<double>())
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
                        else if (txtBH.Text.To<double>() == SS3.ActiveSheet.Cells[j, 1].Text.To<double>())
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
                if (SS4.ActiveSheet.Cells[0, i - 1].Text.Trim() == strTGrade)
                {
                    for (int j = nStart; j <= nStart + 3; j++)
                    {
                        if (txtBL.Text.Trim().To<double>() < SS4.ActiveSheet.Cells[j, i - 1].Text.Trim().To<double>())
                        {
                            if (j == nStart)
                            {
                                strLGrade = SS4.ActiveSheet.Cells[j, 1].Text.Trim();
                                break;
                            }
                            else if (j != nStart)
                            {
                                strLGrade = SS4.ActiveSheet.Cells[j - 1, 1].Text.Trim();
                                break;
                            }
                        }
                        else if (txtBL.Text.To<double>() == SS4.ActiveSheet.Cells[j, i - 1].Text.To<double>())
                        {
                            strLGrade = SS4.ActiveSheet.Cells[i - 1, 1].Text.Trim();
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

            if (strHGrade.To<double>() >= 90)
            {
                txtBH.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                //txtBH.BorderBackColor = &H8080FF;    
            }

            if (strLGrade.To<double>() >= 90)
            {
                txtBL.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                //txtBL.BorderBackColor = &H8080FF;
            }

            //혈압 판정 --------------------------------------------
            lblPan23.Text = "정상";

            if (txtBH.Text.To<double>() >= 130 || txtBL.Text.To<double>() >= 80)
            {
                lblPan23.Text = "정상(경계)";
            }

            if (strHGrade.To<long>() >= 90 && strHGrade.To<long>() <= 95)
            {
                lblPan23.Text = "정상(경계)";
            }

            if (strHGrade.To<long>() >= 90 && strLGrade.To<double>() <= 95)
            {
                lblPan23.Text = "정상(경계)";
            }

            if (strHGrade.To<long>() > 95 && strLGrade.To<double>() > 95)
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

            if (txtHeight.Text.To<double>() == 0 || txtWeight.Text.To<double>() == 0)
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
                    for (int i = 1; i < SS5.ActiveSheet.RowCount; i++)
                    {
                        if (SS5.ActiveSheet.Cells[i, 0].Text.To<long>() == FnAge)
                        {
                            for (int j = 1; j < SS5.ActiveSheet.ColumnCount; j++)
                            {
                                if (nBMI < SS5.ActiveSheet.Cells[i, j].Text.To<double>())
                                {
                                    if (j == 1)
                                    {
                                        j += 1;
                                    }
                                    strPan = SS5.ActiveSheet.Cells[0, j - 1].Text.Trim();
                                    break;
                                }
                            }
                            if (strPan.IsNullOrEmpty())
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
                    for (int i = 1; i < SS6.ActiveSheet.RowCount; i++)
                    {
                        if (SS6.ActiveSheet.Cells[i, 0].Text.To<long>() == FnAge)
                        {
                            for (int j = 1; j < SS6.ActiveSheet.ColumnCount; j++)
                            {
                                if (nBMI < SS6.ActiveSheet.Cells[i, j].Text.To<double>())
                                {
                                    if (j == 1)
                                    {
                                        j += 1;
                                    }
                                    strPan = SS6.ActiveSheet.Cells[0, j - 1].Text.Trim();
                                    break;
                                }
                            }
                            if (strPan.IsNullOrEmpty())
                            {
                                strPan = "97";
                            }
                        }
                    }
                }
            }

            if (strPan.To<double>() >= 85 && strPan.To<double>() < 95)
            {
                lblPan0.Text = "과체중";
                txtGrade1.Text = "2";
            }
            else if (strPan.To<double>() >= 95)
            {
                lblPan0.Text = "비만";
                txtGrade1.Text = "3";
            }
            else if (strPan.To<double>() < 5)
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

            lblPan24.Text = nBMI.To<string>();
            strBiman = hm.CALC_Biman_Rate(txtWeight.Text.To<double>(), txtHeight.Text.To<double>(), FstrSex);
            if (!strBiman.IsNullOrEmpty())
            {
                txtSWeight.Text = VB.Pstr(strBiman, ".", 1);
                lblPan1.Text = VB.Pstr(strBiman, ".", 2);
            }
        }

        void fn_Sangdam_Display(long nWrtNo)
        {
            int nRead = 0;
            string strGbn = "";

            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

            if (list.Count > 0)
            {
                txtPRes1.Text = list[0].PPANB1;
                lblPan22.Text = hf.READ_Mush("2", txtPRes1.Text.Trim());
                txtPResEtc.Text = list[0].PPANB2;
                txtEJ1.Text = VB.Pstr(list[0].PPANC4, "^^", 1);
                lblPanEye1.Text = hf.READ_Eye("2", txtEJ1.Text.Trim());
                txtEJ2.Text = VB.Pstr(list[0].PPANC4, "^^", 2);
                lblPanEye2.Text = hf.READ_Eye("2", txtEJ2.Text.Trim());
                txtEJEtc.Text = VB.Pstr(list[0].PPANC4, "^^", 3);
                txtHJ.Text = VB.Pstr(list[0].PPANC6, "^^", 1);
                lblPan3.Text = hf.READ_Hear("2", txtHJ.Text.Trim());
                txtHJEtc.Text = VB.Pstr(list[0].PPANC6, "^^", 2);
                txtM.Text = VB.Pstr(list[0].PPANC7, "^^", 1);
                lblPan21.Text = hf.READ_Nose("2", txtM.Text.Trim());
                txtMEtc.Text = VB.Pstr(list[0].PPANC7, "^^", 2);
                txtN.Text = VB.Pstr(list[0].PPANC8, "^^", 1);
                lblPan4.Text = hf.READ_Neck("2", txtN.Text.Trim());
                txtNEtc.Text = VB.Pstr(list[0].PPANC8, "^^", 2);
                txtS.Text = VB.Pstr(list[0].PPANC9, "^^", 1);
                lblPan5.Text = hf.READ_Skin("2", txtS.Text.Trim());
                txtSEtc.Text = VB.Pstr(list[0].PPANC9, "^^", 2);
                //기관능력
                txtOrgan0.Text = VB.Pstr(list[0].PPAND1, "^^", 1);
                lblPan6.Text = hf.READ_YN_NEW("2", txtOrgan0.Text.Trim());
                txtOrgan0Etc.Text = VB.Pstr(list[0].PPAND1, "^^", 2);
                txtOrgan1.Text = VB.Pstr(list[0].PPAND2, "^^", 1);
                lblPan7.Text = hf.READ_YN_NEW("2", txtOrgan1.Text.Trim());
                txtOrgan1Etc.Text = VB.Pstr(list[0].PPAND2, "^^", 2);
                txtOrgan2.Text = VB.Pstr(list[0].PPAND3, "^^", 1);
                lblPan8.Text = hf.READ_YN_NEW("2", txtOrgan2.Text.Trim());
                txtOrgan2Etc.Text = VB.Pstr(list[0].PPAND3, "^^", 2);
                txtOrgan3.Text = VB.Pstr(list[0].PPAND4, "^^", 1);
                lblPan9.Text = hf.READ_YN_NEW("2", txtOrgan3.Text.Trim());
                txtOrgan3Etc.Text = VB.Pstr(list[0].PPAND4, "^^", 2);
                txtOrgan4.Text = VB.Pstr(list[0].PPAND5, "^^", 1);
                lblPan10.Text = hf.READ_YN_NEW("2", txtOrgan4.Text.Trim());
                txtOrgan4Etc.Text = VB.Pstr(list[0].PPAND5, "^^", 2);
                txtOrgan5.Text = VB.Pstr(list[0].PPAND6, "^^", 1);
                lblPan11.Text = hf.READ_YN_NEW("2", txtOrgan5.Text.Trim());
                txtOrgan5Etc.Text = VB.Pstr(list[0].PPAND6, "^^", 2);
                //진촬및상담
                txtJ0.Text = VB.Pstr(list[0].PPANK1, "^^", 1);
                lblPan17.Text = hf.READ_YN_1("2", txtJ0.Text.Trim());
                txtJEtc0.Text = VB.Pstr(list[0].PPANK1, "^^", 2);
                txtJ1.Text = VB.Pstr(list[0].PPANK2, "^^", 1);
                lblPan18.Text = hf.READ_Old1("2", txtJ1.Text.Trim());
                txtJEtc1.Text = VB.Pstr(list[0].PPANK2, "^^", 2);
                txtJ2.Text = VB.Pstr(list[0].PPANK3, "^^", 1);
                lblPan19.Text = hf.READ_YN_1("2", txtJ2.Text.Trim());
                txtJEtc2.Text = VB.Pstr(list[0].PPANK3, "^^", 2);
                txtJ3.Text = VB.Pstr(list[0].PPANK4, "^^", 1);
                lblPan20.Text = hf.READ_Old2("2", txtJ3.Text.Trim());
                txtJEtc3.Text = VB.Pstr(list[0].PPANK4, "^^", 2);
            }

            txtRemark.Text = hicSangdamNewService.GetRemarkbyWrtNo(txtWrtNo.Text.To<long>());
        }

        void fn_Screen_Display(long nWrtNo)
        {
            int nRead = 0;
            string strGbn = "";

            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

            nRead = list.Count;
            if (nRead > 1)
            {
                MessageBox.Show("자료가 2건 이상입니다.확인하세요", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRead > 0)
            {
                //결과및판정
                txtHeight.Text = list[0].PPANA1;
                txtWeight.Text = list[0].PPANA2;
                txtGrade1.Text = list[0].PPANA3;
                txtSWeight.Text = list[0].PPANA4;
                txtPRes1.Text = list[0].PPANB1;
                lblPan22.Text = hf.READ_Mush("2", txtPRes1.Text.Trim());
                txtPResEtc.Text = list[0].PPANB2;
                txtNE1.Text = VB.Pstr(list[0].PPANC1, "^^", 1);
                txtNE2.Text = VB.Pstr(list[0].PPANC1, "^^", 2);
                txtE1.Text = VB.Pstr(list[0].PPANC2, "^^", 1);
                txtE2.Text = VB.Pstr(list[0].PPANC2, "^^", 2);
                txtColor.Text = list[0].PPANC3;  //색각
                lblPan2.Text = hf.READ_YN2("2", txtColor.Text.Trim());
                txtEJ1.Text = VB.Pstr(list[0].PPANC4, "^^", 1);
                lblPanEye1.Text = hf.READ_Eye("2", txtEJ1.Text.Trim());
                txtEJ2.Text = VB.Pstr(list[0].PPANC4, "^^", 2);
                lblPanEye2.Text = hf.READ_Eye("2", txtEJ2.Text.Trim());
                txtEJEtc.Text = VB.Pstr(list[0].PPANC4, "^^", 3);
                txtH1.Text = VB.Pstr(list[0].PPANC5, "^^", 1);
                lblPanH1.Text = hf.READ_YN2("2", txtH1.Text.Trim());
                txtH2.Text = VB.Pstr(list[0].PPANC5, "^^", 2);
                lblPanH2.Text = hf.READ_YN2("2", txtH2.Text.Trim());
                txtHJ.Text = VB.Pstr(list[0].PPANC6, "^^", 1);
                lblPan3.Text = hf.READ_Hear("2", txtHJ.Text.Trim());
                txtHJEtc.Text = VB.Pstr(list[0].PPANC6, "^^", 2);
                txtM.Text = VB.Pstr(list[0].PPANC7, "^^", 1);
                lblPan21.Text = hf.READ_Nose("2", txtM.Text.Trim());
                txtMEtc.Text = VB.Pstr(list[0].PPANC7, "^^", 2);
                txtN.Text = VB.Pstr(list[0].PPANC8, "^^", 1);
                lblPan4.Text = hf.READ_Neck("2", txtN.Text.Trim());
                txtNEtc.Text = VB.Pstr(list[0].PPANC8, "^^", 2);
                txtS.Text = VB.Pstr(list[0].PPANC9, "^^", 1);
                lblPan5.Text = hf.READ_Skin("2", txtS.Text.Trim());
                txtSEtc.Text = VB.Pstr(list[0].PPANC9, "^^", 2);
                //기관능력
                txtOrgan0.Text = VB.Pstr(list[0].PPAND1, "^^", 1);
                lblPan6.Text = hf.READ_YN_NEW("2", txtOrgan0.Text.Trim());
                txtOrgan0Etc.Text = VB.Pstr(list[0].PPAND1, "^^", 2);
                txtOrgan1.Text = VB.Pstr(list[0].PPAND2, "^^", 1);
                lblPan7.Text = hf.READ_YN_NEW("2", txtOrgan1.Text.Trim());
                txtOrgan1Etc.Text = VB.Pstr(list[0].PPAND2, "^^", 2);
                txtOrgan2.Text = VB.Pstr(list[0].PPAND3, "^^", 1);
                lblPan8.Text = hf.READ_YN_NEW("2", txtOrgan2.Text.Trim());
                txtOrgan2Etc.Text = VB.Pstr(list[0].PPAND3, "^^", 2);
                txtOrgan3.Text = VB.Pstr(list[0].PPAND4, "^^", 1);
                lblPan9.Text = hf.READ_YN_NEW("2", txtOrgan3.Text.Trim());
                txtOrgan3Etc.Text = VB.Pstr(list[0].PPAND4, "^^", 2);
                txtOrgan4.Text = VB.Pstr(list[0].PPAND5, "^^", 1);
                lblPan10.Text = hf.READ_YN_NEW("2", txtOrgan4.Text.Trim());
                txtOrgan4Etc.Text = VB.Pstr(list[0].PPAND5, "^^", 2);
                txtOrgan5.Text = VB.Pstr(list[0].PPAND6, "^^", 1);
                lblPan11.Text = hf.READ_YN_NEW("2", txtOrgan5.Text.Trim());
                txtOrgan5Etc.Text = VB.Pstr(list[0].PPAND6, "^^", 2);
                //소변
                txtU1.Text = list[0].PPANE1;
                lblPan12.Text = hf.READ_URO("2", txtU1.Text.Trim());
                txtU2.Text = list[0].PPANE2;
                lblPan13.Text = hf.READ_URO("2", txtU2.Text.Trim());
                txtU3.Text = list[0].PPANE3;
                lblPan16.Text = hf.READ_URO("2", txtU3.Text.Trim());
                txtU4.Text = list[0].PPANE4;
                //혈액
                txtB1.Text = list[0].PPANF1;
                lblPan25.Text = hf.READ_HYEOLDANG(txtB1.Text.Trim());
                txtB2.Text = list[0].PPANF2;
                lblPan26.Text = hf.READ_TotalCholesterol(txtB2.Text.Trim());
                txtB3.Text = list[0].PPANF3;
                lblPan27.Text = hf.READ_AST(txtB3.Text.Trim(), FnAge);
                txtB4.Text = list[0].PPANF4;
                lblPan28.Text = hf.READ_ALT(txtB4.Text.Trim());
                txtB5.Text = list[0].PPANF5;
                lblPan29.Text = hf.READ_Hemoglobin(txtB5.Text.Trim(), FstrSex);
                txtB6.Text = VB.Pstr(list[0].PPANF6, "^^", 1);
                if (!VB.Pstr(list[0].PPANF6.Trim(), "^^", 2).IsNullOrEmpty())
                {
                    cboRH.SelectedIndex = VB.Left(VB.Pstr(list[0].PPANF6, "^^", 2), 1).To<int>();
                }

                txtB7.Text = list[0].PPANF7;
                lblPan30.Text = hf.READ_HYEOLDANG(txtB7.Text.Trim());
                txtB8.Text = list[0].PPANF8;
                lblPan31.Text = hf.READ_HYEOLDANG(txtB8.Text.Trim());
                txtB9.Text = list[0].PPANF9;
                lblPan32.Text = hf.READ_HYEOLDANG(txtB9.Text.Trim());

                txtX.Text = VB.Pstr(list[0].PPANG1, "^^", 1);
                lblPan14.Text = hf.READ_Xray("2", txtX.Text.Trim());
                txtXEtc.Text = VB.Pstr(list[0].PPANG1, "^^", 2);
                txtLiver.Text = list[0].PPANH1;
                lblPan15.Text = hf.READ_PM("2", txtLiver.Text.Trim());
                //혈압
                txtBH.Text = VB.Pstr(list[0].PPANJ1, "^^", 1);
                txtBL.Text = VB.Pstr(list[0].PPANJ1, "^^", 2);
                //진촬및상담
                txtJ0.Text = VB.Pstr(list[0].PPANK1, "^^", 1);
                lblPan17.Text = hf.READ_YN_1("2", txtJ0.Text.Trim());
                txtJEtc0.Text = VB.Pstr(list[0].PPANK1, "^^", 2);
                txtJ1.Text = VB.Pstr(list[0].PPANK2, "^^", 1);
                lblPan18.Text = hf.READ_Old1("2", txtJ1.Text.Trim());
                txtJEtc1.Text = VB.Pstr(list[0].PPANK2, "^^", 2);
                txtJ2.Text = VB.Pstr(list[0].PPANK3, "^^", 1);
                lblPan19.Text = hf.READ_YN_1("2", txtJ2.Text.Trim());
                txtJEtc2.Text = VB.Pstr(list[0].PPANK3, "^^", 2);
                txtJ3.Text = VB.Pstr(list[0].PPANK4, "^^", 1);
                lblPan20.Text = hf.READ_Old2("2", txtJ3.Text.Trim());
                txtJEtc3.Text = VB.Pstr(list[0].PPANK4, "^^", 2);
                txtPanDate.Text = list[0].RDATE;
                if (txtPanDate.Text.IsNullOrEmpty())
                {
                    txtPanDate.Text = clsPublic.GstrSysDate;
                }
                if (list[0].GBPAN.To<long>() > 0)
                {
                    cboPan.SelectedIndex = list[0].GBPAN.To<int>();
                }
                txtPanSogen.Text = list[0].PPANREMARK1;
                txtPanJochi.Text = list[0].PPANREMARK2;
                //판정의사


                txtPanDrNo.Text = list[0].PPANDRNO.To<string>();
                if (txtPanDrNo.Text == "84571")
                {
                    txtPanDrNo.Text = "86651";
                }
                if (!txtPanDrNo.Text.IsNullOrEmpty())
                {
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                }
                if (txtPanDrNo.Text != "0")
                {
                    if (clsHcVariable.GnHicLicense != list[0].PPANDRNO)
                    {
                        //2020.09.23 테스트 위해 막음
                        //btnOK.Enabled = false;
                    }
                    else
                    {
                        txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                        lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    }
                }
            }

            txtRemark.Text = hicSangdamNewService.GetRemarkbyWrtNo(txtWrtNo.Text.To<long>());
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
            if (!txtHeight.Text.IsNullOrEmpty())
            {
                if (FstrSex == "M")
                {
                    if (FnAge == 7 && txtHeight.Text.To<double>() < 114.4)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 8 && txtHeight.Text.To<double>() < 118.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 9 && txtHeight.Text.To<double>() < 123.2)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 10 && txtHeight.Text.To<double>() < 127.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 11 && txtHeight.Text.To<double>() < 132.6)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 12 && txtHeight.Text.To<double>() < 137.8)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 13 && txtHeight.Text.To<double>() < 143.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 14 && txtHeight.Text.To<double>() < 150.3)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 15 && txtHeight.Text.To<double>() < 156.4)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 16 && txtHeight.Text.To<double>() < 160.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 17 && txtHeight.Text.To<double>() < 162.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 18 && txtHeight.Text.To<double>() < 162.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                }
                else if (FstrSex == "F")
                {
                    if (FnAge == 7 && txtHeight.Text.To<double>() <= 113.2)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 8 && txtHeight.Text.To<double>() <= 117.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 9 && txtHeight.Text.To<double>() < 122.3)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 10 && txtHeight.Text.To<double>() < 127.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 11 && txtHeight.Text.To<double>() < 133.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 12 && txtHeight.Text.To<double>() < 139.7)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 13 && txtHeight.Text.To<double>() < 145)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 14 && txtHeight.Text.To<double>() < 148.4)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 15 && txtHeight.Text.To<double>() < 149.9)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 16 && txtHeight.Text.To<double>() < 150.5)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 17 && txtHeight.Text.To<double>() < 151)
                    {
                        strData = strHeight_Ment;
                        strHeight_Low = "1";
                        strData_2 = strPoint_2 + "신장(정상 표준 이하)";
                    }
                    else if (FnAge == 18 && txtHeight.Text.To<double>() < 151.6)
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }
            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //근골격계
            if (txtPRes1.Text.Trim() != "1" && !txtPRes1.Text.IsNullOrEmpty())
            {
                strData = "♣ 근골격계이상이 의심되니 증상 관찰 및 확인진료 하시고 필요시 정형외과전문의의 관리를 받으십시오.";
                strData_2 = strPoint_2 + "근골격계- 척추측만증(의심)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //시력(나안)
            if (txtNE2.Text.To<double>() < 0.8 && !txtNE2.Text.IsNullOrEmpty() && txtNE1.Text.To<double>() < 0.8 && !txtNE1.Text.IsNullOrEmpty())
            {
                strData = "♣ 시력저하 소견이 있으므로 안과진료 바랍니다. 안경을 착용하고 있지 않다면 안경 처방이 필요할 수도 있습니다.";
                strData_2 = strPoint_2 + "눈- 시력저하 (양안)";
            }
            else if (txtNE2.Text.To<double>() < 0.8 && !txtNE2.Text.IsNullOrEmpty())
            {
                strData = "♣ 편측 시력저하소견이 있으므로 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "눈- 시력저하 (우안)";
            }
            else if (txtNE1.Text.To<double>() < 0.8 && !txtNE1.Text.IsNullOrEmpty())
            {
                strData = "♣ 편측 시력저하소견이 있으므로 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "눈- 시력저하 (좌안)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //시력(교정)
            if (txtE2.Text.To<double>() < 0.8 && !txtE2.Text.IsNullOrEmpty() && txtE1.Text.To<double>() < 0.8 && !txtE1.Text.IsNullOrEmpty())
            {
                strData = "♣ 교정시력이 낮으므로 재교정을 위해 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "교정시력저하(양안)";
            }
            else if (txtE2.Text.To<double>() < 0.8 && !txtE2.Text.IsNullOrEmpty())
            {
                strData = "♣ 편측  교정시력이 낮으므로 재교정을 위해 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "교정시력저하 (우안)";
            }
            else if (txtE1.Text.To<double>() < 0.8 && !txtE1.Text.IsNullOrEmpty())
            {
                strData = "♣ 편측  교정시력이 낮으므로 재교정을 위해 안과진료상담 바랍니다.";
                strData_2 = strPoint_2 + "교정시력저하 (좌안)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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
            else if (txtH2.Text.To<double>() < 0.8 && !txtH2.Text.IsNullOrEmpty())
            {
                strData = "♣ 청력저하 또는 측정불가소견이 있으므로 이비인후과 진료 바랍니다.";
                strData_2 = strPoint_2 + "귀- 청력저하 (우측)";
            }
            else if (txtH1.Text.To<double>() < 0.8 && !txtH1.Text.IsNullOrEmpty())
            {
                strData = "♣ 청력저하 또는 측정불가소견이 있으므로 이비인후과 진료 바랍니다.";
                strData_2 = strPoint_2 + "귀- 청력저하 (좌측)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //요단백&요잠혈
            if (txtU1.Text.To<double>() >= 3 && txtU2.Text.To<double>() >= 3)
            {
                strData = "♣ 소변검사상 이상소견(뇨단백 및 뇨잠혈)이 있으니 소아청소년과에서 진료상담이 필요합니다(정밀검사필요).";
                strData_2 = strPoint_2 + "신장질환(의심) - 요잠혈양성(+) 및 요단백 양성(+)";
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF")); 
                txtU2.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }
            else if (txtU1.Text.To<double>() >= 3)
            {
                strData = "♣ 소변검사상 이상소견(뇨단백)이 있으니 소아청소년과에서 진료상담 및 재확인이 필요합니다(필요시 정밀검사)";
                strData_2 = strPoint_2 + "소변검사이상 - 요단백양성 (+)";
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }
            else if (txtU2.Text.To<double>() >= 3)
            {
                strData = "♣ 소변검사상 이상소견(뇨잠혈)이 있으니 소아청소년과에서 진료상담 및 재확인이 필요합니다(필요시 정밀검사)";
                strData_2 = strPoint_2 + "소변검사이상 - 요잠혈 양성(+)";
                txtU1.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //당뇨질환(혈당)
            if (txtB1.Text.To<double>() >= 100 && txtB1.Text.To<double>() <= 125)
            {
                strData = "♣ 공복혈당수치가 경계수치로 약간 높으므로 소아청소년과에서 공복혈당에 대한 재확인바랍니다.";
                strData_2 = strPoint_1 + "당뇨질환 - 공복혈당장애<정상(경계)>";
            }
            else if (txtB1.Text.To<double>() >= 126)
            {
                strData = "♣ 공복혈당수치가 당뇨병으로 진단되는 수치입니다. 소아청소년과에서 진료상담 및 재확인바랍니다(정밀검사요).";
                strData_2 = strPoint_2 + "당뇨질환(의심)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //고지혈증(총콜레스테롤)
            if (txtB2.Text.To<double>() >= 200)
            {
                strData = "♣ 공복혈당수치가 경계수치로 약간 높으므로 소아청소년과에서 공복혈당에 대한 재확인바랍니다.";
                strData_2 = strPoint_1 + "당뇨질환 - 공복혈당장애<정상(경계)>";
            }
            else if (txtB2.Text.To<double>() >= 171 && txtB2.Text.To<double>() <= 199)
            {
                strData = "♣ 총콜레스테롤 수치가 경계수준입니다. 생활요법 및 소아청소년과에서 정기적 검진 받으십시오.";
                strData_2 = strPoint_1 + "고지혈증 - 고콜레스테롤증<정상(경계)>";
            }

            //고밀도지단백 콜레스테롤(HDL)
            if (txtB7.Text.To<double>() < 40)
            {
                strData = "♣ 고밀도지단백 콜레스테롤 수치가 높으므로 소아청소년과에서 추적 재검사 받으십시오(필요시 정밀검사).";
                strData_2 = strPoint_2 + "고밀도지단백 콜레스테롤증(의심)";
            }
            else if (txtB7.Text.To<double>() >= 40 && txtB7.Text.To<double>() <= 45)
            {
                strData = "♣ 고밀도지단백 콜레스테롤 수치가 경계수준입니다. 생활요법 및 소아청소년과에서 정기적 검진 받으십시오.";
                strData_2 = strPoint_1 + "고밀도지단백 콜레스테롤증<정상(경계)>";
            }

            //중성지방(TG)
            if (txtB8.Text.To<double>() >= 130)
            {
                strData = "♣ 중성지방 수치가 높으므로 소아청소년과에서 추적 재검사 받으십시오(필요시 정밀검사).";
                strData_2 = strPoint_2 + "중성지방(의심)";
            }
            else if (txtB8.Text.To<double>() >= 90 && txtB8.Text.To<double>() <= 129)
            {
                strData = "♣ 중성지방 수치가 경계수준입니다. 생활요법 및 소아청소년과에서 정기적 검진 받으십시오.";
                strData_2 = strPoint_1 + "중성지방<정상(경계)";
            }

            //저밀도지단백 콜레스테롤(LDL)
            if (txtB9.Text.To<double>() >= 130)
            {
                strData = "♣ 저밀도지단백 콜레스테롤 수치가 높으므로 소아청소년과에서 추적 재검사 받으십시오(필요시 정밀검사).";
                strData_2 = strPoint_2 + "저밀도지단백 콜레스테롤증(의심)";
            }
            else if (txtB9.Text.To<double>() >= 90 && txtB9.Text.To<double>() <= 129)
            {
                strData = "♣ 저밀도지단백 콜레스테롤 수치가 경계수준입니다. 생활요법 및 소아청소년과에서 정기적 검진 받으십시오.";
                strData_2 = strPoint_1 + "고지혈증 - 저밀도지단백 콜레스테롤증<정상(경계)>";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!@strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //간장질환(AST, ALT)
            if ((FnAge < 10 && txtB3.Text.To<double>() > 55) || (FnAge >= 10 && txtB3.Text.To<double>() > 45) || (txtB4.Text.To<double>() > 45))
            {
                strData = "♣ 간기능검사 수치가 높으므로 소아청소년과에서 진료상담 및 추적재검사 받으십시오(필요시 정밀검사).";
                strData_2 = strPoint_2 + "간장질환 - 간기능이상(AST, ALT)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //빈혈(혈색소) - 여성만 해당
            if (FstrSex == "F" && txtB5.Text.To<double>() < 12 && !txtB5.Text.IsNullOrEmpty())
            {
                strData = "♣ 혈액 검사상 빈혈이 의심됩니다. 소아청소년과에서 진료상담 및 추적 관리받으십시오.";
                strData_2 = strPoint_2 + "빈혈증 - 혈색소 감소(정밀검사요함)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
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

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //목(목 병)
            if (txtN.Text.Trim() != "1" && !txtN.Text.IsNullOrEmpty())
            {
                strData = "♣ 목병진찰시 이상소견 의심되니 이비인후과 관리를 받으십시오.";
                strData_2 = strPoint_2 + "목- 이상소견(의심)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //피부(피부병)
            if (txtS.Text.Trim() != "1" && !txtS.Text.IsNullOrEmpty())
            {
                strData = "♣ 피부병진찰시 이상소견 의심되니 피부과 관리를 받으십시오.";
                strData_2 = strPoint_2 + "피부- 이상소견(의심)";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(호흡기)
            if (txtOrgan0.Text.Trim() != "1" && !txtOrgan0.Text.IsNullOrEmpty())
            {
                strData = "♣ 호흡기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "호흡기 기관능력 이상소견(   )";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(순환기)
            if (txtOrgan1.Text.Trim() != "1" && !txtOrgan1.Text.IsNullOrEmpty())
            {
                strData = "♣ 순환기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "순환기 기관능력 이상소견(   )";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(비뇨기)
            if (txtOrgan2.Text.Trim() != "1" && !txtOrgan2.Text.IsNullOrEmpty())
            {
                strData = "♣ 비뇨기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "비뇨기 기관능력 이상소견(   )";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(소화기)
            if (txtOrgan3.Text.Trim() != "1" && !txtOrgan3.Text.IsNullOrEmpty())
            {
                strData = "♣ 소화기 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "소화기 기관능력 이상소견(   )";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(신경계)
            if (txtOrgan4.Text.Trim() != "1" && !txtOrgan4.Text.IsNullOrEmpty())
            {
                strData = "♣ 신경계 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "신경계 기관능력 이상소견(   )";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //기관능력(기타)
            if (txtOrgan5.Text.Trim() != "1" && !txtOrgan5.Text.IsNullOrEmpty())
            {
                strData = "♣ 기타 기관능력 이상소견(   ) 발견되오니 전문의 진료요합니다";
                strData_2 = strPoint_2 + "기타 기관능력 이상소견(   )";
            }

            if (!strData.IsNullOrEmpty())
            {
                strDATA_ALL += strData + "\r\n";
                strData = "";
            }

            if (!strData_2.IsNullOrEmpty())
            {
                strDATA_ALL_2 += strData_2 + "\r\n";
                strData_2 = "";
            }

            //가정에서 조치사항
            if (strDATA_ALL.IsNullOrEmpty())
            {
                txtPanJochi.Text = "♣ 학생건강검사결과 특별한 이상소견 없습니다. 계속 건강을 유지하십시오.";
            }
            else
            {
                txtPanJochi.Text = strDATA_ALL;
            }

            //종합소견
            if (strDATA_ALL_2.IsNullOrEmpty())
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
